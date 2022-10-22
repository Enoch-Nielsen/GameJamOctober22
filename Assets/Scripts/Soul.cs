using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soul : MonoBehaviour
{
    public Transform playerPos;
    public Player player;
    [SerializeField] private float playerMaxDist;
    [SerializeField] private float speed;

    private void Start()
    {
        Destroy(gameObject, 10.0f);
    }

    private void Update()
    {
        transform.position = Vector2.Lerp(transform.position, playerPos.position, speed * Time.deltaTime);
        
        if (Vector2.Distance(playerPos.position, transform.position) <= playerMaxDist)
        {
            player.currentPlayer.GetComponent<PlayerMove>().canMove = true;
            Destroy(gameObject, 0.2f);
        }
    }
}
