using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent = null;
    [SerializeField] private GameObject[] wayPoints = null;
    // Start is called before the first frame update
    void Start()
    {
        
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
}
