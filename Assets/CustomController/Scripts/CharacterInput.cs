/// <summary>
/// Third Person Controller
/// <copyright>(c) AlexRak2TheDev or Alejandro Hernandez 2013</copyright>
/// 
/// Third Person Controller homepage: https://github.com/AlexRak2/PlayerControllerTemplates
/// 
/// This software is provided 'as-is', without any express or implied
/// warranty.  In no event will the authors be held liable for any damages
/// arising from the use of this software.
///
/// Permission is NOT granted to anyone to use this software for any purpose,
/// and to alter it and redistribute it freely.
///
/// 1. The origin of this software must not be misrepresented; you must not
/// claim that you wrote the original software. If you use this software
/// in a product, an acknowledgment in the product documentation would be
/// appreciated but is not required.
/// 2. Altered source versions must be plainly marked as such, and must not be
/// misrepresented as being the original software.
/// 3. This notice may not be removed or altered from any source distribution.
/// </summary>
/// 
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
