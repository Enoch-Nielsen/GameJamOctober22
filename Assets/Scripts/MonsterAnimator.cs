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
        animator.SetFloat("Horizontal", velocity.x);
        animator.SetFloat("Vertical", velocity.y);
    }
}
