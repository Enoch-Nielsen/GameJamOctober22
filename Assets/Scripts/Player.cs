using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Player : MonoBehaviour
{
    [SerializeField] private Volume postProcessing;
    [SerializeField] private TextMeshProUGUI timerTextTemp;
    [SerializeField] private CameraController cameraController;
    
    [SerializeField] private GameObject monsterParent;
    [SerializeField] private GameObject currentPlayer;
    [SerializeField] private GameObject currentSelectedMonster;

    [SerializeField] private List<GameObject> viableMonsters;

    [SerializeField] private float maxDistance;

    [SerializeField] private float maxDeathTimer;
    [SerializeField] private float currentDeathTimer;

    [SerializeField] private float currentVignette, vignetteLerpStandIn, vignetteLerpTime;
    private Vignette _vignette;

    private void Start()
    {
        monsterParent = GameObject.FindWithTag("MonsterParent");
        currentPlayer.GetComponent<PlayerMove>().enabled = true;
        currentDeathTimer = maxDeathTimer;

        if (postProcessing.profile.TryGet<Vignette>(out var tmpVignette))
        {
            _vignette = tmpVignette;
        }
        
        SetStats();
    }

    private void Update()
    {
        if (currentPlayer == null)
            return;

        // Fill monster list.
        foreach (Transform monster in monsterParent.transform)
        {
            if (viableMonsters.Contains(monster.gameObject))
                continue;
            
            if(monster.CompareTag("Monster"))
                viableMonsters.Add(monster.gameObject);
        }

        // Remove non viable monsters.
        
        foreach (var monster in viableMonsters.ToArray())
        {
            if (Vector2.Distance(currentPlayer.transform.localPosition, monster.transform.localPosition) >= maxDistance)
            {
                viableMonsters.Remove(monster);
            }

            if (monster == currentPlayer)
            {
                viableMonsters.Remove(monster);
            }
        }
        
        // Check distances and find the closest monster.

        List<float> distances = new List<float>();
        foreach (var monster in viableMonsters)
        {
            distances.Add(Vector2.Distance(currentPlayer.transform.localPosition, monster.transform.localPosition));
        }

        int shortIndex = 0;
        for (int i = 0; i < viableMonsters.Count; i++)
        {
            if (viableMonsters.Count == 0)
                break;

            if (distances[i] < distances[shortIndex])
                shortIndex = i;
        }

        if (viableMonsters.Count == 0)
        {
            currentSelectedMonster = null;
        }
        else
        {
            currentSelectedMonster = viableMonsters[shortIndex];
        }

        // Highlight Selected Monster.
        
        foreach (var monster in viableMonsters)
        {
            if (monster == currentSelectedMonster)
            {
                monster.GetComponent<SpriteRenderer>().color = Color.yellow;
            }
            else
            {
                monster.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
        
        // Update Death Timer.
        if (currentDeathTimer >= 0)
        {
            currentDeathTimer -= Time.deltaTime;
        }
        
        // Death Timer Text.
        timerTextTemp.text = "" + Mathf.RoundToInt(currentDeathTimer);
        
        // Update Vignette.
        vignetteLerpStandIn = Mathf.Lerp(vignetteLerpStandIn, currentVignette, vignetteLerpTime * Time.deltaTime);
        _vignette.intensity.value = vignetteLerpStandIn;

        // Update camera target.

        cameraController.target = currentPlayer.transform;
        
        // Switch players.

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentSelectedMonster != null)
            {
                currentSelectedMonster.GetComponent<PlayerMove>().enabled = true;
                
                currentPlayer.GetComponent<PlayerMove>().enabled = false;
                currentPlayer.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                
                // Kill the current monster.
                KillMonster(currentPlayer, false);
                
                // Set the player to the new monster.
                currentPlayer = currentSelectedMonster;
                
                SetStats();
                
                // Reset Color.
                currentPlayer.GetComponent<SpriteRenderer>().color = Color.white;
                currentSelectedMonster.GetComponent<SpriteRenderer>().color = Color.white;
                

                
                currentSelectedMonster = null;

                currentDeathTimer = maxDeathTimer;
            }
        }
        
        // Kill the player if they go overtime.
        if (currentDeathTimer <= 0)
        {
            KillMonster(currentPlayer, true);
            Debug.Log("Dead");
        }
    }

    private void KillMonster(GameObject monster, bool player)
    {
        // This is just a placeholder.

        if (player)
            currentPlayer = null;

        viableMonsters.Remove(monster);
        Destroy(monster); 
    }

    private void SetStats()
    {
        // Get Stats.
        maxDeathTimer = currentPlayer.GetComponent<MonsterStats>().monsterDeathTime;
        currentVignette = currentPlayer.GetComponent<MonsterStats>().nightVision;
    }
}
