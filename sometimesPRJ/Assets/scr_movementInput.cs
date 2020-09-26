using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_movementInput : MonoBehaviour
{
    public float InputX;
    public float InputZ;
    public float InputRT;
    public Vector3 desiredMoveDirection;
    public bool blockRotationPlayer;
    public float desiredRotationSpeed;
    public float moveSpeedMultiplier;
    public Animator anim;
    public GameObject animObj;
    public float Speed;
    //public float sprintSpeed;
    public float allowPlayerRotation;
    public Camera cam;
    public CharacterController controller;
    public bool isGrounded;
    public bool isSprinting;
    public float sprintSpeedMult = 6.0f;
    public float walkSpeedMult = 2.0f;

    private Vector2 paramCheck;

    private float verticalVel;
    private Vector3 moveVector;

    // Start is called before the first frame update
    void Start()
    {
        anim = animObj.GetComponent<Animator>();
        cam = Camera.main;
        controller = this.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        InputMagnitude();

        if (anim.GetFloat("InputMagnitude") > 0.4 && anim.GetBool("canMove") == true && paramCheck != Vector2.zero)
        {
            anim.SetBool("isWalking", true);
            print("true");
        }
        else
        {
            anim.SetBool("isWalking", false);
            print("false");
        }


        if (Input.GetKey("left shift") || InputRT > 0.2)
        {
            isSprinting = true;
            anim.SetBool("isSprinting", true);
        }
        else
        {
            isSprinting = false;
            anim.SetBool("isSprinting", false);
        }
        //Debug.Log("Right Trigger");
        //Debug.Log(InputRT);

        //ground
        isGrounded = controller.isGrounded;
        if (isGrounded)
        {
            verticalVel -= 0;
        }
        else
        {
            verticalVel -= 2;
        }
        moveVector = new Vector3(0, verticalVel, 0);
        controller.Move(moveVector);

        paramCheck = new Vector2(InputX, InputZ);
    }

    void PlayerMoveAndRotation()
    {
        InputX = Input.GetAxis("Horizontal");
        InputZ = Input.GetAxis("Vertical");
        InputRT = Input.GetAxis("Right Trigger");

        var camera = Camera.main;
        var forward = cam.transform.forward;
        var right = cam.transform.right;

        forward.y = 0.0f;
        right.y = 0.0f;

        forward.Normalize();
        right.Normalize();

        desiredMoveDirection = forward * InputZ + right * InputX;

        if (blockRotationPlayer == false)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), desiredRotationSpeed);
            controller.Move(desiredMoveDirection * Time.deltaTime * moveSpeedMultiplier);
        }
    }

    void InputMagnitude()
    {
        //calc input vectors
        InputX = Input.GetAxisRaw("Horizontal");
        InputZ = Input.GetAxisRaw("Vertical");
        InputRT = Input.GetAxis("Right Trigger");

        anim.SetFloat("InputZ", InputZ, 0.0f, Time.deltaTime * 2.0f);
        anim.SetFloat("InputX", InputX, 0.0f, Time.deltaTime * 2.0f);



        //calc input magnitude
        Speed = new Vector2(InputX, InputZ).sqrMagnitude;

        //physically move player
        if (Speed > allowPlayerRotation && anim.GetBool("isSprinting") == false && anim.GetBool("canMove") == true && !anim.GetCurrentAnimatorStateInfo(0).IsTag("static"))
        {
            anim.SetFloat("InputMagnitude", Speed, 0.0f, Time.deltaTime);
            moveSpeedMultiplier = walkSpeedMult;
            PlayerMoveAndRotation();
        }
        else if (Speed > allowPlayerRotation && anim.GetBool("isSprinting") == true && anim.GetBool("canMove") == true && !anim.GetCurrentAnimatorStateInfo(0).IsTag("static"))
        {
            anim.SetFloat("InputMagnitude", Speed, 0.0f, Time.deltaTime);
            moveSpeedMultiplier = sprintSpeedMult;
            PlayerMoveAndRotation();
            //Debug.Log("springint");
        }
        else if (Speed < allowPlayerRotation)
        {
            anim.SetFloat("InputMagnitude", Speed, 0.0f, Time.deltaTime);
            moveSpeedMultiplier = 2;
        }
    }
}
