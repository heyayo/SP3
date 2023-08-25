using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dashspell : Item
{
    private Rigidbody2D rb;
    private float dashForce = 10.0f; // Adjust this value as needed
    private float dashDuration = 1.5f; // Adjust this value as needed
    private float dashCooldown = 2.0f; // Adjust this value as needed
    private float cooldownTimer = 0.0f;
    private float dashEndTime = 0.0f;
    private GameObject player;

    private void Start()
    {
        player = PlayerManager.Instance.gameObject;
        rb = PlayerManager.Instance.GetComponent<Rigidbody2D>();
    }

    public override void Use()
    {

    }

    public override void WhileHolding()
    {
        // Update cooldown timer
        cooldownTimer -= Time.deltaTime;

        // Check for dash input if cooldown is not active
        if (cooldownTimer <= 0.0f && Input.GetMouseButtonDown(0)) // Assuming left mouse button triggers dash
        {
            DashTowardsMousePosition();
            cooldownTimer = dashCooldown; // Set the cooldown
        }

        // Check if the dash duration has passed
        if (Time.time >= dashEndTime)
        {
            rb.velocity = Vector2.zero; // Stop the player's movement
        }
    }

    private void DashTowardsMousePosition()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f; // Assuming your game is 2D, adjust if it's 3D

        // Calculate the direction to dash towards
        Vector3 dashDirection = (mousePos - player.transform.position).normalized;

        // Apply force for dashing
        rb.velocity = dashDirection * dashForce;

        // Set the time when the dash will end
        dashEndTime = Time.time + dashDuration;
    }
}
