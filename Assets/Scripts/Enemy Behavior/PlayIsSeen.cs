using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayIsSeen : MonoBehaviour
{
    [SerializeField] private EnemyAI parentObject = null;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            parentObject.SeesPlayer();
        }
    }
}
