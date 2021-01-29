using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnEffect : MonoBehaviour
{

    public int RotationAngle = 45;
    public float RotationForce = 1000f;
    public float SideForce = 250f;

    // Start is called before the first frame update
    void Start()
    {
        SpawnEffect();
    }

    private void OnEnable()
    {
        //SpawnEffect();
    }

    public void SpawnEffect()
    {
        // Rotation
        transform.rotation = Quaternion.Euler(0, Random.Range(-1 * RotationAngle, RotationAngle), 0);

        // Force
        Rigidbody body = GetComponent<Rigidbody>();
        body.AddForce(-1 * transform.right * SideForce);
        body.AddTorque(0f, 0f, Random.Range(RotationForce / 2, RotationForce));

    }
}
