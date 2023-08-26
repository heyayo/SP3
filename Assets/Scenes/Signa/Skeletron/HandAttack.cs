using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAttack : MonoBehaviour
{

    [SerializeField] private Transform targetPoint; // Assign the target point in the Inspector
    [SerializeField] private float moveForce = 5f; // Adjust the movement force as needed
    [SerializeField] private float maxRotationAngle = 45f; // Adjust the maximum rotation angle as needed
    [SerializeField] private float maxVelocityMagnitude = 5f; // Maximum allowed velocity magnitude

    public bool activate = false;
    private Rigidbody2D rb;

    private float activationTimer = 0f;
    private bool toggle = true;

    private void Start()
    {
        GetComponent<Mortality>().onHealthZero.AddListener(() => 
        { Destroy(transform.parent.gameObject); });

        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {


        // Increment the timer
        activationTimer += Time.deltaTime;

        // Check if it's time to toggle the activate boolean
        if (toggle && activationTimer >= Random.Range(4f, 5f))
        {
            activate = true;
            toggle = false;
            activationTimer = 0f; // Reset the timer
        }
        else if (!toggle && activationTimer >= 5f)
        {
            activate = false;
            toggle = true;
            activationTimer = 0f; // Reset the timer
        }


        if (targetPoint != null && activate == true)
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
            // Apply force to the Rigidbody to move the enemy
            //rb.AddForce(direction * moveForce * Time.deltaTime);
        }
    }
}
