using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


    private float transitionSpeed = 4f;
    public float jumpHeight;
    public bool isTargeting;
    public float airSpeed;
    public Camera cam;
    Rigidbody playerRigidbody;
    CapsuleCollider playerCollider;
    Animator playerAnim;

    private bool grounded = false;

	// Use this for initialization
	void Start () {
        playerCollider = GetComponent<CapsuleCollider>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

        
    }

    private void FixedUpdate()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
        isGrounded();
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        
        Move(h, v);

        respawn();
        
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
        Vector3 movement = forward * v + right * h;


        if (!isTargeting)
        {
            //Calculate movement and move
            
            float forwardBack = Mathf.Lerp(Mathf.Abs(playerAnim.GetFloat("ForwardBack")), movement.magnitude, Time.deltaTime * transitionSpeed);
            float leftRight = Mathf.Lerp(playerAnim.GetFloat("LeftRight"), h, Time.deltaTime * transitionSpeed);
            playerAnim.SetFloat("ForwardBack", forwardBack);
            playerAnim.SetFloat("LeftRight", leftRight);

            if (h > 0.1f || h < -0.1f || v > 0.1 || v < -0.1)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(movement), Time.deltaTime * 10f);
            }
        } else
        {
            float forwardBack = Mathf.Lerp(playerAnim.GetFloat("ForwardBack"), v, Time.deltaTime * transitionSpeed);
            float leftRight = Mathf.Lerp(playerAnim.GetFloat("LeftRight"), h, Time.deltaTime * transitionSpeed);
            playerAnim.SetFloat("ForwardBack", forwardBack);
            playerAnim.SetFloat("LeftRight", leftRight);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(forward, Vector3.up), Time.deltaTime * 10f);
        }
        

        if (grounded)
        {
            playerRigidbody.MovePosition(transform.position + playerAnim.deltaPosition);
        } else
        {
            //playerRigidbody.AddForce(movement);
        }
        

        
    }

    private void Jump()
    {
        if (grounded) {

            //grounded = false;
            playerRigidbody.velocity += new Vector3(0f, jumpHeight, 0f);
            //playerRigidbody.velocity += playerAnim.deltaPosition * 1f / Time.deltaTime;
            playerAnim.SetTrigger("Jump");
        }
    }

    private bool isGrounded()
    {
        //grounded = Physics.Raycast(transform.position, -Vector3.up, playerCollider.bounds.extents.y + playerCollider.center.y + 0.2f);
        bool og = grounded;
        grounded = Physics.Raycast(transform.position + playerCollider.center, -Vector3.up, playerCollider.bounds.extents.y + 0.3f);
        if (og == true && grounded == false) {
            playerRigidbody.velocity += playerAnim.deltaPosition * 1f / Time.deltaTime;
        }
        
        playerAnim.SetBool("Grounded", grounded);
        return grounded;
    }

    private void respawn()
    {
        if (transform.position.y < -20)
        {
            transform.position = new Vector3(0, 3f, 0);
        }
    }


}
