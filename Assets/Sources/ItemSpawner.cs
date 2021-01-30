using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public List<Item> AvailableItems = new List<Item>();

    [SerializeField]
    private List<Item> _pool = new List<Item>();

    public List<Item> Pool { get => _pool; set => _pool = value; }


    float timer;

    bool _canSpawn = true;

    private void OnDrawGizmos()
    {
        // Draw a semitransparent cube at the transforms position
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.position, new Vector3(1, 1, 1));
    }

    // Start is called before the first frame update
    private void Start()
    {
        timer = GameManager.instance.ItemSpawDelay;
        //if(AvailableItems.Count > 0)
        //{
        //    InvokeRepeating("SpawnItem", GameManager.instance.ItemSpawDelay, GameManager.instance.ItemSpawnInterval);
        //}
    }

    private void Update() {

        switch(GameManager.instance.CurrentGameState)
        {
            case GameState.Preparation:
                _canSpawn = true;
                break;
            case GameState.Gameover:
                _canSpawn = false;
                break;
            case GameState.Wave:
                _canSpawn = true;
                break;
            case GameState.Tutorial:
                if(Pool.Count >= 1)
                {
                    _canSpawn = false;
                }
                break;
        }

        if(_canSpawn)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                if(AvailableItems.Count > 0)
                {
                    SpawnItem();
                }
                timer = GameManager.instance.ItemSpawnInterval;
            }
        }
    }

    private void SpawnItem()
    {
        Item newItem = null;
        Item itemToInstantiate = AvailableItems[Random.Range(0, AvailableItems.Count)];

        // [TODO] Régler le problème de "respawn" quand un objet est reprit depuis la pool (tourne en continue sur le spawner)
        // First, check if we have this item in available in the pool
        //foreach(Item item in _pool)
        //{
        //    if(item.ItemType == itemToInstantiate.ItemType && item.CurrentState == ItemState.Queued)
        //    {
        //        newItem = item;
        //        newItem.gameObject.transform.SetParent(transform);
        //        newItem.transform.rotation = Quaternion.Euler(0, 0, 0);
        //        newItem.transform.localPosition = Vector3.zero;
        //        newItem.gameObject.SetActive(true);
        //        break; // We have found an item queued, we can leave
        //    }
        //}

        // No item found ?
        if (newItem == null)
        {
            newItem = Instantiate(itemToInstantiate, transform);

            _pool.Add(newItem);
        }
    }

    public Item RequestItem()
    {
        if(_pool.Count == 0)
        {
            return null;
        }

        System.Random rand = new System.Random();

        List<Item> availableItems = _pool.Where(item => item.CurrentState == ItemState.Available).ToList();

        if (availableItems != null)
        {
            return availableItems[rand.Next(availableItems.Count)];
        }

        return null;
    }
}