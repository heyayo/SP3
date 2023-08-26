using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Items/Teleportskill")]
public class Teleport : Item
{
    private bool isTeleporting = false;
    private Transform PLAYERPOS;
    private float teleportCooldown = 2.0f; // Adjust this value as needed
    private float cooldownTimer = 0.0f;


    public override void Use()
    {
        // Check for teleport input if cooldown is not active
        if (cooldownTimer <= 0.0f) // Assuming left mouse button triggers teleport
        {
            TeleportToMousePosition();
            cooldownTimer = teleportCooldown; // Set the cooldown
        }
    }

    public override void WhileHolding()
    {
        // Update cooldown timer
        cooldownTimer -= Time.deltaTime;
<<<<<<< HEAD
=======

        // Check for teleport input if cooldown is not active
        if (cooldownTimer <= 0.0f && Input.GetMouseButtonDown(0)) // Assuming left mouse button triggers teleport
        {
            SoundManager.Instance.PlaySound(12);
            TeleportToMousePosition();
            cooldownTimer = teleportCooldown; // Set the cooldown
        }
>>>>>>> aa58a3db220286bb4cb2b93a388f68cab9f397e4
    }

    private void TeleportToMousePosition()
    {
        PLAYERPOS =PlayerManager.Instance.transform;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f; // Assuming your game is 2D, adjust if it's 3D
        PLAYERPOS.position = mousePos;

    }


}
