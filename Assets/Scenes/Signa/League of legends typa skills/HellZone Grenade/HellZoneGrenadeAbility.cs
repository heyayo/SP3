using System.Collections;
using UnityEngine;

public class HellzoneGrenadeAbility : MonoBehaviour
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

    private void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }

        if (cooldownTimer <= 0f && Input.GetKeyDown(KeyCode.J))
        {
            StartCoroutine(LaunchProjectilesWithDelay());
            abilityUsed = true;
            cooldownTimer = abilityCooldown;
        }
        else if (abilityUsed && Input.GetKeyDown(KeyCode.J))
        {
            abilityUsed = false;
        }
    }

    private IEnumerator LaunchProjectilesWithDelay()
    {
        for (int i = 0; i < projectileCount; i++)
        {
            float angle = spreadAngle * i / (projectileCount - 1) - spreadAngle / 2f;
            Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

            GameObject projectile = Instantiate(projectilePrefab, transform.position, rotation);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

            float launchForce = Random.Range(minLaunchForce, maxLaunchForce);
            float curveAmount = Random.Range(-maxCurveAmount, maxCurveAmount);

            Vector2 launchDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector3)transform.position).normalized;
            Vector2 curvedDirection = Quaternion.Euler(0, 0, curveAmount) * launchDirection;

            rb.velocity = curvedDirection * launchForce;

            while (Input.GetKey(KeyCode.J))
            {
                Vector2 currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 updatedLaunchDirection = (currentMousePosition - (Vector2)transform.position).normalized;

                rb.velocity = updatedLaunchDirection * launchForce;

                yield return null;
            }

            yield return new WaitForSeconds(launchDelay);
        }
    }
}