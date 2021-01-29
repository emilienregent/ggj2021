using System.Collections;
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

    bool _canInteractWithItem = true;

    private void Start() {
    }

    //Interactable Object detections and distance check
    void Update() {
        //Here we check if we're currently looking at an interactable object
        RaycastHit hit;
        if(Physics.SphereCast(SphereCastPos.position, sphereCastRadius, transform.TransformDirection(Vector3.forward), out hit, maxDistance, 1 << interactableItemLayer))
        {
            if (lookObject != null)
            {
                lookObject.transform.GetComponent<Item>().meshRenderer.material.SetFloat("_OutlineWidth", 0.0f);
            }
            lookObject = hit.collider.transform.gameObject;
            lookObject.transform.GetComponent<Item>().meshRenderer.material.SetFloat("_OutlineWidth", 0.02f);
        } 
        else
        {
            if (lookObject != null)
            {
                lookObject.transform.GetComponent<Item>().meshRenderer.material.SetFloat("_OutlineWidth", 0.0f);
            }
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
        if(Input.GetButtonDown("Action"))
        {
            if(_canInteractWithItem) { 
                if(currentlyPickedUpObject == null)
                {
                    if(lookObject != null)
                    {
                        PickUpObject();
                    }
                } else
                {
                    if(lookCustomer != null)
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
        pickupRB = null;
    }

    public void PickUpObject() {
        currentlyPickedUpObject = lookObject;
        currentlyPickedUpObject.transform.position = pickupParent.position;
        pickupRB = currentlyPickedUpObject.GetComponent<Rigidbody>();
        pickupRB.isKinematic = false;
        pickupRB.drag = 10;
        pickupRB.useGravity = false;
        pickupRB.constraints = RigidbodyConstraints.FreezePosition;
        currentlyPickedUpObject.transform.parent = pickupParent;

        Physics.IgnoreCollision(PlayerCollider, currentlyPickedUpObject.GetComponent<BoxCollider>(), true);
    }

    public void StoreObject() {
        pickupRB.isKinematic = true;
        currentlyPickedUpObject.transform.parent = null;
        currentlyPickedUpObject.gameObject.transform.position = new Vector3(-100, -100, -100);
        pickupRB = null;
        
        Physics.IgnoreCollision(PlayerCollider, currentlyPickedUpObject.GetComponent<BoxCollider>(), false);
        currentlyPickedUpObject = null;
    }

    public void UnstoreObject(GameObject storedObject) {
        lookObject = storedObject;
        PickUpObject();
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag == "Chest")
        {
            _canInteractWithItem = false;
        }
            
    }

    private void OnCollisionExit(Collision collision) {
        if(collision.gameObject.tag == "Chest")
        {
            _canInteractWithItem = true;
        }
    }

    private void OnCollisionStay(Collision collision) {
        if(collision.gameObject.tag == "Chest")
        {
            if(Input.GetButtonDown("Action"))
            {
                Inventory chest = collision.gameObject.GetComponent<Inventory>();
                chest.ChestUI.gameObject.SetActive(true);
                if(currentlyPickedUpObject != null)
                {
                    chest.Add(currentlyPickedUpObject.GetComponent<Item>());
                    StoreObject();  
                }
            }
        }
    }
}