using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    public float speed;
    [SerializeField] private Vector2 move;
    [SerializeField] private MonsterAnimator monsterAnimator;
    public AudioClip walkClip, possessClip;
    public AudioManager audioManager;
    public float walkVolume, possessVolume;
    [SerializeField] private float walkTimer, walkTimerMax;
    private bool played;

    public bool canMove;

    private void Start()
    {
        if (walkClip != null)
        {
            walkTimerMax = walkClip.length + 0.05f;
        }

        if (possessClip != null)
        {
            Invoke(nameof(PlayPossessAudio), 1.0f);
        }
    }

    private void Update()
    {
        if (!canMove)
            return;
        
        float xIn = Input.GetAxisRaw("Horizontal");
        float yIn = Input.GetAxisRaw("Vertical");
        
        move = new Vector2(xIn, yIn);

        monsterAnimator.velocity = move;
        
        rb.velocity = new Vector2(move.x * speed, move.y * speed);
        
        if (GetComponent<Monster>().monsterType != Monster.MonsterType.Wraith)
        {
            if (walkTimer <= walkTimerMax)
            {
                walkTimer += Time.deltaTime;
            }
            else
            {
                if (walkClip != null && (xIn != 0 || yIn != 0))
                {
                    audioManager.AddSoundToQueue(walkClip, false, walkVolume);
                    walkTimer = 0.0f;
                }
            }
        }
        else if(!played)  
        {
            audioManager.AddSoundToQueue(walkClip, true, walkVolume);
            played = true;
        }
    }

    private void PlayPossessAudio()
    {
        audioManager.AddSoundToQueue(possessClip, false, possessVolume);
    }
}
