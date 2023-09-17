using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Camera Look Stats")]
    [SerializeField] private float lookXSens = 1f;
    [SerializeField] private float lookYSens = 1f;
    [SerializeField] private bool isXInverted;
    [SerializeField] private bool isYInverted;
    private Vector2 lookInput;
    [Header("Camera Lerp")]
    [Range(0f, 100f)]
    public float cameraLerpSpeed;
    public Transform playerLookAt;
    GameObject cameraFollowLookAt;

    [Header("Aim Stats")]
    [SerializeField] private Vector3 aimLeftOffset;
    [SerializeField] private Vector3 aimRightOffset;
    [Range(0f, 100f)]
    [SerializeField] private float aimSpeed = 20f;
    [SerializeField] public bool isAiming = false;
    bool isAimCamLerping = false;


    [Header("Shoulder Swap Stats")]
    [SerializeField] private Vector3 leftShoulderOffset;
    [SerializeField] private Vector3 rightShoulderOffset;
    [Range(0f, 100f)]
    [SerializeField] private float swapSpeed = 20f;
    [SerializeField] bool cameraLeftSided;

    [Header("Camera Functions")]
    [SerializeField] private bool ShoulderSwaping;
    [SerializeField] private bool Aim;


    [Space(10)]
    [SerializeField] private CharacterMotor characterMotor;
    [SerializeField] private CharacterInput characterInput;

    private CinemachineFreeLook freeLookCamera;
    private CinemachineCameraOffset cameraOffset;

    #region Getters

    public Vector3 LookInput { get { return lookInput; } }
    #endregion

    private void Awake()
    {
        InitializeCamera();
    }

    private void LateUpdate()
    {
        AimLogic();
        UpdateCameraRotation();

        cameraOffset.m_Offset = Vector3.Lerp(cameraOffset.m_Offset, cameraLeftSided ? rightShoulderOffset : leftShoulderOffset, Time.deltaTime * swapSpeed);
/*
        //Camera Follow Object Lerping to player
        cameraFollowLookAt.transform.position = Vector3.Lerp(cameraFollowLookAt.transform.position, playerLookAt.forward * 3, Time.deltaTime * cameraLerpSpeed);*/


    }

    void InitializeCamera()
    {
        freeLookCamera = GetComponentInChildren<CinemachineFreeLook>();
        cameraOffset = freeLookCamera.GetComponent<CinemachineCameraOffset>();

/*        cameraFollowLookAt = new GameObject();
        cameraFollowLookAt.name = "Camera Follow Object";
        cameraFollowLookAt.transform.position = playerLookAt.position;


        freeLookCamera.LookAt = cameraFollowLookAt.transform;*/
    }

    private void AimLogic()
    {
        if (isAimCamLerping)
        {
            if (isAiming)
            {
                cameraOffset.m_Offset = Vector3.Lerp(cameraOffset.m_Offset, (cameraLeftSided ? aimRightOffset : aimLeftOffset), Time.deltaTime * aimSpeed);
            }
            else
            {
                isAimCamLerping = false;
            }
        }
    }

    void UpdateCameraRotation()
    {
        Vector2 lookInput = characterInput.look.ReadValue<Vector2>();
        lookInput = new Vector2(lookInput.x, lookInput.y);

        lookInput.y = isYInverted ? -lookInput.y : lookInput.y;
        lookInput.x = isXInverted ? -lookInput.x : lookInput.x * 180f;

        freeLookCamera.m_YAxis.Value += lookInput.y * lookYSens * Time.deltaTime;
        freeLookCamera.m_XAxis.Value += lookInput.x * lookXSens * Time.deltaTime;
    }

    #region Input Actions

    public void OnShoulderSwap()
    {
        if (!ShoulderSwaping)
            return;

        cameraLeftSided = !cameraLeftSided;
    }

    public void OnAim(bool value)
    {
        if (!Aim)
            return;

        isAimCamLerping = true;
        isAiming = value;
    }
    #endregion
}
