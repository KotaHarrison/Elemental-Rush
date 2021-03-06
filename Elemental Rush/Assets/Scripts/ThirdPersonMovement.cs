using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ThirdPersonMovement : MonoBehaviour
{
    public Animator animator;
    public CharacterController controller;
    public Transform cam;
    public Transform groundCheck;

    public float speed = 6f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public UnityEvent OnLandEvent;

    Vector3 velocity;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    bool isGrounded;
    bool canDoubleJump;
    //The function for what to do when jumping, the math behind calculating and applying the force for the jump.
    void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    private void FixedUpdate()
    {
        if(isGrounded == true)
        {
            OnLandEvent.Invoke();
        }

    }

    public void OnLanding()
    {
        animator.SetBool("IsAirbourne", false);
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        animator.SetFloat("Speed", Mathf.Abs(horizontal + vertical));

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded == true)
            {
                //Function that is called from above telling the system what to do
                Jump();
                canDoubleJump = true;
                animator.SetBool("IsAirbourne", true);

            }
            else if (Input.GetButtonDown("Jump") && canDoubleJump)
            {
                Jump();
                canDoubleJump = false;
            }
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
            
        }        
    }
}
