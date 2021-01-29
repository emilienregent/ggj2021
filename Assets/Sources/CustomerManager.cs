using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    private int inGameIndex = 0;
    private Dictionary<int, CustomerController> inGameCustomers = new Dictionary<int, CustomerController>();
    private bool _customerSpawnStarted = false;

    public int maxCustomers = 0;
    public List<CustomerController> customerPrefabs = new List<CustomerController>();
    public Vector3 spawnPosition = Vector3.zero;
    public int inGameCustomersCount { get { return inGameCustomers.Count; } }

    // Update is called once per frame
    void Update()
    {
        StartSpawnCustomers();
    }

    public virtual void StartSpawnCustomers()
    {
        if (_customerSpawnStarted == false)
        {
            _customerSpawnStarted = true;
            StartCoroutine("SpawnCustomers");
        }
    }

    private IEnumerator SpawnCustomers()
    {
        while (true)
        {
            //float difficulty = _difficultyByTime.Evaluate(GameManager.instance.playTime / _playtimeTreshold);

            // From maximum delay (no difficulty) to minimum delay
            float delay = 1;

            //Debug.Log("Start for " + Time.timeSinceLevelLoad + "s\nDifficulty of " + difficulty + " --> Delay of " + delay + "s");

            yield return new WaitForSeconds(delay);

            int indexToUse = Random.Range(0, customerPrefabs.Count);
            if (inGameCustomersCount < maxCustomers)
            {
                SpawnCustomer(customerPrefabs[indexToUse]);
            }
        }
    }

    // Spawn a customer
    private CustomerController SpawnCustomer(CustomerController customerController)
    {
        CustomerController customer = null;

        // Search from the pool
        //for (int i = 0; i < availableItems.Count; ++i)
        //{
        //    if (availableItems[i].itemType == itemController.itemType)
        //    {
        //        // Get item from pool
        //        item = availableItems[i];

        //        // Remove item from pool
        //        availableItems.RemoveAt(i);

        //        break;
        //    }
        //}

        // Nothing in pull, create one
        if (customer == null)
        {
            // Create a new item
            customer = Instantiate(customerController);

            customer.Initialize(this);
        }

        customer.SetReady(++inGameIndex);

        // Add item to the dictionnary of in game items
        inGameCustomers.Add(customer.index, customer);

        return customer;
    }
}
