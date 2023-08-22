//using UnityEngine;

//public class KINGKEEGAN : MonoBehaviour
//{
//    public float moveSpeed = 2.0f;
//    public float chargeSpeedMultiplier = 3.0f;
//    public float chargeDuration = 2.0f;
//    public float stoppingDistance = 1.5f;
//    public float restDuration = 1.0f;
//    public string playerTag = "Player";

//    private Transform playerTransform;
//    private Rigidbody2D rb;
//    private float lastSwitchTime; // Time when the movement switch happened
//    private Vector2 targetPosition; // Target position for the enemy's movement

//    private void Start()
//    {
//        playerTransform = GameObject.FindGameObjectWithTag(playerTag).transform;
//        rb = GetComponent<Rigidbody2D>();
//        lastSwitchTime = Time.time;
//    }

//    private void Update()
//    {
//        Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
//        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

//        // Check if enough time has passed to switch movement
//        if (Time.time - lastSwitchTime >= 5.0f)
//        {
//            lastSwitchTime = Time.time;

//            // Calculate a random position within a 15 unit radius of the player
//            Vector2 randomOffset = Random.insideUnitCircle.normalized * 15f;
//            targetPosition = (Vector2)playerTransform.position + randomOffset;
//        }

//        // Move towards the player or the random target based on distance
//        if (distanceToPlayer > stoppingDistance)
//        {
//            rb.velocity = directionToPlayer * moveSpeed * chargeSpeedMultiplier;
//        }
//        else
//        {
//            rb.velocity = Vector2.zero;
//        }

//        // Check if the enemy should rest
//        if (distanceToPlayer <= stoppingDistance && Time.time - lastSwitchTime >= chargeDuration)
//        {
//            rb.velocity = Vector2.zero;
//            // Rest for the specified duration
//            if (Time.time - lastSwitchTime >= chargeDuration + restDuration)
//            {
//                // Resume movement towards the player
//                lastSwitchTime = Time.time;
//            }
//        }

//        // Move towards the random target if enough time has passed
//        if (Time.time - lastSwitchTime <= chargeDuration)
//        {
//            Vector2 directionToTarget = (targetPosition - (Vector2)transform.position).normalized;
//            rb.velocity = directionToTarget * moveSpeed;
//        }
//    }
//}

//using UnityEngine;

//public class KINGKEEGAN : MonoBehaviour
//{
//    public float moveSpeed = 2.0f;
//    public float chargeSpeedMultiplier = 3.0f;
//    public float chargeDuration = 2.0f;
//    public float stoppingDistance = 1.5f;
//    public float restDuration = 1.0f;
//    public string playerTag = "Player";

//    private Transform playerTransform;
//    private Rigidbody2D rb;
//    private float lastSwitchTime; // Time when the movement switch happened
//    private Vector2 targetPosition; // Target position for the enemy's movement

//    private void Start()
//    {
//        playerTransform = GameObject.FindGameObjectWithTag(playerTag).transform;
//        rb = GetComponent<Rigidbody2D>();
//        lastSwitchTime = Time.time;
//    }

//    private void Update()
//    {
//        Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
//        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

//        // Check if enough time has passed to switch movement
//        if (Time.time - lastSwitchTime >= 5.0f)
//        {
//            lastSwitchTime = Time.time;

//            // Calculate a random position within a 15 unit radius of the player
//            Vector2 randomOffset = Random.insideUnitCircle.normalized * 15f;
//            targetPosition = (Vector2)playerTransform.position + randomOffset;
//        }

//        // Move towards the player or the random target based on distance
//        if (distanceToPlayer > stoppingDistance)
//        {
//            rb.velocity = directionToPlayer * moveSpeed * chargeSpeedMultiplier;

//            // Rotate to face the movement direction
//            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
//            transform.rotation = Quaternion.Euler(0, 0, angle - 90);
//        }
//        else
//        {
//            rb.velocity = Vector2.zero;
//        }

//        // Check if the enemy should rest
//        if (distanceToPlayer <= stoppingDistance && Time.time - lastSwitchTime >= chargeDuration)
//        {
//            rb.velocity = Vector2.zero;
//            // Rest for the specified duration
//            if (Time.time - lastSwitchTime >= chargeDuration + restDuration)
//            {
//                // Resume movement towards the player
//                lastSwitchTime = Time.time;
//            }
//        }

//        // Move towards the random target if enough time has passed
//        if (Time.time - lastSwitchTime <= chargeDuration)
//        {
//            Vector2 directionToTarget = (targetPosition - (Vector2)transform.position).normalized;
//            rb.velocity = directionToTarget * moveSpeed;

//            // Rotate to face the movement direction
//            float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
//            transform.rotation = Quaternion.Euler(0, 0, angle - 90);
//        }
//    }
//}

using UnityEngine;

public class KINGKEEGAN : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    public float chargeSpeedMultiplier = 3.0f;
    public float chargeDuration = 2.0f;
    public float stoppingDistance = 1.5f;
    public float restDuration = 1.0f;
    public float randomOffsetRadius = 15f; // Adjust this radius as needed
    public string playerTag = "Player";

    private Transform playerTransform;
    private Rigidbody2D rb;
    private float lastSwitchTime; // Time when the movement switch happened
    private Vector2 targetPosition; // Target position for the enemy's movement
    private Vector2 lastDirectionToPlayer; // Direction to the player before target switch
    private Vector2 lastPlayerPosition;
    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag(playerTag).transform;
        rb = GetComponent<Rigidbody2D>();
        lastSwitchTime = Time.time;

        targetPosition = playerTransform.position;
    }

    private void Update()
    {
        Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (Time.time - lastSwitchTime >= chargeDuration + restDuration)
        {
            lastSwitchTime = Time.time;

            // Store the direction to the player before switching targets
            lastDirectionToPlayer = directionToPlayer;

            // Store the last position of the player
            lastPlayerPosition = playerTransform.position;

            // Calculate random offsets for x and y within the specified radius
            float randomOffsetX = Random.Range(-randomOffsetRadius, randomOffsetRadius);
            float randomOffsetY = Random.Range(-randomOffsetRadius, randomOffsetRadius);

            targetPosition = (Vector2)lastPlayerPosition + new Vector2(randomOffsetX, randomOffsetY);

            //targetPosition = (Vector2)playerTransform.position + new Vector2(randomOffsetX, randomOffsetY);
        }

        if (distanceToPlayer > stoppingDistance)
        {
            Vector2 movementDirection = (targetPosition - (Vector2)transform.position).normalized;

            // Use the stored direction to the player if transitioning from random target
            if (Time.time - lastSwitchTime <= chargeDuration)
            {
                movementDirection = lastDirectionToPlayer;
            }

            rb.velocity = movementDirection * moveSpeed * (distanceToPlayer > chargeDuration ? chargeSpeedMultiplier : 1f);

            float angle = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        }
        else
        {
            rb.velocity = Vector2.zero;
        }

        if (distanceToPlayer <= stoppingDistance && Time.time - lastSwitchTime >= chargeDuration)
        {
            rb.velocity = Vector2.zero;

            if (Time.time - lastSwitchTime >= chargeDuration + restDuration)
            {
                lastSwitchTime = Time.time;
            }
        }
    }
}