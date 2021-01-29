using UnityEngine;

public class CustomerController : MonoBehaviour
{
    public struct ItemRequest
    {
        public int price;
        public int satisfaction;
    }

    public int queuePosition = 0;

    [Header("Model")]
    public MeshRenderer body = null;
    public MeshRenderer face = null;
    public Material[] outfits = null;

    [Header("Order")]
    public int priceMin = 0;
    public int priceMax = 0;

    private int _index = -1;
    private Vector3 _currentPosition = Vector3.zero;
    private ItemRequest _currentRequest = new ItemRequest();

    public int index { get { return _index; } }

    public int score { get { return Mathf.RoundToInt(_currentRequest.price * _currentRequest.satisfaction); } }

    public void SetReady(int inGameIndex)
    {
        _index = inGameIndex;
        _currentPosition = CustomerManager.instance.spawnPosition;

        Spawn();
        SetOutfit();
        SetItemRequest();

        gameObject.SetActive(true);
    }

    private void Spawn()
    {
        gameObject.name = "Customer " + _index;

        queuePosition = CustomerManager.instance.inGameCustomersCount;

        transform.position = _currentPosition;
        transform.rotation = Quaternion.Euler(0f, 90f, 0f);
    }

    private void SetOutfit()
    {
        face.material = outfits[Random.Range(0, outfits.Length)];
        body.material = outfits[Random.Range(0, outfits.Length)];
    }

    private void SetItemRequest()
    {
        _currentRequest.price = Random.Range(priceMin, priceMax);
        _currentRequest.satisfaction = 1; // Percentage to apply on price once request complete
    }

    public void RefreshQueuePosition()
    {
        queuePosition--;
    }

    private void Update()
    {
        if(transform.position.x < 6.5 - (1.4* queuePosition))
        {
            _currentPosition.x += .03f;
            transform.position = _currentPosition;
        }
    }
}
