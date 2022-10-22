using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Animator anim = null;
    [SerializeField] private NavMeshAgent agent = null;
    [SerializeField] private GameObject[] wayPoints = null;
    [SerializeField] private float speed = 5;
    [SerializeField] private GameObject focalPoint = null;
    [SerializeField] private Rigidbody2D enemyRB = null;

    private GameObject playerInView = null;
    // Start is called before the first frame update
    void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(enemyRB.velocity.y.ToString());
        Vector3 viewRotation = focalPoint.transform.rotation.eulerAngles;
        if(enemyRB.velocity.y !=0 && enemyRB.velocity.x != 0)
        {
            if (enemyRB.velocity.y > 0 && enemyRB.velocity.x > 0)
            {
                // Debug.Log("DoneIThink");
                //-45 degrees
                viewRotation.z = -45.0f;
                focalPoint.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -45));
                
            }
            else if (enemyRB.velocity.y > 0 && enemyRB.velocity.x < 0)
            {
                //Debug.Log("DoneIThink");

                //-135 degrees 
                viewRotation.z = -135.0f;
                focalPoint.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -135));
            }
            else if (enemyRB.velocity.y < 0 && enemyRB.velocity.x > 0)
            {
                //Debug.Log("DoneIThink");

                //45 degrees
                viewRotation.z = 45.0f;
                focalPoint.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 45));
            }
            else if(enemyRB.velocity.y < 0 && enemyRB.velocity.x < 0)
            {
                //Debug.Log("DoneIThink");

                // 135 degrees
                viewRotation.z = 135.0f;
                focalPoint.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 135));
            }
            return;
        }
        else
        {
            if(enemyRB.velocity.y > 0)
            {
                //Debug.Log("LessStuff");

                //0 degrees
                viewRotation.z = 0.0f;
                focalPoint.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
            else if(enemyRB.velocity.y < 0)
            {
                //Debug.Log("LessStuff");

                //180
                viewRotation.z = 180.0f;
                focalPoint.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
            }
            else if(enemyRB.velocity.x > 0)
            {
                //Debug.Log("LessStuff");

                //-90 degrees
                viewRotation.z = -90.0f;
                focalPoint.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
            }
            else if(enemyRB.velocity.x < 0)
            {
                //Debug.Log("LessStuff");

                //90 degrees
                viewRotation.z = 90.0f;
                focalPoint.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
            }
        }

    }

    public NavMeshAgent GetNavMeshAgent()
    {
        return agent;
    }

    public GameObject[] GetWayPoints()
    {
        return wayPoints;
    }

    public GameObject GetCurrentPlayerInView()
    {
        return playerInView;
    }

    public Animator GetAnim()
    {
        return anim;
    }

    public void SeesPlayer(GameObject playerObject)
    {
        anim.SetBool("PlayerInSight", true);
        playerInView = playerObject;
    }

}
