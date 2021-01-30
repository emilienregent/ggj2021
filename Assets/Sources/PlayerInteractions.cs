using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour {
    public Collider PlayerCollider;
    public Animator PlayerAnimator;
    [Header("InteractableInfo")]
    public float sphereCastRadius = 0.5f;
    public int interactableItemLayer;
    public int interactableCustomerLayer;
    public Transform SphereCastPos;
    public float DropDistance;
    [SerializeField]
    public Item lookedItem;
    [SerializeField]
    public CustomerController lookCustomer;

    [Header("Pickup")]
    [SerializeField] private Transform pickupParent = null;
    public GameObject currentlyPickedUpObject;
    private Rigidbody pickupRB;
    [SerializeField]
    private float maxDistance = 0.3f;

    bool _canInteractWithItem = true;
    private GameObject _currentChest;

    private void Start() {
    }

    //Interactable Object detections and distance check
    void Update() {
        //Here we check if we're currently looking at an interactable object
        RaycastHit hit;
        if (lookedItem != null)
        {
            lookedItem.meshRenderer.material.SetFloat("_OutlineWidth", 0.0f);
        }

        if (Physics.SphereCast(SphereCastPos.position, sphereCastRadius, transform.TransformDirection(Vector3.forward), out hit, maxDistance, 1 << interactableItemLayer))
        {
            lookedItem = hit.collider.transform.GetComponent<Item>();
            lookedItem.meshRenderer.material.SetFloat("_OutlineWidth", 0.02f);
        } 
        else
        {
            lookedItem = null;
        }

        if (lookCustomer != null)
        {
            lookCustomer.face.material.SetFloat("_OutlineWidth", 0.0f);
            lookCustomer.body.material.SetFloat("_OutlineWidth", 0.0f);
        }

        if (Physics.SphereCast(SphereCastPos.position, sphereCastRadius, transform.TransformDirection(Vector3.forward), out hit, maxDistance, 1 << interactableCustomerLayer))
        {
            lookCustomer = hit.collider.transform.GetComponent<CustomerController>();
            if(lookCustomer != null)
            {
                lookCustomer.face.material.SetFloat("_OutlineWidth", 0.02f);
                lookCustomer.body.material.SetFloat("_OutlineWidth", 0.02f);
            }
        }
        else
        {
            lookCustomer = null;
        }

        //if we press the button of choice
        if (Input.GetButtonDown("Action"))
        {
            PlayerAnimator.SetTrigger("Pick");
            if (_canInteractWithItem)
            {
                if (currentlyPickedUpObject == null)
                {
                    if (lookedItem != null)
                    {
                        PickUpObject();
                    }
                }
                else
                {
                    if (lookCustomer != null)
                    {
                        CustomerController customer = lookCustomer;

                        if (currentlyPickedUpObject != null)
                        {
                            Item item = currentlyPickedUpObject.GetComponent<Item>();

                            if (CustomerManager.instance.CompleteCustomerRequest(customer, item))
                            {
                                currentlyPickedUpObject.SetActive(false);
                                currentlyPickedUpObject.transform.parent = null;
                                currentlyPickedUpObject = null;
                                pickupRB.drag = 1;
                                pickupRB.useGravity = true;
                            }
                        }
                    } else
                    {
                        BreakConnection();
                    }
                }
            }

            if (_currentChest != null)
            {
                Inventory chest = _currentChest.GetComponent<Inventory>();
                chest.ChestUI.gameObject.SetActive(true);
                if (currentlyPickedUpObject != null)
                {
                    chest.Add(currentlyPickedUpObject.GetComponent<Item>());
                    StoreObject();
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

                    CustomerManager.instance.CompleteCustomerRequest(customer, ItemSpawner.instance.RequestItem());
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
        currentlyPickedUpObject.transform.position += transform.forward * DropDistance;
        Physics.IgnoreCollision(PlayerCollider, currentlyPickedUpObject.GetComponent<BoxCollider>(), false);
        currentlyPickedUpObject = null;
        pickupRB = null;
    }

    public void PickUpObject() {
        currentlyPickedUpObject = lookedItem.gameObject;
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
        lookedItem = storedObject.transform.GetComponent<Item>();
        PickUpObject();
    }


    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Chest")
        {
            other.gameObject.transform.GetComponent<MeshRenderer>().material.SetFloat("_OutlineWidth", 0.02f);
            _currentChest = other.gameObject;
            _canInteractWithItem = false;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.tag == "Chest")
        {
            other.gameObject.transform.GetComponent<MeshRenderer>().material.SetFloat("_OutlineWidth", 0.0f);
            _currentChest = null;
            _canInteractWithItem = true;
        }
    }

}