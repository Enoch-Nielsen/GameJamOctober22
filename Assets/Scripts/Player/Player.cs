using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private AudioClip death, possess;

    [Header("Light")]
    [SerializeField] private Transform lightParent;
    [SerializeField] private Light2D light2D;
    [SerializeField] private float lightMaxTimer, lightLerpSpeed;
    private float _currentLightIntensity, _lightChangeTimer;
    
    [Header("Camera")]
    [SerializeField] private Volume postProcessing;
    [SerializeField] private float currentVignette, vignetteLerpStandIn, vignetteLerpTime;
    private Vignette _vignette;
    [SerializeField] private CameraController cameraController;
    
    [Header("Possession")]
    [SerializeField] private GameObject monsterParent;
    public GameObject currentPlayer;
    [SerializeField] private GameObject currentSelectedMonster;
    [SerializeField] private List<GameObject> viableMonsters;
    [SerializeField] private float maxDistance;
    [SerializeField] private float maxDeathTimer;
    [SerializeField] private float currentDeathTimer;
    [SerializeField] private GameObject soulObject;
    
    [Header("Misc")]
    [SerializeField] private TextMeshProUGUI timerTextTemp;

    private void Start()
    {
        monsterParent = GameObject.FindWithTag("MonsterParent");
        currentPlayer.GetComponent<PlayerMove>().enabled = true;
        currentPlayer.GetComponent<PlayerMove>().canMove = true;
        currentPlayer.GetComponent<EnemyAI>().enabled = false;
        currentPlayer.GetComponent<NavMeshAgent>().enabled = false;
        
        currentDeathTimer = maxDeathTimer;
        currentPlayer.tag = "Player";

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
            if (currentSelectedMonster != null && monster == currentSelectedMonster && Vector2.Distance(currentPlayer.transform.localPosition,
                    currentSelectedMonster.transform.localPosition) <= maxDistance)
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
        
        // Update Light.
        if (currentPlayer.GetComponent<MonsterStats>().isTracker)
        {
            if (currentSelectedMonster != null)
            {
                Vector2 direction = ((Vector2)currentSelectedMonster.transform.position - (Vector2)lightParent.transform.position).normalized;
                var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; 
                var offset = 90f;
                lightParent.transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));
                
                FlickerLight();
            }
            
        }
        else
        {
            light2D.intensity = 0;
        }

        // Update camera target.

        cameraController.target = currentPlayer.transform;
        
        // Switch players.

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentSelectedMonster != null)
            {
                PossessMonster();
            }
        }
        
        // Kill the player if they go overtime.
        if (currentDeathTimer <= 0)
        {
            Die();
        }
    }

    private void PossessMonster()
    {
        if (Vector2.Distance(currentPlayer.transform.localPosition,
                currentSelectedMonster.transform.localPosition) <= maxDistance)
        {
            currentSelectedMonster.GetComponent<PlayerMove>().enabled = true;
            currentSelectedMonster.GetComponent<EnemyAI>().enabled = false;
            currentSelectedMonster.GetComponent<NavMeshAgent>().enabled = false;

            currentPlayer.GetComponent<PlayerMove>().enabled = false;
            currentPlayer.GetComponent<EnemyAI>().enabled = false;
            currentPlayer.GetComponent<NavMeshAgent>().enabled = false;
            currentPlayer.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            currentPlayer.transform.tag = "Monster";
            
            // Set Soul Target.
            Soul soul = Instantiate(soulObject, currentPlayer.transform.position, Quaternion.identity).GetComponent<Soul>();
            soul.playerPos = currentSelectedMonster.transform;
            soul.player = this;
            
            // Kill the current monster.
            KillMonster(currentPlayer, false);

            // Set the player to the new monster.
            currentPlayer = currentSelectedMonster;
            currentPlayer.tag = "Player";

            SetStats();

            // Reset Color.
            currentPlayer.GetComponent<SpriteRenderer>().color = Color.white;
            currentSelectedMonster.GetComponent<SpriteRenderer>().color = Color.white;

            currentSelectedMonster = null;

            currentDeathTimer = maxDeathTimer;
            
            // Play Audio.
            audioManager.AddSoundToQueue(possess);
        }
    }

    private void Die()
    {
        KillMonster(currentPlayer, true);
        audioManager.AddSoundToQueue(death);
        
        Debug.Log("Dead");
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
        currentDeathTimer = maxDeathTimer;
        currentVignette = currentPlayer.GetComponent<MonsterStats>().nightVision;
    }

    private void FlickerLight()
    {
        float lightIntensityTemp = light2D.intensity;
        lightIntensityTemp = Mathf.Lerp(lightIntensityTemp, _currentLightIntensity, lightLerpSpeed * Time.deltaTime);
        light2D.intensity = lightIntensityTemp;
        
        if (_lightChangeTimer <= lightMaxTimer)
            _lightChangeTimer += Time.deltaTime;
        else
        {
            _currentLightIntensity = Random.Range(0.5f, 1.1f);
        }

        lightParent.transform.position = currentPlayer.transform.position;
    }

    public void Damage(float damage)
    {
        currentDeathTimer -= damage;
        currentDeathTimer = Mathf.Clamp(currentDeathTimer, 0, maxDeathTimer);
        Debug.Log("OW : " + damage);
    }
}
