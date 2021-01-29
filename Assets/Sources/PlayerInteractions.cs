﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour {
    public Collider PlayerCollider;

    [Header("InteractableInfo")]
    public float sphereCastRadius = 0.5f;
    public int interactableItemLayer;
    public int interactableCustomerLayer;
    public Transform SphereCastPos;
    [SerializeField]
    public GameObject lookObject;
    [SerializeField]
    public GameObject lookCustomer;

    [Header("Pickup")]
    [SerializeField] private Transform pickupParent = null;
    public GameObject currentlyPickedUpObject;
    private Rigidbody pickupRB;
    [SerializeField]
    private float maxDistance = 0.3f;

    private void Start() {
    }

    //Interactable Object detections and distance check
    void Update() {
        //Here we check if we're currently looking at an interactable object
        RaycastHit hit;
        if(Physics.SphereCast(SphereCastPos.position, sphereCastRadius, transform.TransformDirection(Vector3.forward), out hit, maxDistance, 1 << interactableItemLayer))
        {
            lookObject = hit.collider.transform.gameObject;
        } 
        else
        {
            lookObject = null;
        }

        if (Physics.SphereCast(SphereCastPos.position, sphereCastRadius, transform.TransformDirection(Vector3.forward), out hit, maxDistance, 1 << interactableCustomerLayer))
        {
            lookCustomer = hit.collider.transform.gameObject;
        }
        else
        {
            lookCustomer = null;
        }

        //if we press the button of choice
        if (Input.GetButtonDown("Action"))
        {
            //and we're not holding anything
            if(currentlyPickedUpObject == null)
            {
                //and we are looking an interactable object
                if(lookObject != null)
                {
                    PickUpObject();
                }

            }
            //if we press the pickup button and have something, we drop it
            else
            {
                if (lookCustomer != null) // we are looking at a customer ? give the item
                {
                    CustomerController customer = lookCustomer.transform.GetComponent<CustomerController>();
                    CustomerManager.instance.CompleteCustomerRequest(customer);
                    currentlyPickedUpObject.SetActive(false);
                    currentlyPickedUpObject.transform.parent = null;
                    currentlyPickedUpObject = null;
                    pickupRB.drag = 1;
                    pickupRB.useGravity = true;
                } else
                {
                    BreakConnection();
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.tag == "Customer")
                {
                    CustomerController customer = hit.transform.GetComponent<CustomerController>();

                    CustomerManager.instance.CompleteCustomerRequest(customer);
                }

                // Do something with the object that was hit by the raycast.
            }
        }
    }

    //Release the object
    public void BreakConnection() {

        pickupRB.drag = 1;
        pickupRB.useGravity = true;
        currentlyPickedUpObject.transform.parent = null;
        pickupRB.constraints = RigidbodyConstraints.None;
        Physics.IgnoreCollision(PlayerCollider, currentlyPickedUpObject.GetComponent<BoxCollider>(), false);
        currentlyPickedUpObject = null;
    }

    public void PickUpObject() {
        currentlyPickedUpObject = lookObject;
        currentlyPickedUpObject.transform.position = pickupParent.position;
        pickupRB = currentlyPickedUpObject.GetComponent<Rigidbody>();
        pickupRB.drag = 10;
        pickupRB.useGravity = false;
        pickupRB.constraints = RigidbodyConstraints.FreezePosition;
        pickupRB.ResetInertiaTensor();
        currentlyPickedUpObject.transform.parent = pickupParent;

        Physics.IgnoreCollision(PlayerCollider, currentlyPickedUpObject.GetComponent<BoxCollider>(), true);
    }


}