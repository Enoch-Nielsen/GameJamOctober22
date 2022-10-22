using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class AntiVillain : MonoBehaviour
{
    [SerializeField] private float shootTimerMax, shootTimer;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float accuracyRandom;
    [SerializeField] private float maxDistance;
    
    [SerializeField] private Player player;
    [SerializeField] private Transform playerTransform;


    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    private void Update()
    {
        if (player.currentPlayer == null)
            return;
        
        playerTransform = player.currentPlayer.transform;
        
        if (shootTimer <= shootTimerMax)
        {
            shootTimer += Time.deltaTime;
        }
        else
        {
            if (Vector2.Distance(transform.position, playerTransform.position) <= maxDistance)
            {
                Shoot();
            }
        }
    }

    private void Shoot()
    {
        float rX = Random.Range(-accuracyRandom, accuracyRandom);
        float rY = Random.Range(-accuracyRandom, accuracyRandom);

        Vector2 randomAcc = new Vector2(rX, rY);

        Vector2 direction = ((Vector2)playerTransform.position - ((Vector2)transform.position + randomAcc));

        Bullet b = Instantiate(bullet, transform.position, Quaternion.Euler(Vector3.zero)).GetComponent<Bullet>();
        b.playerPos = playerTransform;
        b.player = player;
        b.direction = direction;
        
        shootTimer = 0;
        
        Debug.Log("Bang");
    }
}
