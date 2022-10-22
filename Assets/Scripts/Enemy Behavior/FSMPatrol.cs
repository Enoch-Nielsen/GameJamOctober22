using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMPatrol : BaseFSM
{
    private int wayPointNumber = 0;
    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        wayPointNumber = Random.Range(0, 3);
        agent.SetDestination(wayPoints[wayPointNumber].transform.position);
    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(agent.remainingDistance != 0)
        {
            return;
        }

        agent.SetDestination(wayPoints[wayPointNumber].transform.position);
    }

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

}
