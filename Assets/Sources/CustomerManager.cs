using System.Collections;
using System.Collections.Generic;
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

    private Dictionary<int, CustomerController> inGameCustomers = new Dictionary<int, CustomerController>();
    private bool _customerSpawnStarted = false;
    private int[] slots = new int[10];

    public List<CustomerController> customerPrefabs = new List<CustomerController>();
    public Vector3 spawnPosition = Vector3.zero;
    public int inGameCustomersCount { get { return inGameCustomers.Count; } }

    // Update is called once per frame
    private void Start()
    {
        // Spawn first customer for FTUE
        SpawnCustomer(customerPrefabs[0]);
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

            if (inGameCustomersCount < slots.Length)
            {
                CustomerController customer = PickRandomCustomer();

                SpawnCustomer(customer);
            }
        }
    }

    private CustomerController PickRandomCustomer()
    {
        int indexToUse = Random.Range(0, customerPrefabs.Count);

        return customerPrefabs[indexToUse];
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

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] <= 0)
            {
                slots[i] = 1;
                customer.SetReady(i);
                break;
            }
        }

        // Add item to the dictionnary of in game items
        inGameCustomers.Add(customer.index, customer);
    }

    public void ReleaseCustomer(CustomerController customerController)
    {
        slots[customerController.index] = 0;

        customerController.gameObject.SetActive(false);

        inGameCustomers.Remove(customerController.index);
    }

    public void CompleteCustomerRequest(CustomerController customerController)
    {
        // Start real spawning once first customer has been completed
        if (GameManager.instance.currentScore == 0)
        {
            StartSpawnCustomers();
        }

        GameManager.instance.UpdateScore(customerController.score);

        ReleaseCustomer(customerController);
    }
}