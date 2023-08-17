using UnityEngine;

public class KINGKEEGAN : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    public string playerTag = "Player";

    private Transform playerTransform;
    private Rigidbody2D rb;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag(playerTag).transform;
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;

        // Calculate the angle needed to face the player
        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;

        // Apply rotation to the boss's transform to face the player
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);

        // Move the boss towards the player
       // transform.position += (Vector3)directionToPlayer * moveSpeed * Time.fixedDeltaTime;
        rb.AddForce((Vector3)directionToPlayer * moveSpeed); 
    }
}