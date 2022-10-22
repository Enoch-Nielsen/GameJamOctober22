using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMRunAway : BaseFSM
{
    private float runDistance = 5.0f;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        Debug.Log("ScaryGuy");

        //agent.SetDestination(playerInView.transform.position);
        //agent.speed *= -1;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerInView = badGuyAI.GetCurrentPlayerInView();

        float distance = Vector3.Distance(badGuy.transform.position, playerInView.transform.position);

        if(distance > runDistance)
        {
            Animator anim = badGuyAI.GetAnim();
            anim.SetBool("PlayerInSight", false);
            return;
        }

        Vector3 dirToPlayer = badGuy.transform.position - playerInView.transform.position;
        Vector3 newPos = badGuy.transform.position + dirToPlayer;
        agent.SetDestination(newPos);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

}
