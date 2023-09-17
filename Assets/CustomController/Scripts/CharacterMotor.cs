
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class CharacterMotor : MonoBehaviour
{
    [Header("Movement Stats")]
    [SerializeField] private float jumpHeight = 5f;
    [SerializeField] private float jumpInputMultiplier = 5f;
    [SerializeField] private float jumpAirControlPower = 1f;
    [SerializeField] private float distanceTillLand = 0.05f;
    [SerializeField] private float distanceTillHardLand = 5;
    [SerializeField] private float groundedDistance = 0.02f;
    [Range(0f, 100f)]
    [SerializeField] private float rotationSpeed = 20f;
    [Range(0f, 0.5f)]
    [SerializeField] private float runSpeedDampTime = 0.05f;
    [SerializeField] Transform groundCheckObj;
    float distanceToGround;

    [Header("Stair Movement")]
    [SerializeField] GameObject stepRayUpper;
    [SerializeField] GameObject stepRayLower;
    [SerializeField] float stepHeight = 0.3f;
    [SerializeField] float stepSmooth = 2f;


    [Header("Movement Functions")]
    [SerializeField] private bool RotateInAir;
    [SerializeField] private bool ControlInAir;
    [SerializeField] private bool OnSpotTurn;


    #region Privates

    //PRIVATES
    private Camera mainCamera;
    private ThirdPersonCamera tpsCamera;
    private Animator anim;
    private Rigidbody rb;
    private CharacterInput characterInput;

    private bool isSprinting;
    private bool isJumping;
    private bool isGrounded;
    [NonSerialized]
    public bool canIKLook = true;

    #endregion

    #region Getters

    public Camera Camera 
    {
        get { return mainCamera; }
    }

    #endregion

    #region SmoothDamp Variables

    Vector3 input = Vector3.zero;
    float inputMagnitude;
    [NonSerialized] 
    public Vector2 moveInput;

    float smoothXVelocity;
    float x;
    float smoothYVelocity;
    float y;
    float newXVelocity;
    float newX;
    #endregion


    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        mainCamera = Camera.main;

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        characterInput = GetComponent<CharacterInput>();
        tpsCamera = mainCamera.GetComponent<ThirdPersonCamera>();

        stepRayUpper.transform.position = new Vector3(stepRayUpper.transform.position.x, stepHeight, stepRayUpper.transform.position.z);

    }

    private void FixedUpdate()
    {
        //Handle Input
        if(inputMagnitude > 0.1f)
            isSprinting = characterInput.sprint.IsPressed();
        
        MovementInput();
        RotationInput();
        CheckIfGrounded();
        stepClimb();
    }

    private void CheckIfGrounded()
    {
        if (Physics.Raycast(groundCheckObj.position, Vector3.down, out RaycastHit hit, Mathf.Infinity))
        {
            distanceToGround = Vector3.Distance(groundCheckObj.position, hit.point);

            isGrounded = distanceToGround > groundedDistance ? false : true;

            anim.SetFloat("DistanceToGround", distanceToGround);

            if (distanceToGround > distanceTillHardLand)
                anim.SetBool("HardFall", true);

            if (!isGrounded && ControlInAir) 
            {
                if(moveInput.x > 0.1)
                    rb.AddForce(transform.right * jumpAirControlPower, ForceMode.Force);
                if (moveInput.x < -0.1)
                    rb.AddForce(-transform.right * jumpAirControlPower, ForceMode.Force);
                if (moveInput.y > 0.1)
                    rb.AddForce(transform.forward * jumpAirControlPower, ForceMode.Force);
                if (moveInput.y < -0.1)
                    rb.AddForce(-transform.forward * jumpAirControlPower, ForceMode.Force);

            }

        }
    }


    private void MovementInput()
    {
        //getting move input from new input
         moveInput = characterInput.move.ReadValue<Vector2>();
         inputMagnitude = input.magnitude;


        #region Diagnol Input Fix
        float newInputX = moveInput.x;
        float newInputY =  moveInput.y;
        //Stoping Vertical Input from slowing down due to horizontal input
        if ((moveInput.x > 0.4 && moveInput.x < 0.8 || moveInput.x < -0.4 && moveInput.x > -0.8) && moveInput.y > 0.4)
        {
            newInputY = 1;
        }

        //Stoping horizontal Input from slowing down due to vertical input backwards
        if (moveInput.y < -0.4 && moveInput.y > -0.8 && moveInput.x < -0.1)
        {
            newInputY = -1;
            newInputX = -1;

        }
        else if (moveInput.y < -0.4 && moveInput.y > -0.8 && moveInput.x > 0.1)
        {
            newInputY = -1;
            newInputX = 1;

        }

        #endregion

        x = Mathf.SmoothDamp(x, newInputX, ref smoothXVelocity, runSpeedDampTime);
        y = Mathf.SmoothDamp(y, isSprinting ? 1.5f : newInputY, ref smoothYVelocity, runSpeedDampTime);

        input = new Vector3(x, 0, y);


        //Setting anim 
        anim.SetFloat("InputMagnitude", inputMagnitude, runSpeedDampTime, Time.deltaTime);
        anim.SetFloat("Vertical", input.z);

        newX = Mathf.SmoothDamp(newX, input.x, ref newXVelocity, runSpeedDampTime);
        anim.SetFloat("Horizontal", input.x);



    }

    [NonSerialized]
    public bool canRotate;

    public bool turnOnSpotNow = false;
    private void RotationInput()
    {
        if(!RotateInAir)
            if (!canRotate)
                return;

        Vector3 rotationOffset = mainCamera.transform.TransformDirection(input);
        rotationOffset.y = 0;

        if (OnSpotTurn)
        {
            float lookDir = Vector3.SignedAngle(this.transform.forward, mainCamera.transform.forward, Vector3.up);
            if (!turnOnSpotNow) 
            {
                anim.SetFloat("LookDirection", lookDir);
            }
        }

        //if running forward
        if (inputMagnitude > 0.1 && canRotate || tpsCamera.isAiming)
        {
            float yEulerCamera = mainCamera.transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yEulerCamera, 0), Time.deltaTime * rotationSpeed); 

        }
    }


    void stepClimb()
    {
        RaycastHit hitLower;
        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(Vector3.forward), out hitLower, 0.1f))
        {
            RaycastHit hitUpper;
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(Vector3.forward), out hitUpper, 0.2f))
            {
                rb.position -= new Vector3(0f, -stepSmooth * Time.deltaTime, 0f);
            }
        }

        RaycastHit hitLower45;
        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(1.5f, 0, 1), out hitLower45, 0.1f))
        {

            RaycastHit hitUpper45;
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(1.5f, 0, 1), out hitUpper45, 0.2f))
            {
                rb.position -= new Vector3(0f, -stepSmooth * Time.deltaTime, 0f);
            }
        }

        RaycastHit hitLowerMinus45;
        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(-1.5f, 0, 1), out hitLowerMinus45, 0.1f))
        {

            RaycastHit hitUpperMinus45;
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(-1.5f, 0, 1), out hitUpperMinus45, 0.2f))
            {
                rb.position -= new Vector3(0f, -stepSmooth * Time.deltaTime, 0f);
            }
        }
    }

    #region Input Actions
    [NonSerialized]
    public bool canJump;
    public void OnJump(bool value)
    {
        if (!canJump)
            return;

        anim.Play("StartJump");
        rb.velocity += Vector3.up * jumpHeight;

    }

    #endregion
}
