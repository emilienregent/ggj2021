using UnityEngine;
using UnityEngine.UI;

public class CustomerController : MonoBehaviour
{
    public struct ItemRequest
    {
        public int price;
        public float satisfaction;
        public Item item;
    }

    public int queuePosition = 0;

    [Header("Model")]
    public MeshRenderer body = null;
    public MeshRenderer face = null;
    public Material[] outfits = null;

    [Header("Request")]
    public GameObject bubble = null;
    public Image icon = null;
    public Image satisfactionGauge;
    public int priceMin = 0;
    public int priceMax = 0;
    [Tooltip("Decreasing value of satisfaction every second (float value from 0 to 1)")]
    public float SatisfactionStepOverTime = 0.01f;

    private int _index = -1;
    private int _tweenId = -1;
    private Vector3 _currentPosition = Vector3.zero;
    private Vector3 _queuePosition = Vector3.zero;
    private ItemRequest _currentRequest = new ItemRequest();

    public int Index { get { return _index; } }

    public int Score { get { return Mathf.RoundToInt(_currentRequest.price * _currentRequest.satisfaction); } }

    public ItemRequest CurrentRequest { get { return _currentRequest; } }

    public bool SetReady(int inGameIndex)
    {
        _index = inGameIndex;
        _queuePosition = new Vector3(6.5f - (1.75f * _index), 0f, 3f);
        _currentPosition = CustomerManager.instance.SpawnPosition;

        // Find an item available
        if (SetItemRequest())
        {
            Spawn();
            SetOutfit();

            bubble.SetActive(false);
            gameObject.SetActive(true);

            MoveToQueuePosition();

            return true;
        }

        // If not abort the spawn
        return false;
    }

    private void MoveToQueuePosition()
    {
        _tweenId = LeanTween.moveX(gameObject, _queuePosition.x, 1f).setOnComplete(OnQueuePositionCompleted).id;
    }

    private void MoveToDeskPosition()
    {
        _tweenId = LeanTween.moveZ(gameObject, _queuePosition.z, 0.5f).id;
    }

    private void Spawn()
    {
        gameObject.name = "Customer " + _index;

        transform.position = _currentPosition;
        transform.rotation = Quaternion.Euler(0f, 90f, 0f);
    }

    private void SetOutfit()
    {
        face.material = outfits[Random.Range(0, outfits.Length)];
        body.material = outfits[Random.Range(0, outfits.Length)];
    }

    private bool SetItemRequest()
    {
        _currentRequest.item = ItemSpawner.instance.RequestItem();

        if (_currentRequest.item != null)
        {
            _currentRequest.price = Random.Range(priceMin, priceMax);
            _currentRequest.satisfaction = 1f; // Percentage to apply on price once request complete, decrease over time
            satisfactionGauge.fillAmount = _currentRequest.satisfaction;

            InvokeRepeating("UpdateSatisfaction", 1f, 1f);

            icon.sprite = _currentRequest.item.icon;

            return true;
        }

        return false;
    }

    private void UpdateSatisfaction()
    {
        _currentRequest.satisfaction -= SatisfactionStepOverTime;
        satisfactionGauge.fillAmount = _currentRequest.satisfaction;

        if (_currentRequest.satisfaction <= 0f)
        {
            CustomerManager.instance.ReleaseCustomer(this);
        }
    }

    private void OnQueuePositionCompleted()
    {
        MoveToDeskPosition();

        LeanTween.rotateY(gameObject, 180f, 0.3f).setOnComplete(OnRotationCompleted);
    }

    private void OnRotationCompleted()
    {
        bubble.SetActive(true);
    }
}
