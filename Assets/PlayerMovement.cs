using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float InputX;
    public float InputZ;
    public Vector3 desiredMoveDirection;
    public bool blockRotationPlayer;
    public float desiredRotationSpeed;
    private Animator anim;
    public float Speed;
    public float allowPlayerRotation;
    public Camera cam;
    public bool isGrounded;
    private float verticalVel;
    private Vector3 moveVector = Vector3.zero;
    public float gravity = 0.35F;
    public float jumpSpeed = 500.0F;
    private CharacterController controller;
    public bool jumping;
    // Start is called before the first frame update
    void Start()
    {
        anim = this.GetComponent<Animator>();
        controller = this.GetComponent<CharacterController>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        InputX = Input.GetAxis("Horizontal");
        InputZ = Input.GetAxis("Vertical");

        anim.SetBool("Jumping", jumping);
        
        InputMagnitude();
    }

    void FixedUpdate()
    {
        InputX = Input.GetAxis("Horizontal");
        InputZ = Input.GetAxis("Vertical");

        isGrounded = CheckIsGrounded();
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            jumping = true;
            moveVector.y = jumpSpeed;
        }
        else if(!isGrounded && moveVector.y > -1.0f)
        {
            moveVector.y -= gravity * Time.deltaTime;
        }
        else if(isGrounded)
        {
            jumping = false;
        }

        moveVector.x = InputX * 4 * Time.deltaTime;
        moveVector.z = InputZ * 4 * Time.deltaTime;
        controller.Move(new Vector3(0, moveVector.y * Time.deltaTime, 0));
        controller.Move(Quaternion.Euler(0, cam.transform.eulerAngles.y, 0) * (new Vector3(moveVector.x, 0, moveVector.z)));
    }

    void PlayerMoveAndRotation()
    {
        var forward = cam.transform.forward;
        var right = cam.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        desiredMoveDirection = forward * InputZ + right * InputX;

        if (blockRotationPlayer == false)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), desiredRotationSpeed);
        }
    }

    void InputMagnitude()
    {
        anim.SetFloat("InputZ", InputZ, 0.0f, Time.deltaTime * 2f);
        anim.SetFloat("InputX", InputX, 0.0f, Time.deltaTime * 2f);

        Speed = new Vector2(InputX, InputZ).sqrMagnitude;

        if (Speed > allowPlayerRotation)
        {
            anim.SetFloat("InputMagnitude", Speed, 0.0f, Time.deltaTime);
            PlayerMoveAndRotation();
        }
        else if(Speed < allowPlayerRotation)
        {
            anim.SetFloat("InputMagnitude", Speed, 0.0f, Time.deltaTime);
        }
    }

    private bool CheckIsGrounded()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity);
        if (hit.distance < 0.8f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
