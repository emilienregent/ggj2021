using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    #region singleton
    public static CustomerManager instance { private set; get; }

    private void Awake()
    {
        // First destroy any existing instance of it
        if (instance != null)
        {
            Destroy(instance);
        }

        // Then reassign a proper one
        instance = this;
    }
    #endregion

    private Dictionary<int, CustomerController> _inGameCustomers = new Dictionary<int, CustomerController>();
    private bool _customerSpawnStarted = false;

    [SerializeField]
    private bool[] _slots;

    public List<CustomerController> CustomerPrefabs = new List<CustomerController>();
    public Vector3 SpawnPosition = Vector3.zero;
    public int InGameCustomersCount { get { return _inGameCustomers.Count; } }

    private float _timer;

    // Update is called once per frame
    private void Start()
    {
        // Max customers at the same time
        _slots = new bool[GameManager.instance.CustomerMaxNumber];

        _timer = GameManager.instance.CustomerSpawDelay;
    }

    private void Update() {
        switch(GameManager.instance.CurrentGameState)
        {
            case GameState.Preparation:
                ReleaseAllCustomers();
                _customerSpawnStarted = false;
                break;

            case GameState.Gameover:
                ReleaseAllCustomers();
                _customerSpawnStarted = false;
                break;

            case GameState.Wave:
                StartSpawnCustomers();
                break;

            case GameState.Tutorial:
                if(InGameCustomersCount <= 0)
                {
                    StartSpawnCustomers();
                } else
                {
                    _customerSpawnStarted = false;
                }
                break;
        }

        if(_customerSpawnStarted)
        {
            _timer -= Time.deltaTime;
            if(_timer <= 0)
            {
                SpawnCustomers();
                _timer = GameManager.instance.CustomerSpawnInterval;
            }
        }
       
    }

    private void StartSpawnCustomers()
    {
        if (_customerSpawnStarted == false && ItemSpawner.instance.Pool.Count > 0)
        {
            _customerSpawnStarted = true;
        }
    }
    
    private void SpawnCustomers()
    {
        if (InGameCustomersCount < _slots.Length)
        {
            CustomerController customer = PickRandomCustomer();

            SpawnCustomer(customer);
        }
    }

    private CustomerController PickRandomCustomer()
    {
        int indexToUse = Random.Range(0, CustomerPrefabs.Count);

        return CustomerPrefabs[indexToUse];
    }

    // Spawn a customer
    private void SpawnCustomer(CustomerController customerController)
    {
        CustomerController customer = null;

        // Nothing in pull, create one
        if (customer == null)
        {
            // Create a new item
            customer = Instantiate(customerController, this.transform);
        }

        for (int i = 0; i < _slots.Length; i++)
        {
            if(_slots[i] == false)
            {
                if (customer.Initialize(i))
                {
                    // Add item to the dictionnary of in game items
                    _inGameCustomers.Add(customer.Index, customer);
                    _slots[i] = true;
                } else
                {
                    GameObject.Destroy(customer.gameObject);
                }
                break;
            }
        }

    }

    public void ReleaseCustomer(CustomerController customerController)
    {
        customerController.CancelInvoke();
        _slots[customerController.Index] = false;

        _inGameCustomers.Remove(customerController.Index);

        customerController.Leave();

        //GameObject.Destroy(customerController.gameObject);
        //customerController.gameObject.SetActive(false);
    }

    public void ReleaseAllCustomers()
    {
        int currentCustomerCount = _inGameCustomers.Count;
        for (int i = 0; i < currentCustomerCount; i++)
        {
            ReleaseCustomer(_inGameCustomers.First().Value);
        }
    }

    public bool CompleteCustomerRequest(CustomerController customerController, Item item)
    {
        if (item.ItemType == customerController.CurrentRequest.item.ItemType)
        {
            // Start real spawning once first customer has been completed
            if (GameManager.instance.currentScore == 0)
            {
                StartSpawnCustomers();
            }

            GameManager.instance.UpdateScore(customerController.Score);

            ReleaseCustomer(customerController);
            return true;
        }

        return false;
    }
}