using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimator : MonoBehaviour
{
    public Animator animator;
    public Vector2 velocity;

    private void Update()
    {
        Vector2 vel = animVectorRound();
        
        animator.SetFloat("Horizontal", vel.x);
        animator.SetFloat("Vertical", vel.y);
    }

    private Vector2 animVectorRound()
    {
        if (velocity.x != 0 || velocity.y != 0)
        {
            if (Mathf.Abs(velocity.x) >= Mathf.Abs(velocity.y))
                return new Vector2(velocity.x, 0);
            else
                return new Vector2(0, velocity.y);
        }
        
        return new Vector2(velocity.x, velocity.y);
    }
}
