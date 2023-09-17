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

public class CanBehaviour : StateMachineBehaviour
{
    public bool canJump;
    public bool canRotate;
    public bool canLookIK;
    public bool canTurnOnSpot;
    public bool hasRootmotion;

    [Header("Jump")]
    public bool isJump;
    public bool isLand;
    private CharacterMotor character;

    bool isFeetIKOn;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        character = animator.gameObject.GetComponent<CharacterMotor>();
        character.canJump = this.canJump;
        character.canRotate = this.canRotate;
        character.canIKLook = this.canLookIK;
        character.turnOnSpotNow = canTurnOnSpot;

        if(hasRootmotion)
                        animator.applyRootMotion = true;

        if (isJump) 
        {
            animator.applyRootMotion = false;
            animator.SetLayerWeight(1, 0);

            if (animator.GetComponent<IK>().FeetIK) 
            {
                isFeetIKOn = true;
                animator.GetComponent<IK>().FeetIK = false;

            }

        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!canLookIK)
            animator.gameObject.GetComponent<CharacterMotor>().canIKLook = true;

        if (canTurnOnSpot)
            character.turnOnSpotNow = false;

        if (isLand)
        {
            if (isFeetIKOn)
            {
                animator.GetComponent<IK>().FeetIK = true;
                isFeetIKOn = false;

            }
            animator.applyRootMotion = true;
            animator.SetBool("HardFall", false);

        }

    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
