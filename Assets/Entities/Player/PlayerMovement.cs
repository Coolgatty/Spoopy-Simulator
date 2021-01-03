using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float InputX;
    private float InputZ;
    private bool InputY;
    private Vector3 desiredMoveDirection;
    public bool blockRotationPlayer;
    public float desiredRotationSpeed;
    private Animator anim;
    private float speed;
    private Camera cam;
    private bool isGrounded;
    private float verticalVel;
    private Vector3 moveVector = Vector3.zero;
    public float gravity;
    public float jumpSpeed;
    private CharacterController controller;
    private bool jumping;

    EntityPlayer player;

    public int selectedSlot = 1;
    ProjectileSpawner spawner;
    // Start is called before the first frame update
    void Start()
    {
        anim = this.GetComponent<Animator>();
        controller = this.GetComponent<CharacterController>();
        cam = Camera.main;
        spawner = GetComponent<ProjectileSpawner>();
        player = GetComponent<EntityPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            selectedSlot = 1;
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            selectedSlot = 2;
        }

        spawner.switchSlot(selectedSlot);

        InputX = Input.GetAxis("Horizontal");
        InputZ = Input.GetAxis("Vertical");
        InputY = Input.GetButtonDown("Jump");
        isGrounded = CheckIsGrounded();
        if (isGrounded && InputY)
        {
            jumping = true;
            moveVector.y = jumpSpeed;
        }
        else if (!isGrounded && moveVector.y > -1.0f)
        {
            moveVector.y -= gravity * Time.deltaTime;
        }
        else if (isGrounded)
        {
            jumping = false;
        }
        anim.SetBool("Jumping", jumping);
        
        InputMagnitude();
    }

    void FixedUpdate()
    {
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

        speed = new Vector2(InputX, InputZ).sqrMagnitude;

        if (speed > 0)
        {
            anim.SetFloat("InputMagnitude", speed, 0.0f, Time.deltaTime);
            PlayerMoveAndRotation();
        }
        else if(speed < 0)
        {
            anim.SetFloat("InputMagnitude", speed, 0.0f, Time.deltaTime);
        }
    }

    private bool CheckIsGrounded()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer

        Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity);
        if (hit.distance < 0.5f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
