
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IK : MonoBehaviour
{
    [Header("Head Look At")]
    [Range(0, 1)]
    [SerializeField] private float weight = 1f;
    private Transform headBone;

    [Header("Body Look At")]
    [Range(0, 20)]
    [SerializeField] private float bodylerpSpeed;
    [SerializeField] float sideLookMultiplier;
    private Transform bodyBone;
    Transform bodyLookAt;


    [Header("IK Functions")]
    [SerializeField] private bool HeadLookAt;
    [SerializeField] private bool BodyLookAt;
    [SerializeField] public bool FeetIK;


    private Animator anim;
    private Camera mainCamera;
    private CharacterMotor playerMotor;
    private ThirdPersonCamera tpsCamera;


    Vector3 lookAtPosition;
    Vector3 bodyLookAtPosition;
    private void Start()
    {
        InitializeIK();
    }

    private void FixedUpdate()
    {
        if (BodyLookAt && playerMotor.canIKLook)
        {

            Ray lookAt = new Ray(transform.position, transform.forward);
            bodyLookAtPosition = lookAt.GetPoint(10f);


            if (playerMotor.moveInput.x > 0.1)
            {
                bodyLookAt.transform.position = Vector3.Slerp(bodyLookAt.transform.position, bodyLookAtPosition + (transform.right * sideLookMultiplier), Time.deltaTime * bodylerpSpeed);

            }
            else if (playerMotor.moveInput.x < -0.1)
            {
                bodyLookAt.transform.position = Vector3.Slerp(bodyLookAt.transform.position, bodyLookAtPosition - (transform.right * sideLookMultiplier), Time.deltaTime * bodylerpSpeed);
            }
            else
            {
                bodyLookAt.transform.position = Vector3.Slerp(bodyLookAt.transform.position, bodyLookAtPosition, Time.deltaTime * bodylerpSpeed);

            }


        }

        
    }

    private void LateUpdate()
    {

        if (BodyLookAt)
        {
            if (playerMotor.moveInput.magnitude < 1.1 )
                    bodyBone.LookAt(bodyLookAt.transform.position);
        }


    }

    public GameObject headTarget;
    private void OnAnimatorIK(int layerIndex)
    {
        #region Head Look At
        if (HeadLookAt && playerMotor.canIKLook) 
        {
            anim.SetLookAtWeight(1, 0f, 1.0f ,1f, 0.5f);

            Ray lookAt = new Ray(transform.position, mainCamera.transform.forward);
            lookAtPosition = lookAt.GetPoint(10f);

            anim.SetLookAtPosition(headTarget.transform.position);
        }
        #endregion

        
    }

    void InitializeIK()
    {
        anim = GetComponent<Animator>();
        playerMotor = GetComponent<CharacterMotor>();
        mainCamera = playerMotor.Camera;
        tpsCamera = mainCamera.GetComponentInChildren<ThirdPersonCamera>();

        //Bones
        if (anim)
        {
            headBone = anim.GetBoneTransform(HumanBodyBones.Head);
            bodyBone = anim.GetBoneTransform(HumanBodyBones.Spine);

        }

        Ray lookAt = new Ray(transform.position, transform.forward);
        lookAtPosition = lookAt.GetPoint(5f);

        //Creating Body Look at run Time
        bodyLookAt = new GameObject().transform;
        bodyLookAt.gameObject.name = "Body Look At";
        bodyLookAt.position = lookAtPosition;

    }

    
}
