
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterInput : MonoBehaviour
{
    private InputMaster controls;
    public InputAction move;
    public InputAction look;
    public InputAction sprint;
    public InputAction jump;
    public InputAction shoulderSwap;
    public InputAction Aim;

    private CharacterMotor characterMotor;
    private ThirdPersonCamera tpsCamera;

    private void OnEnable()
    {
        controls = new InputMaster();

        move = controls.PlayerMechanics.Movement;
        move.Enable();

        look = controls.PlayerMechanics.Look;
        look.Enable();

        sprint = controls.PlayerMechanics.Sprint;
        sprint.Enable();

        jump = controls.PlayerMechanics.Jump;
        jump.Enable();
        jump.started += JumpInput;

        shoulderSwap = controls.PlayerMechanics.ShoulderSwap;
        shoulderSwap.Enable();
        shoulderSwap.started += ShoulderSwapInput;

        Aim = controls.PlayerMechanics.Aim;
        Aim.Enable();
        Aim.performed += AimInput;
        Aim.canceled += AimInput;
    }
    private void Start()
    {
        characterMotor = GetComponent<CharacterMotor>();
        tpsCamera = characterMotor.Camera.GetComponent<ThirdPersonCamera>();
    }

    #region Inputs
    private void JumpInput(InputAction.CallbackContext context)
    {
        characterMotor.OnJump(context.ReadValueAsButton());
    }

    void ShoulderSwapInput(InputAction.CallbackContext context)
    {
        tpsCamera.OnShoulderSwap();
    }

    public void AimInput(InputAction.CallbackContext context)
    {
        tpsCamera.OnAim(context.ReadValueAsButton());
    }
    #endregion
}
