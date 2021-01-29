using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Available : Item is in game but not requested by a customer
// Requested : Item is in game but requested by a customer, can't be requested anymore
// Queued : Item is not in game anymore and ready to be "instantiate" again
public enum ItemState { Available, Requested, Queued };
public class Item : MonoBehaviour
{

    public string Name;
    public ItemState CurrentState;
    public Sprite icon;

    // Start is called before the first frame update
    void Start()
    {
        CurrentState = ItemState.Available;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDisable()
    {
        CurrentState = ItemState.Queued;
    }


}
