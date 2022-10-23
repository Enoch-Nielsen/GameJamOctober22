using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public enum MonsterType {Cyclops, Spider, Wraith, Wolf};
    public MonsterType monsterType;
    public GameObject stats;

    public MonsterStats GetStats()
    {
        return stats.GetComponent<MonsterStats>();
    }

    private void Update()
    {
        if (transform.CompareTag("Monster"))
        {
            GetComponent<Rigidbody2D>().isKinematic = true;
        }
        else
        {
            GetComponent<Rigidbody2D>().isKinematic = false;
        }
    }
}
