using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private CameraController cameraController;
    
    [SerializeField] private GameObject monsterParent;
    [SerializeField] private GameObject currentPlayer;
    [SerializeField] private GameObject currentSelectedMonster;

    [SerializeField] private List<GameObject> viableMonsters;

    [SerializeField] private float maxDistance;

    private void Start()
    {
        monsterParent = GameObject.FindWithTag("MonsterParent");
        currentPlayer.GetComponent<PlayerMove>().enabled = true;
    }

    private void Update()
    {
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
                currentPlayer = currentSelectedMonster;
                
                currentPlayer.GetComponent<SpriteRenderer>().color = Color.white;
                currentSelectedMonster.GetComponent<SpriteRenderer>().color = Color.white;


                currentSelectedMonster = null;
            }
        }
    }
}
