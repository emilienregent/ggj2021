using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerController : MonoBehaviour
{
    private CustomerManager _manager = null;
    private int _index = -1;
    public int _queuePosition = 0;

    private Vector3 _position = Vector3.zero;

    public int index { get { return _index; } }

    public void Initialize(CustomerManager manager)
    {
        _manager = manager;
    }

    public void SetReady(int inGameIndex)
    {
        _index = inGameIndex;

        this.name = "Customer " + _index;

        _queuePosition = _manager.inGameCustomersCount;

        _position = _manager.spawnPosition;

        transform.position = _position;

        gameObject.SetActive(true);
    }

    public void refreshQueuePosition()
    {
        _queuePosition--;
    }

    private void Update()
    {
        if(transform.position.x < 6.5 - (1.4* _queuePosition))
        {
            _position.x += .03f;
            transform.position = _position;
        }

    }
}
