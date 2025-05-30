﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlaySoundBehaviour : StateMachineBehaviour
{
    private AudioSource audioSource;
    public AudioClip audioSound;
    public bool loop = false;
    

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        audioSource = animator.transform.GetComponent<AudioSource>();
        audioSource.clip = audioSound;
        audioSource.loop = loop;
        if (animator.GetBool("IsAttacking"))
        {
            audioSource.pitch = UnityEngine.Random.Range(1f, 2f);
            audioSource.Play();
        }
        else
        {
            audioSource.pitch = 1f;
            audioSource.Play();
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
        audioSource.Stop();
        audioSource.loop = false;
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
