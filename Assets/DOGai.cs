using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormAI : MonoBehaviour
{
    [SerializeField] private Transform targetPoint; // Assign the target point in the Inspector
    [SerializeField] private float moveForce = 5f; // Adjust the movement force as needed
    [SerializeField] private float maxRotationAngle = 45f; // Adjust the maximum rotation angle as needed
    [SerializeField] private float maxVelocityMagnitude = 5f; // Maximum allowed velocity magnitude

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (targetPoint != null)
        {
            // Calculate the direction to the target point
            Vector2 direction = (targetPoint.position - transform.position).normalized;

            // Calculate the angle to face the target point
            float targetRotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;

            // Limit rotation to a maximum angle
            targetRotation = Mathf.Clamp(targetRotation, -maxRotationAngle, maxRotationAngle);

            // Set the rotation directly
            rb.rotation = targetRotation;


            // Calculate the new velocity based on the direction and speed
            Vector2 newVelocity = direction * moveForce;


            // Limit the velocity magnitude
            if (newVelocity.magnitude > maxVelocityMagnitude)
            {
                newVelocity = newVelocity.normalized * maxVelocityMagnitude;
            }
            // Apply the new velocity to the Rigidbody
            rb.velocity = newVelocity;
            // Apply force to the Rigidbody to move the enemy
            //rb.AddForce(direction * moveForce * Time.deltaTime);
        }
    }
}