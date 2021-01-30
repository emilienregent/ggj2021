using System;
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
    private Vector3 _spawnPosition = Vector3.zero;
    private Vector3 _queuePosition = Vector3.zero;
    private ItemRequest _currentRequest = new ItemRequest();

    public int Index { get { return _index; } }

    public int Score { get { return Mathf.RoundToInt(_currentRequest.price * _currentRequest.satisfaction); } }

    public ItemRequest CurrentRequest { get { return _currentRequest; } }

    public bool Initialize(int inGameIndex)
    {
        _index = inGameIndex;
        _queuePosition = new Vector3(6.5f - (1.75f * _index), 0f, 3f);
        _spawnPosition = CustomerManager.instance.SpawnPosition;

        // Find an item available
        if (SetItemRequest())
        {
            Spawn();
            SetOutfit();

            bubble.SetActive(false);
            gameObject.SetActive(true);

            // Move to align with queue position
            MoveTo(new Vector3(_queuePosition.x, 0f, _spawnPosition.z), 1f).setOnComplete(MoveToDeskPosition);

            return true;
        }

        // If not abort the spawn
        return false;
    }

    public void Leave()
    {
        bubble.SetActive(false);

        LeanTween.rotateY(gameObject, 0f, 0.3f);

        MoveTo(new Vector3(transform.position.x, 0f, _spawnPosition.z), 0.5f).setOnComplete(MoveToExitPosition);
    }

    private LTDescr MoveTo(Vector3 position, float duration)
    {
        return LeanTween.move(gameObject, position, duration);
    }

    private void MoveToDeskPosition()
    {
        MoveTo(new Vector3(_queuePosition.x, 0f, _queuePosition.z), 0.5f);

        LeanTween.rotateY(gameObject, 180f, 0.3f).setOnComplete(OnRotationCompleted);
    }

    private void MoveToExitPosition()
    {
        MoveTo(new Vector3(_queuePosition.x + 10f, 0f, _spawnPosition.z), 1f).setOnComplete(OnExitPositionCompleted);
    }

    private void Spawn()
    {
        gameObject.name = "Customer " + _index;

        transform.position = _spawnPosition;
        transform.rotation = Quaternion.Euler(0f, 90f, 0f);
    }

    private void SetOutfit()
    {
        face.material = outfits[UnityEngine.Random.Range(0, outfits.Length)];
        body.material = outfits[UnityEngine.Random.Range(0, outfits.Length)];
    }

    private bool SetItemRequest()
    {
        _currentRequest.item = ItemSpawner.instance.RequestItem();

        if (_currentRequest.item != null)
        {
            _currentRequest.price = UnityEngine.Random.Range(priceMin, priceMax);
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

    private void OnExitPositionCompleted()
    {
        Destroy(gameObject);
    }

    private void OnRotationCompleted()
    {
        bubble.SetActive(true);
    }
}
