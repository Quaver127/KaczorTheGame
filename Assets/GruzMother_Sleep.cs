using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruzMother_Sleep : StateMachineBehaviour
{
    
    [SerializeField] GruzMother gruzMother;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        gruzMother = GameObject.FindGameObjectWithTag("GruzMother").GetComponent<GruzMother>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        gruzMother.SleepState();
    }
}
