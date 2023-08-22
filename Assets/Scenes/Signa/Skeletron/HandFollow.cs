using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandFollow : MonoBehaviour
{
    [SerializeField] private Transform targetPoint; // Assign the target point in the Inspector
    [SerializeField] private float moveForce = 5f; // Adjust the movement force as needed
    [SerializeField] private float maxRotationAngle = 45f; // Adjust the maximum rotation angle as needed
    [SerializeField] private float maxVelocityMagnitude = 5f; // Maximum allowed velocity magnitude
    [SerializeField] private float maxFollowRadius = 10f; // Maximum radius to follow the target

    public bool activate = false;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            activate = !activate;
        }

        if (targetPoint != null && activate == true)
        {
            // Calculate the distance to the target point
            float distanceToTarget = Vector2.Distance(transform.position, targetPoint.position);

            // Check if within follow radius
            if (distanceToTarget <= maxFollowRadius)
            {
                // Calculate the direction to the target point
                Vector2 direction = (targetPoint.position - transform.position).normalized;

                // Calculate the new velocity based on the direction and speed
                Vector2 newVelocity = direction * moveForce;

                // Limit the velocity magnitude
                if (newVelocity.magnitude > maxVelocityMagnitude)
                {
                    newVelocity = newVelocity.normalized * maxVelocityMagnitude;
                }

                // Apply the new velocity to the Rigidbody
                rb.velocity = newVelocity;
            }
            else
            {
                // Stop moving if outside follow radius
                rb.velocity = Vector2.zero;
            }
        }
    }
}
