using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform playerPos;
    public Player player;
    public Vector3 direction;
    [SerializeField] private float playerMaxDist;
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    [SerializeField] private float bulletLifetime;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, bulletLifetime);
    }

    private void Update()
    {
        if (playerPos == null)
            Destroy(gameObject);
        
        transform.position += (direction * (Time.deltaTime * speed));

        if (Vector2.Distance(playerPos.position, transform.position) <= playerMaxDist)
        {
            player.Damage(damage);
            Destroy(gameObject);
        }
    }
}
