using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public InventoryUI ChestUI;
    public PlayerInteractions Player;

    public int InventorySpace = 36;
    public List<Item> items = new List<Item>();


    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    private void Awake() {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInteractions>();
    }

    public bool Add(Item item) {

        if(items.Count >= InventorySpace)
        {
            Debug.Log("NOT ENOUGH SPACE");
            return false;
        }
        items.Add(item);

        if(onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
        
        return true;
    }

    public bool Remove(Item item) {

        if(items.Count < 1)
        {
            Debug.Log("CHEST IS EMPTY");
            return false;
        }
        items.Remove(item);
        if(onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
        Player.UnstoreObject(item.gameObject);
        ChestUI.gameObject.SetActive(false);
        return true;
    }

    private void OnCollisionExit(Collision collision) {
        if(collision.gameObject.tag == "Player")
        {
            ChestUI.gameObject.SetActive(false);
        }
    }
}
