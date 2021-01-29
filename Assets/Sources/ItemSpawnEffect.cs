using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnEffect : MonoBehaviour
{

    public float RotationForce = 1000f;
    public float SideForce = 250f;

    // Start is called before the first frame update
    void Start()
    {

        Rigidbody body = GetComponent<Rigidbody>();

        body.AddForce(-1 * transform.right * SideForce);
        body.AddTorque(0f, 0f, Random.Range(RotationForce/2, RotationForce));

    }
}
