using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed;
    [SerializeField] private Vector2 move;
    public bool canMove;
    
    void Update()
    {
        if (!canMove)
            return;        
        
        float xIn = Input.GetAxisRaw("Horizontal");
        float yIn = Input.GetAxisRaw("Vertical");

        move = new Vector2(xIn, yIn).normalized;
    }

    private void FixedUpdate()
    {
        if (!canMove)
            return;
        
        rb.velocity = new Vector2(move.x * speed, move.y * speed);
    }
}
