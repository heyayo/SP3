using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Boss Item")]
public class BossItem : Item
{
    [Header("Spawns this boss")]
    [SerializeField]
    private GameObject bossPrefab;

    private Transform playerTransform; // Reference to the player's transform

    private void OnEnable()
    {
        // Find and assign the player's transform using the tag "Player"
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
    }

    public override void Use()
    {
        // Spawn Randomly around player
        float angle = Random.Range(0f, 360f);
        float x = 20f * Mathf.Cos(angle);
        float y = 20f * Mathf.Sin(angle);
        Vector2 spawnPos = new Vector2(x, y);

        Instantiate(bossPrefab, new Vector2(playerTransform.position.x, playerTransform.position.y)
            + spawnPos, Quaternion.identity);

        consumed.Invoke();
    }
}