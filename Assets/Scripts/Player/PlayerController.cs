using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


    public float walkSpeed = 6f;
    public Camera cam;
    Rigidbody playerRigidbody;
    CapsuleCollider playerCollider;

    private bool isTargeting = false;
    private bool grounded = false;

	// Use this for initialization
	void Start () {
        playerCollider = GetComponent<CapsuleCollider>();
        playerRigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        float j = Input.GetAxisRaw("Jump");
        Move(h, v);
        Jump(j);
        respawn();
        isGrounded();
    }

    private void Move(float h, float v)
    {
        //Camera forward and right vectors
        Vector3 forward = cam.transform.forward;
        Vector3 right = cam.transform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        //Calculate movement and move
        Vector3 movement = forward * v + right * h;
        movement = movement * walkSpeed * Time.deltaTime;

        playerRigidbody.MovePosition(transform.position + movement);
        
        if (h > 0.1f || h < -0.1f || v > 0.1 || v < -0.1) {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(movement), Time.deltaTime * 10f);
        }
        
    }

    private void Jump(float j)
    {
        if (j > 0 && grounded)
        {
            playerRigidbody.velocity += new Vector3(0, 4f, 0);
        }
    }

    private bool isGrounded()
    {
        grounded = Physics.Raycast(transform.position, -Vector3.up, playerCollider.bounds.extents.y + 0.2f);
        return grounded;
    }

    private void respawn()
    {
        if (transform.position.y < -40)
        {
            transform.position = new Vector3(0, 3f, 0);
        }
    }


}
