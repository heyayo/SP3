using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Items/Spiritbombl")]
public class SpiritBombAbility : Item
{
    public float scaleFactor = 2f; // Adjust this factor to make the scaling more noticeable

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
        //SpiritBombPrefab = Instantiate(SpiritBombPrefab);
        //SpiritBombPrefab.SetActive(false);
    }
    override public void WhileHolding()
    {
        player = PlayerManager.Instance.gameObject;

        if (!isOnCooldown)
        {
            if (Input.GetMouseButtonDown(0))
            {
                SpiritBombPrefab = Instantiate(SpiritBombPrefab);
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

                
                //SpiritBombPrefab.SetActive(false);
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

    public override void Use()
    {


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
        GameObject spiritBomb = SpiritBombPrefab;

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

        Destroy(spiritBomb, 10.0f);

    }

    private void OnDisable()
    {
        
    }
}
