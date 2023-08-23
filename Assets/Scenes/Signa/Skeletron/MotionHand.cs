using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionHand : MonoBehaviour
{
    public Transform pivotPoint; // Set this in the Inspector to the pivot point GameObject
    public Transform player; // Set this in the Inspector to the player GameObject
    public float amplitude = 1.0f; // The amount of vertical movement
    public float frequency = 1.0f; // The speed of the vertical movement
    public float semiCircleRadius = 2.0f; // Radius of the semi-circle movement around the player
    public float semiCircleSpeed = 1.0f; // Speed of the semi-circle movement
    public float stateChangeInterval = 5.0f; // Time interval to change states
    public float orbitRadius = 5f; // Radius of the semicircle orbit

    public float BackAndForthMove = 5f;
    public float BackAndForthSpeed = 5f;


    private Rigidbody2D rb;
    private enum MotionState
    {
        Vertical,
        Horizontal,
        SemiCircle
    }

    private MotionState currentState;
    
    private float stateChangeTimer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody component attached to this object
        //initialPosition = (Vector2)transform.position - (Vector2)pivotPoint.position;
        currentState = MotionState.Vertical;
        stateChangeTimer = stateChangeInterval;
    }

    private void Update()
    {
        stateChangeTimer -= Time.deltaTime;

        if (stateChangeTimer <= 0.0f)
        {
            RandomlyChangeState();
            stateChangeTimer = stateChangeInterval;
        }

        switch (currentState)
        {
            case MotionState.Vertical:
                MoveVertically();
                break;
            case MotionState.SemiCircle:
                MoveInSemiCircle();
                break;
            case MotionState.Horizontal:
                MoveHorizontally();
                break;
        }
    }

    private void MoveVertically()
    {

        float doubleDashX = Mathf.PingPong(Time.time * BackAndForthSpeed, BackAndForthMove) - BackAndForthMove / 2.0f;
        Vector2 doubleDashPosition = player.position + new Vector3(doubleDashX, 0.0f, 0.0f);

        // Calculate the desired velocity to reach the target position smoothly
        Vector2 desiredVelocity = (doubleDashPosition - (Vector2)transform.position) / Time.deltaTime;

        // Apply the desired velocity to the Rigidbody2D using interpolation for smoother movement
        rb.velocity = Vector2.Lerp(rb.velocity, desiredVelocity, 0.5f);

        // Update the position based on the Rigidbody2D's velocity
        transform.position += (Vector3)(rb.velocity * Time.deltaTime);
      
       
    }

    private void MoveHorizontally()
    {
        float doubleDashY = Mathf.PingPong(Time.time * BackAndForthSpeed, BackAndForthMove) - BackAndForthMove / 2.0f;
        Vector2 targetPosition = player.position + new Vector3(0, doubleDashY, 0.0f);

        // Calculate the desired velocity to reach the target position smoothly
        Vector2 desiredVelocity = (targetPosition - (Vector2)transform.position) / Time.deltaTime;

        // Apply the desired velocity to the Rigidbody2D using interpolation for smoother movement
        rb.velocity = Vector2.Lerp(rb.velocity, desiredVelocity, 0.5f);

        // Update the position based on the Rigidbody2D's velocity
        transform.position += (Vector3)(rb.velocity * Time.deltaTime);
    }

    private void MoveInSemiCircle()
    {
        float angle = Time.time * semiCircleSpeed;
        float x = Mathf.Cos(angle) * orbitRadius;
        float y = Mathf.Sin(angle) * orbitRadius;

        Vector2 newPosition = pivotPoint.position + new Vector3(x, y, 0f);
        transform.position = newPosition;

        //float angle = Time.time * 5;
        //float x = Mathf.Sin(angle) * orbitRadius;
        //float y = Mathf.Abs(Mathf.Cos(angle)) * orbitRadius;

        //// Check if the calculated 'y' is below the pivot point's 'y'
        //if (player.position.y < pivotPoint.position.y)
        //{
        //    y *= -1; // Add '-' to the 'y' coordinate
        //}

        //else
        //{
        //    y *= 1;
        //}

        //Vector2 newPosition = (Vector2)player.position + new Vector2(x, y);
        //transform.position = newPosition;
    }

    private void RandomlyChangeState()
    {
        currentState = (MotionState)Random.Range(0, System.Enum.GetValues(typeof(MotionState)).Length);
        //semiCircleAngle = Mathf.Atan2(transform.position.y - player.position.y, transform.position.x - player.position.x);
    }
}