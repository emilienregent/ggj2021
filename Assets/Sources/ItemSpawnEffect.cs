using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnEffect : MonoBehaviour
{

    public float UpForce = 1f;
    public float SideForce = .1f;

    // Start is called before the first frame update
    void Start()
    {

        float xForce = Random.Range(-SideForce, SideForce);
        float yForce = Random.Range(UpForce / 2f, UpForce);
        float zForce = Random.Range(-SideForce, SideForce);

        Vector3 force = new Vector3(xForce, yForce, zForce);

        GetComponent<Rigidbody>().velocity = force;

    }
}
