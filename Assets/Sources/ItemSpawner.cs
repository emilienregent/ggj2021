using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    #region singleton
    public static ItemSpawner instance { private set; get; }

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

    [Header("Configuration")]
    public float SpawnFirstDelay = 1f;
    public float SpawnInterval = 2f;
    public List<Item> AvailableItems = new List<Item>();

    private List<Item> _pool = new List<Item>();

    public List<Item> Pool { get => _pool; set => _pool = value; }

    private void OnDrawGizmos()
    {
        // Draw a semitransparent cube at the transforms position
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.position, new Vector3(1, 1, 1));
    }

    // Start is called before the first frame update
    private void Start()
    {
        if(AvailableItems.Count > 0)
        {
            InvokeRepeating("SpawnItem", SpawnFirstDelay, SpawnInterval);
        }
    }

    private void SpawnItem()
    {
        Item newItem = null;
        Item itemToInstantiate = AvailableItems[Random.Range(0, AvailableItems.Count)];

        // First, check if we have this item in available in the pool
        foreach(Item item in _pool)
        {
            if(item.Name == itemToInstantiate.Name && item.CurrentState == ItemState.Queued)
            {
                newItem = item;
                newItem.transform.position = transform.position;
                break; // We have found an item queued, we can leave
            }
        }

        // No item found ?
        if(newItem == null)
        {
            newItem = Instantiate(itemToInstantiate, transform);

            _pool.Add(newItem);
        }
    }
}