

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenBeeState : MonoBehaviour
{
    public enum BeeState
    {
        Idle,
        Hovering,
        Dashing,
        Orbit,

    }

    public float hoverAmplitude = 0.5f;
    public float hoverFrequency = 1.0f;
    public float dashSpeed = 5.0f;
    public float dashDuration = 2.0f;

    [SerializeField] private GameObject Player;
    private Vector3 initialPosition;
    private BeeState currentState = BeeState.Idle;

    private int dashDirection = 1;

    private float HoverDuration = (Mathf.PI) * 4;

    private float DashTimer;

    private Rigidbody2D rb;

    private float HoverTimer;

    public float maxXOffset = 5.0f; // Adjust this value as needed
    public float maxYOffset = 4.0f; // Adjust this value as needed


    private float nextStateTimer = 0.0f;
    private float nextStateDuration = 3.0f; // Adjust this duration as needed

    private QueenBeeAi QueenBee;

    public float hoverSpeed = 5;
    private void Start()
    {
       QueenBee = GetComponent<QueenBeeAi>();
        initialPosition = Player.transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
         HandleState();

        
    }

    private void HandleState()
    {
        switch (currentState)
        {
            case BeeState.Idle:
                HandleIdleState();
                break;

            case BeeState.Hovering:
                HandleHoveringState();
                break;

            case BeeState.Dashing:
                HandleDashingState();
                break;
            case BeeState.Orbit:
                HandleOrbit();
                break;
        }

        //// Check for input to trigger dash
        if (Input.GetKeyDown(KeyCode.Space) && currentState == BeeState.Idle)
        {
            TransitionToDashingState();
        }
    }

    private void HandleIdleState()
    {
        // Calculate the horizontal offset based on time
        float xOffset = Mathf.Sin(Time.time * hoverFrequency * 10) * maxXOffset;



        if (rb != null)
        {
            // Calculate the new position with the updated offset
            Vector2 newPosition = (Vector2)Player.transform.position + new Vector2(xOffset, maxYOffset);

            // Move the Player using Rigidbody2D
            rb.MovePosition(newPosition);
        }
        else
        {
            Debug.LogError("Rigidbody2D component not found on the Player GameObject.");
        }
    }




    private void HandleHoveringState()
    {



        // Calculate vertical position using a sine wave to create the up and down movement
        float verticalOffset = Mathf.Sin(HoverTimer) * hoverAmplitude;

        // Smoothly move the bee towards the target vertical position
        Vector2 targetPosition = new Vector2(transform.position.x, Player.transform.position.y + verticalOffset);
        Vector2 newPosition = Vector2.Lerp(transform.position, targetPosition, Time.deltaTime * hoverSpeed);

        // Apply horizontal clamping
        newPosition.x = Mathf.Clamp(newPosition.x, Player.transform.position.x - maxXOffset, Player.transform.position.x + maxXOffset);

        transform.position = newPosition;

        HoverTimer += Time.deltaTime * hoverFrequency;

        if (HoverTimer >= Mathf.PI * 2)
        {
            HoverTimer = 0;
            TransitionToDashingState();
        }
    }

    private void HandleOrbit()
    {

        // Calculate the new position for orbiting
        float angle = Time.time * 5;
        float x = Mathf.Sin(angle) * 5;
        float y = Mathf.Abs(Mathf.Cos(angle)) * 5;

        Vector2 newPosition = (Vector2)Player.transform.position + new Vector2(x, y);
        transform.position = newPosition;
    }

    private void HandleDashingState()
    {
        Debug.Log(dashDirection);
        Vector2 playerToBeeDirection = (Vector2)Player.transform.position - (Vector2)transform.position;

        if (playerToBeeDirection.x > 0)
        {
            dashDirection = 1;
        }
        else if (playerToBeeDirection.x < 0)
        {
            dashDirection = -1;
        }

        // Apply gradual force for smoother acceleration and deceleration
        float dashForce = dashDirection * dashSpeed * (1 - DashTimer / dashDuration);
        rb.AddForce(new Vector2(dashForce, 0), ForceMode2D.Force);

        DashTimer += Time.deltaTime;

        if (DashTimer >= dashDuration)
        {
            DashTimer = 0;
            TransitionToHoveringState();
        }

    }

    private void TransitionToDashingState()
    {
        currentState = BeeState.Dashing;
    }

    private void TransitionToHoveringState()
    {
        currentState = BeeState.Hovering;
    }

    private void TransitionToOrbitState()
    {
        currentState = BeeState.Orbit;
    }

    private void TransitionToIdleState()
    {
        currentState = BeeState.Idle;
    }
}