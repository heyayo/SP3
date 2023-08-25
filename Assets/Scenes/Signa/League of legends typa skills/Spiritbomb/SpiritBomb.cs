using UnityEngine;
using UnityEngine.UI;

public class SpiritBomb: MonoBehaviour
{
    public float  scaleFactor = 2f; // Adjust this factor to make the scaling more noticeable

    public float maxChargeTime = 3f; // Maximum charge time in seconds
    public float chargeSpeed = 1f; // Charging speed

    public GameObject SpiritBombPrefab; // Reference to the charging indicator GameObject
    private float chargeTime = 0f; // Current charge time
    private bool isCharging = false; // Is the player charging?

    private GameObject player;



    public float cooldownDuration = 5f; // Cooldown duration in seconds
    private bool isOnCooldown = false; // Is the ability on cooldown?
    private float cooldownTimer = 0f; // Timer for tracking cooldown

    void Start()
    {
        player = PlayerManager.Instance.gameObject;
        //SpiritBombPrefab = Instantiate(SpiritBombPrefab);
        //SpiritBombPrefab.SetActive(false);
    }

    void Update()
    {
        if (!isOnCooldown)
        {
            if (Input.GetMouseButtonDown(0))
            {   SpiritBombPrefab = Instantiate(SpiritBombPrefab);
                isCharging = true;
                SpiritBombPrefab.SetActive(true);
            }

            if (isCharging)
            {
                // Increase charge time up to the maximum
                chargeTime += Time.deltaTime * chargeSpeed;
                chargeTime = Mathf.Clamp(chargeTime, 0f, maxChargeTime);


                SpiritBombPrefab.transform.localScale = Vector3.one * (chargeTime / maxChargeTime) * scaleFactor;
                SpiritBombPrefab.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 9, SpiritBombPrefab.transform.position.z);
            }

            if (Input.GetMouseButtonUp(0))
            {
                LaunchSpiritBomb();
                SpiritBombPrefab.SetActive(false);
                // Start the cooldown
                StartCooldown();
            }
        }

        else
        {
            // Update the cooldown timer
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                isOnCooldown = false;
            }
        }
    }

    void StartCooldown()
    {
        isCharging = false;
        isOnCooldown = true;
        cooldownTimer = cooldownDuration;
    }
    void LaunchSpiritBomb()
    {
        isCharging = false;

        // Instantiate the Spirit Bomb prefab
        GameObject spiritBomb = Instantiate(SpiritBombPrefab, new Vector3(player.transform.position.x, player.transform.position.y + 9, 0), Quaternion.identity);

        // Calculate the launch direction based on the mouse position
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 launchDirection = (mousePosition - (Vector2)player.transform.position).normalized;

        // Calculate launch speed based on charge time
        float launchSpeed = chargeTime * 10f; // Adjust the multiplier as needed

        // Apply velocity to the Spirit Bomb's Rigidbody2D
        Rigidbody2D rb = spiritBomb.GetComponent<Rigidbody2D>();
        rb.velocity = launchDirection * launchSpeed;

        // Reset charge values
        chargeTime = 0f;

        // Destroy the Spirit Bomb after 10 seconds
        Destroy(spiritBomb, 10f); // Change the delay duration as needed
    }
}