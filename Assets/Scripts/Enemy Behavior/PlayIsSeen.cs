using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayIsSeen : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("I Exsist");
    }
    [SerializeField] private EnemyAI parentObject = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Touch");
        if(collision.CompareTag("Player"))
        {
            parentObject.SeesPlayer(collision.gameObject);
        }
    }
}
