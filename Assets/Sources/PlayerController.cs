using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const float DEATH_TRESHOLD = 5f;

    float _moveSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);

        if (transform.position.y < -DEATH_TRESHOLD)
        {
            transform.position = Vector3.zero;
        }
    }

    private void Move() {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(inputX, 0, inputY);
        transform.Translate(movement * _moveSpeed * Time.deltaTime, Space.World);

        if(movement != Vector3.zero)
        {
            transform.forward = movement;
        }
    }
}
