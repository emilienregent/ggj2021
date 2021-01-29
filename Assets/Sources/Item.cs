using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Available : Item is in game but not requested by a customer
// Requested : Item is in game but requested by a customer, can't be requested anymore
// Queued : Item is not in game anymore and ready to be "instantiate" again
public enum ItemState { Available, Requested, Queued };
public enum ItemType { Helmet, WizardHat };

public class Item : MonoBehaviour
{
    public string Name;
    public ItemType ItemType;
    public ItemState CurrentState;
    public Sprite icon;
    public MeshRenderer meshRenderer;

    // Start is called before the first frame update
    private void Start()
    {
        CurrentState = ItemState.Available;
    }

    private void OnEnable()
    {
        CurrentState = ItemState.Available;
    }

    private void OnDisable()
    {
        CurrentState = ItemState.Queued;
    }


}
