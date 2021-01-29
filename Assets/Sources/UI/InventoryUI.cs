using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{

    public Inventory inventory;

    InventorySlot[] slots;

    // Start is called before the first frame update
    void Awake()
    {
        inventory.onItemChangedCallback += UpdateUI;

        slots = GetComponentsInChildren<InventorySlot>();

        for(int i = 0; i < slots.Length; i++)
        {
            slots[i].OnClickItemCallback += inventory.Remove;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateUI() {

        for(int i = 0; i < slots.Length; i++)
        {
            if(i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
            } else
            {
                slots[i].ClearSlot();
            }
        }
    }
}
