using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{

    public Image icon;
    Item item;

    public delegate bool OnClickItem(Item item);
    public OnClickItem OnClickItemCallback;

    public void AddItem(Item newItem) {
        item = newItem;

        icon.sprite = item.icon;
        icon.enabled = true;
    }

    public void ClearSlot() {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
    }

    public void OnButtonClick() {
        OnClickItemCallback.Invoke(item);
    }

}
