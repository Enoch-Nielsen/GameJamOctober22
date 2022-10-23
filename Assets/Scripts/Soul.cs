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
        
        Vector2 direction = ((Vector2)playerPos.position - (Vector2)transform.position).normalized;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; 
        var offset = -90f;
        transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));
        
        if (Vector2.Distance(playerPos.position, transform.position) <= playerMaxDist)
        {
            player.currentPlayer.GetComponent<PlayerMove>().canMove = true;
            Destroy(gameObject, 0.2f);
        }
    }
}
