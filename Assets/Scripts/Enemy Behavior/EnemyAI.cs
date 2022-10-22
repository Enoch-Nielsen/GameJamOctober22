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
    [SerializeField] private Transform initialTransform;

    private GameObject playerInView = null;
    [SerializeField] Vector3 oldPosition;
    [SerializeField] Vector3 newPosition;
    // Start is called before the first frame update
    void Start()
    {
        oldPosition = initialTransform.position;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        StartCoroutine(PositionLag());
    }

    // Update is called once per frame
    void Update()
    {
        newPosition = this.transform.position;

        float velocityY = newPosition.y - Mathf.Abs(oldPosition.y);
        float velocityX = newPosition.x - Mathf.Abs(oldPosition.x);
        //Debug.Log(enemyRB.velocity.y.ToString());
        Vector3 viewRotation = focalPoint.transform.rotation.eulerAngles;
        if((velocityY > 0.00001 || velocityY < -0.1) && (velocityX > 0.00001 || velocityX < -0.00001))
        {
            if (velocityY > 0 && velocityX > 0)
            {
                // Debug.Log("DoneIThink");
                //-45 degrees
                viewRotation.z = 45.0f;
                focalPoint.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -45));
                
            }
            if (velocityY > 0 && velocityX < 0)
            {
                //Debug.Log("DoneIThink");

                //-135 degrees 
                viewRotation.z = 45.0f;
                focalPoint.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 45));
            }
            if (velocityY < 0 && velocityX > 0)
            {
                //Debug.Log("DoneIThink");

                //45 degrees
                viewRotation.z = 135.0f;
                focalPoint.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -135));
            }
            if(velocityY < 0 && velocityX < 0)
            {
                //Debug.Log("DoneIThink");

                // 135 degrees
                viewRotation.z = -135.0f;
                focalPoint.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 135));
            }
            //oldPosition = newPosition;
            return;
        }
        #region secondHalf
        else
        {
            /*if(velocityY > 0)
            {
                //Debug.Log("LessStuff");

                //0 degrees
                viewRotation.z = 0.0f;
                focalPoint.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
            else if(velocityY < 0)
            {
                //Debug.Log("LessStuff");

                //180
                viewRotation.z = 180.0f;
                focalPoint.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
            }
            else if(velocityX > 0)
            {
                //Debug.Log("LessStuff");

                //-90 degrees
                viewRotation.z = -90.0f;
                focalPoint.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
            }
            else if(velocityX < 0)
            {
                //Debug.Log("LessStuff");

                //90 degrees
                viewRotation.z = 90.0f;
                focalPoint.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
            }*/
        }
        #endregion
        //oldPosition = newPosition;
    }

    private IEnumerator PositionLag()
    {
        yield return new WaitForSeconds(0.2f);
        oldPosition = newPosition;
        StartCoroutine(PositionLag());
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
