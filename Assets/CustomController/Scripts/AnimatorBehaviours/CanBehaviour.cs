
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
