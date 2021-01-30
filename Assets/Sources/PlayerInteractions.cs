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
    public Transform Handler = null;
    public GameObject CurrentItem;
    private Rigidbody _currentItemRigidbody;
    [SerializeField]
    private float maxDistance = 0.3f;

    bool _canInteractWithItem = true;
    private GameObject _currentChest;

    private void OnDrawGizmos()
    {
        // Draw a semitransparent cube at the transforms position
        Gizmos.color = new Color(0, 0, 1, 0.5f);
        Gizmos.DrawCube(Handler.transform.position, new Vector3(0.5f, 0.5f, 0.5f));
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
                if (CurrentItem == null)
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

                        if (CurrentItem != null)
                        {
                            Item item = CurrentItem.GetComponent<Item>();

                            if (CustomerManager.instance.CompleteCustomerRequest(customer, item))
                            {
                                CurrentItem.SetActive(false);
                                CurrentItem.transform.parent = null;
                                CurrentItem = null;
                                _currentItemRigidbody.drag = 1;
                                _currentItemRigidbody.useGravity = true;
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
                if (CurrentItem != null)
                {
                    chest.Add(CurrentItem.GetComponent<Item>());
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

        _currentItemRigidbody.drag = 1;
        _currentItemRigidbody.useGravity = true;
        CurrentItem.transform.parent = null;
        _currentItemRigidbody.constraints = RigidbodyConstraints.None;
        CurrentItem.transform.position += transform.forward * DropDistance;
        Physics.IgnoreCollision(PlayerCollider, CurrentItem.GetComponent<BoxCollider>(), false);
        CurrentItem = null;
        _currentItemRigidbody = null;
    }

    public void PickUpObject() {
        CurrentItem = lookedItem.gameObject;
        CurrentItem.transform.parent = Handler.transform;
        CurrentItem.transform.localPosition = Vector3.zero;
        CurrentItem.transform.rotation = Quaternion.Euler(Vector3.zero);
        _currentItemRigidbody = CurrentItem.GetComponent<Rigidbody>();
        _currentItemRigidbody.isKinematic = false;
        _currentItemRigidbody.drag = 10;
        _currentItemRigidbody.useGravity = false;
        _currentItemRigidbody.constraints = RigidbodyConstraints.FreezePosition;

        Physics.IgnoreCollision(PlayerCollider, CurrentItem.GetComponent<BoxCollider>(), true);
    }

    public void StoreObject() {
        _currentItemRigidbody.isKinematic = true;
        CurrentItem.transform.parent = null;
        CurrentItem.gameObject.transform.position = new Vector3(-100, -100, -100);
        _currentItemRigidbody = null;
        
        Physics.IgnoreCollision(PlayerCollider, CurrentItem.GetComponent<BoxCollider>(), false);
        CurrentItem = null;
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