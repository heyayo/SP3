using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/HellZoneDa")]

public class HellZoneGrenade : Item
{
    public GameObject projectilePrefab;
    public int projectileCount = 8;
    public float spreadAngle = 360f;
    public float minLaunchForce = 5f;
    public float maxLaunchForce = 15f;
    public float maxCurveAmount = 3f;
    public float abilityCooldown = 3f;
    public float launchDelay = 0.5f;

    private bool abilityUsed = false;
    private float cooldownTimer = 0f;
    private Transform player;

    public override void Use()
    {
        if (!abilityUsed && cooldownTimer <= 0f)
        {
            abilityUsed = true;
            cooldownTimer = abilityCooldown;
            LaunchProjectiles();
        }
    }

    public override void WhileHolding()
    {
        player = PlayerManager.Instance.gameObject.transform;
        // Implement any behavior you want while holding the item
        // For example, checking if the ability can be used again
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }
        else if (abilityUsed)
        {
            abilityUsed = false;
        }
    }

    private void LaunchProjectiles()
    {
        for (int i = 0; i < projectileCount; i++)
        {
            float angle = spreadAngle * i / (projectileCount - 1) - spreadAngle / 2f;
            Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

            GameObject projectile = Instantiate(projectilePrefab, player.position, rotation);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

            float launchForce = Random.Range(minLaunchForce, maxLaunchForce);
            float curveAmount = Random.Range(-maxCurveAmount, maxCurveAmount);

            Vector2 launchDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector3)player.position).normalized;
            Vector2 curvedDirection = Quaternion.Euler(0, 0, curveAmount) * launchDirection;

            rb.velocity = curvedDirection * launchForce;

            while (Input.GetKey(KeyCode.E))
            {
                Vector2 currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 updatedLaunchDirection = (currentMousePosition - (Vector2)player.position).normalized;

                rb.velocity = updatedLaunchDirection * launchForce;
            }

            rb.velocity = Vector2.zero; // Make sure the velocity is set to zero when not holding the key

            float waitTime = launchDelay;
            while (waitTime > 0f)
            {
                waitTime -= Time.deltaTime;
                if (!Input.GetKey(KeyCode.J))
                {
                    break; // Exit the loop if the key is released
                }
            }
        }

    }


}
