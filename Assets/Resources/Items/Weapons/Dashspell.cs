using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Items/Dash")]
public class Dashspell : Item
{
    private Rigidbody2D rb;
    private float dashForce = 8000.0f; // Adjust this value as needed
   
    private float dashCooldown = 2.0f; // Adjust this value as needed
    private float cooldownTimer = 1.0f;
    private float dashEndTime = 2.0f;
    private GameObject player;

 


   
    public override void Use()
    {
        
    }

    public override void WhileHolding()
    {
        player = PlayerManager.Instance.gameObject;
        rb = PlayerManager.Instance.GetComponent<Rigidbody2D>();
        // Update cooldown timer
        cooldownTimer -= Time.deltaTime;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f; // Assuming your game is 2D, adjust if it's 3D
        // Check for dash input if cooldown is not active
        if (cooldownTimer <= 0.0f && Input.GetMouseButtonDown(0)) // Assuming left mouse button triggers dash
        {
            player = PlayerManager.Instance.gameObject;
            rb = PlayerManager.Instance.GetComponent<Rigidbody2D>();


            SoundManager.Instance.PlaySound(13);
            // Calculate the direction to dash towards
            Vector3 dashDirection = (mousePos - player.transform.position).normalized;

            // Apply force for dashing
            rb.AddForce(dashDirection * dashForce);

          
            cooldownTimer = dashCooldown; // Set the cooldown
        }

        // Check if the dash duration has passed
        if (cooldownTimer >= dashEndTime)
        {
            rb.velocity = Vector2.zero; // Stop the player's movement
        }
    }

    
}
