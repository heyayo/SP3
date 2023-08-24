using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DOGprojectile : MonoBehaviour
{
    public LayerMask enemyLayer;
    public float damageAmount = 10.0f;
    private Rigidbody2D rb;
    private float rotationOffset = -90.0f; // Offset for rotation

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 5.0f);
    }

    // Update is called once per frame
    void Update()
    {
        CheckForCollision();
    }

    void CheckForCollision()
    {
        Vector2 currentPosition = transform.position;
        Vector2 direction = rb.velocity.normalized; // Use the velocity direction as the firing direction

        RaycastHit2D hit = Physics2D.Raycast(currentPosition, direction, 1.0f, enemyLayer);

        if (hit.collider != null)
        {
            Destroy(gameObject); // Destroy the projectile
            // Handle collision logic here
        }

        // Calculate the rotation angle based on the firing direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + rotationOffset;
        transform.rotation = Quaternion.Euler(0, 0, angle); // Apply the rotation
    }
}