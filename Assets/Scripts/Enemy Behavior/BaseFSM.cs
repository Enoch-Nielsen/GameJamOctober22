using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseFSM : StateMachineBehaviour
{
    public GameObject badGuy;
    public EnemyAI badGuyAI;
    public NavMeshAgent agent;
    public GameObject[] wayPoints;
    public bool playerIsInsight = false;

    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        badGuy = animator.gameObject;
        badGuyAI = badGuy.GetComponent<EnemyAI>();
        agent = badGuyAI.GetNavMeshAgent();
        wayPoints = badGuyAI.GetWayPoints();
    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        badGuyAI.CanSeePlayer();
        if (playerIsInsight)
        {
            
        }
    }

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

}
