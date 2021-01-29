using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerController : MonoBehaviour
{
    private CustomerManager _manager = null;
    private int _index = -1;
    private int _customersCountAtSpawn = 0;

    private Vector3 _position = Vector3.zero;

    public int index { get { return _index; } }

    public void Initialize(CustomerManager manager)
    {
        _manager = manager;
        _customersCountAtSpawn = _manager.inGameCustomersCount;
    }

    public void SetReady(int inGameIndex)
    {
        _index = inGameIndex;

        _position = _manager.spawnPosition;

        transform.position = _position;

        gameObject.SetActive(true);
    }

    private void Update()
    {
        if(transform.position.x < 6.5 - (1.4* _customersCountAtSpawn))
        {
            _position.x += .02f;
            transform.position = _position;
        }

    }
}
