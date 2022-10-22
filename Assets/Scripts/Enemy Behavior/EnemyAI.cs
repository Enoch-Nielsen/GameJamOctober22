using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent = null;
    [SerializeField] private GameObject[] wayPoints = null;
    [SerializeField] private float speed = 5;
    public bool canSeePlayer = false;
    // Start is called before the first frame update
    void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public NavMeshAgent GetNavMeshAgent()
    {
        return agent;
    }

    public GameObject[] GetWayPoints()
    {
        return wayPoints;
    }

    public void SeesPlayer()
    {
        canSeePlayer = true;
    }

    public bool CanSeePlayer()
    {
        return canSeePlayer;
    }
}
