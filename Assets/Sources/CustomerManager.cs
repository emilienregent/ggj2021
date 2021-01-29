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

    void Start()
    {
            StartCoroutine("DebugZone");
    }

    private IEnumerator DebugZone()
    {
        yield return new WaitForSeconds(6);
        this.ReleaseCustomer(inGameCustomers[3]);

        yield return new WaitForSeconds(4);
        this.ReleaseCustomer(inGameCustomers[1]);

        yield return new WaitForSeconds(4);
        this.ReleaseCustomer(inGameCustomers[11]);
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
            float delay = 0.5f;

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

        // Nothing in pull, create one
        if (customer == null)
        {
            // Create a new item
            customer = Instantiate(customerController, this.transform);

            customer.Initialize(this);
        }

        customer.SetReady(++inGameIndex);

        // Add item to the dictionnary of in game items
        inGameCustomers.Add(customer.index, customer);

        return customer;
    }

    public void ReleaseCustomer(CustomerController customerController)
    {
        customerController.gameObject.SetActive(false);

        for (int i = customerController.index + 1; i < inGameIndex + 1; i++)
        {
            if (inGameCustomers.ContainsKey(i))
            {
                inGameCustomers[i].refreshQueuePosition();
            }
        }

        inGameCustomers.Remove(customerController.index);

    }
}
