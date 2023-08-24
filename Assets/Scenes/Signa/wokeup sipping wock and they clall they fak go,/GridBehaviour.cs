using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBehaviour : MonoBehaviour
{
   
    public float offsetXRange = 2.0f; // Range for random X offset
    public float offsetYRange = 2.0f; // Range for random Y offset
    public float colorChangeDelay = 0.5f; // Delay before changing color
    public float destroyDelay = 1.5f; // Delay before destroying the object

    void Start()
    {
        // Find the player object using the specified tag
        GameObject player = PlayerManager.Instance.gameObject;

        if (player != null)
        {
            // Get random offsets for X and Y
            float randomOffsetX = Random.Range(-offsetXRange, offsetXRange);
            float randomOffsetY = Random.Range(-offsetYRange, offsetYRange);

            // Set the position of the spawned object with random offsets applied to player's position
            transform.position = player.transform.position + new Vector3(randomOffsetX, randomOffsetY, 0);

            // Disable colliders for all child objects
            ToggleColliders(false);

            // Schedule the color change, collider enable, and destruction after the specified delays
            Invoke("ChangeColorAndEnableColliders", colorChangeDelay);
            Invoke("DestroySelf", destroyDelay);
        }
        else
        {
           
        }
    }

    void ChangeColorAndEnableColliders()
    {
        // Change sprite colors to red
        ChangeColorToRed();

        // Enable colliders for all child objects
        ToggleColliders(true);
    }

    void ChangeColorToRed()
    {
        // Iterate through all child objects
        foreach (Transform child in transform)
        {
            // Check if the child has a SpriteRenderer component
            SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();

            if (spriteRenderer != null)
            {
                // Change the sprite's color to red
                spriteRenderer.color = new Color(0.6f, 0.0f, 0.8f); // Purple color
            }
        }
    }

    void ToggleColliders(bool enable)
    {
        // Iterate through all child objects
        foreach (Transform child in transform)
        {
            // Get the collider component
            Collider2D collider = child.GetComponent<Collider2D>();

            if (collider != null)
            {
                // Enable or disable the collider based on the 'enable' parameter
                collider.enabled = enable;
            }
        }
    }

    void DestroySelf()
    {
        // Destroy the object
        Destroy(gameObject);
    }
}