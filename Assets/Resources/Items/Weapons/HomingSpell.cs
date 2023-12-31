using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/HomingSpell")]
public class HomingSpell : Item
{
    public GameObject projectilePrefabs; // An array of projectile prefabs
    public float projectileSpeed = 10f;

    private Transform playerTransform; // Reference to the player's transform

    private void OnEnable()
    {
       
    }

    public override void Use()
    {
        // Find and assign the player's transform using the tag "Player"
        PlayerManager playerObject = PlayerManager.Instance;
        if (playerObject == null)
            return;
        var mortality = playerObject.MortalityScript;
        if (mortality.ActiveEnergy < 40)
            return;
        mortality.ActiveEnergy -= 40;
        playerTransform = playerObject.transform;

        if (playerTransform == null)
        {
            Debug.LogWarning("Player transform is not assigned.");
            return;
        }

        // Shooting sound
        SoundManager.Instance.PlaySound(4);

        // Get the mouse position in world coordinates
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        // Calculate the direction towards the mouse
        Vector2 direction = (mousePosition - playerTransform.position).normalized;

        // Instantiate the selected projectile prefab
        GameObject projectile = Instantiate(projectilePrefabs, playerTransform.position, Quaternion.identity);
        projectile.GetComponent<DamageSource>().__NativeHPDamage += playerObject.AttackDamage;

        // Get the Rigidbody2D of the projectile
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        // Apply force to the projectile in the calculated direction
        rb.AddForce(direction * projectileSpeed, ForceMode2D.Impulse);

        // Calculate rotation to face the movement direction
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}