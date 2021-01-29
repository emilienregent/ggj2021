using UnityEngine;

public class CustomerController : MonoBehaviour
{
    public int queuePosition = 0;

    private int _index = -1;
    private Vector3 _position = Vector3.zero;

    public int index { get { return _index; } }

    public void SetReady(int inGameIndex)
    {
        _index = inGameIndex;

        this.name = "Customer " + _index;

        queuePosition = CustomerManager.instance.inGameCustomersCount;

        _position = CustomerManager.instance.spawnPosition;

        transform.position = _position;
        transform.rotation = Quaternion.Euler(0f, 90f, 0f);

        gameObject.SetActive(true);
    }

    public void RefreshQueuePosition()
    {
        queuePosition--;
    }

    private void Update()
    {
        if(transform.position.x < 6.5 - (1.4* queuePosition))
        {
            _position.x += .03f;
            transform.position = _position;
        }

    }
}
