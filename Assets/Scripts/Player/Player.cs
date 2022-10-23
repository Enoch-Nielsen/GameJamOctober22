using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private AudioClip death, possess, music1, music2, wind, winAudio;
    [SerializeField] private float musicVolume, possessVolume, windVolume;
    [SerializeField] private float musicTimer, musicTimerMax;

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
    [SerializeField] private MonsterStats currentStats;
    [SerializeField] private Monster monster;
    
    [Header("Misc")]
    [SerializeField] private Image imageTimer;
    [SerializeField] private GameObject deathObject;
    [SerializeField] private Image winImage;

    private void Start()
    {
        audioManager.AddSoundToQueue(music1, false, musicVolume);
        audioManager.AddSoundToQueue(wind, true, windVolume);

        musicTimerMax = music1.length;
        
        monsterParent = GameObject.FindWithTag("MonsterParent");
        currentPlayer.GetComponent<PlayerMove>().enabled = true;
        currentPlayer.GetComponent<PlayerMove>().canMove = true;

        currentPlayer.TryGetComponent<EnemyAI>(out var ai);
        
        if (ai != null)
        {
            ai.enabled = false;
        }

        currentPlayer.TryGetComponent<NavMeshAgent>(out var agent);
        
        if (agent != null)
        {
            agent.enabled = false;
        }
        
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

        if (musicTimer <= musicTimerMax + 5.0f)
        {
            musicTimer += Time.deltaTime;
        }
        else
        {
            audioManager.AddSoundToQueue(music2, true, musicVolume);
        }

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
            distances.Add(Vector2.Distance(currentPlayer.transform.position, monster.transform.position));
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

        // Update Death Timer.
        if (currentDeathTimer >= 0)
        {
            currentDeathTimer -= Time.deltaTime;
        }
        
        // Death Timer Text.
        imageTimer.fillAmount = (currentDeathTimer / maxDeathTimer);
        
        // Update Vignette.
        vignetteLerpStandIn = Mathf.Lerp(vignetteLerpStandIn, currentVignette, vignetteLerpTime * Time.deltaTime);
        _vignette.intensity.value = vignetteLerpStandIn;
        
        // Update Light.
        if (currentStats.isTracker)
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
                Debug.Log("Possess");
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
        if (Vector2.Distance(currentPlayer.transform.position,
                currentSelectedMonster.transform.position) <= maxDistance)
        {
            Instantiate(deathObject, currentPlayer.transform.position, Quaternion.identity);

            if (currentPlayer.GetComponent<Monster>().monsterType == Monster.MonsterType.Wraith)
            {
                Destroy(audioManager.GetSound(currentPlayer.GetComponent<PlayerMove>().walkClip).gameObject);
            }

            currentSelectedMonster.GetComponent<PlayerMove>().enabled = true;
            currentSelectedMonster.GetComponent<PlayerMove>().audioManager = audioManager;

            currentSelectedMonster.TryGetComponent<EnemyAI>(out var ai);
        
            if (ai != null)
            {
                ai.enabled = false;
            }

            currentSelectedMonster.TryGetComponent<NavMeshAgent>(out var agent);
        
            if (agent != null)
            {
                agent.enabled = false;
            }

            currentPlayer.GetComponent<PlayerMove>().enabled = false;
            
            currentPlayer.TryGetComponent<EnemyAI>(out var ai2);
        
            if (ai2 != null)
            {
                ai2.enabled = false;
            }

            currentPlayer.TryGetComponent<NavMeshAgent>(out var agent2);
        
            if (agent2 != null)
            {
                agent2.enabled = false;
            }
            
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
            
            currentSelectedMonster = null;

            currentDeathTimer = maxDeathTimer;

            // Play Audio.
            audioManager.AddSoundToQueue(possess, false, possessVolume);
            
            if (currentPlayer.GetComponent<Monster>().monsterType == Monster.MonsterType.Tree)
            {
                Invoke(nameof(JumpScare), 2.0f);
                audioManager.StopAllSounds();
                audioManager.AddSoundToQueue(winAudio, false, possessVolume);
            }
        }
    }

    private void Die()
    {
        Instantiate(deathObject, currentPlayer.transform.position, Quaternion.identity);
        KillMonster(currentPlayer, true);
        audioManager.AddSoundToQueue(death, false, 1.0f);
        Invoke(nameof(Reset), 5);
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
        monster = currentPlayer.GetComponent<Monster>();

        currentStats = monster.GetStats();

        maxDeathTimer = currentStats.monsterDeathTime;
        currentDeathTimer = maxDeathTimer;
        currentVignette = currentStats.nightVision;
        currentPlayer.GetComponent<PlayerMove>().speed = currentStats.speed;
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
            _currentLightIntensity = Random.Range(10f, 20f);
        }

        lightParent.transform.position = currentPlayer.transform.position;
    }

    public void Damage(float damage)
    {
        currentDeathTimer -= damage;
        currentDeathTimer = Mathf.Clamp(currentDeathTimer, 0, maxDeathTimer);
        Debug.Log("OW : " + damage);
    }

    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void JumpScare()
    {
        winImage.gameObject.SetActive(true);
    }
}
