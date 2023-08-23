using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indianreddot : MonoBehaviour
{
    public Transform playerTransform; // Reference to the player's transform
    public float circleRadius = 2.0f; // Radius of the circular motion
    public float circleSpeed = 1.0f;  // Speed of circling

    public float zigZagDistance = 1.0f; // Distance for zig-zag motion
    public float zigZagSpeed = 2.0f;    // Speed of zig-zag motion

    public float StarDistance = 1.0f; // Distance for zig-zag motion
    public float StarSpeed = 2.0f;    // Speed of zig-zag motion

    public float doubleDashDistance = 3.0f; // Distance for double dash motion
    public float doubleDashSpeed = 3.0f;    // Speed of double dash motion

    public float idleDuration = 2.0f; // Duration of the idle state
    private float idleTimer = 0.0f;

    private float angle = 0.0f;

    private float circleTimer = 0.0f;
    private float zigZagTimer = 0.0f;
    private float doubleDashTimer = 0.0f;
    private float StarTimer = 0.0f;



    private int currentZigZagPointIndex = 0;
    private enum MovementState
    {
        Idle,
        Circle,
        ZigZag,
        DoubleDash,
        Star
    }

    private MovementState currentState = MovementState.Circle;

    private float stateDuration = 5.0f; // Duration of each non-idle state
    private float stateTimer = 0.0f;



    private Vector2[] zigZagPoints = new Vector2[2]; // Store the top-right and bottom-left points
    private int currentZigZagIndex = 0; // Index to alternate between the points

    private Vector2[] StarPoints = new Vector2[5]; // Store the top-right and bottom-left points
    private int currentStarIndex = 0; // Index to alternate between the points

    private bool movingToTopRight = true;



    void Start()
    {
        //SetStarPoints();
    }
    void Update()
    {
        // ... (your existing Update function)

        stateTimer += Time.deltaTime;
        if (stateTimer >= stateDuration)
        {
            SwitchToRandomState();
            stateTimer = 0.0f;
        }

        switch (currentState)
        {
            case MovementState.Idle:
                UpdateIdleState();
                break;
            case MovementState.Circle:
                UpdateCircleMotion();
                circleTimer += Time.deltaTime;
                if (circleTimer >= stateDuration)
                {
                    currentState = MovementState.Idle;
                    circleTimer = 0.0f;
                }
                break;
            case MovementState.Star:

                // Initialize the top-right and bottom-left points

                UpdateStarMotion();
                StarTimer += Time.deltaTime;
                if (StarTimer >= 30.0f)
                {
                    currentState = MovementState.Idle;
                    StarTimer = 0.0f;
                }
                break;
            case MovementState.ZigZag:

                // Initialize the top-right and bottom-left points

                UpdateZigZagMotion();
                zigZagTimer += Time.deltaTime;
                if (zigZagTimer >= stateDuration)
                {
                    currentState = MovementState.Idle;
                    zigZagTimer = 0.0f;
                }
                break;
            case MovementState.DoubleDash:
                UpdateDoubleDashMotion();
                doubleDashTimer += Time.deltaTime;
                if (doubleDashTimer >= stateDuration)
                {
                    currentState = MovementState.Idle;
                    doubleDashTimer = 0.0f;
                }
                break;
        }
    }

    private void UpdateIdleState()
    {
        idleTimer += Time.deltaTime;
        if (idleTimer >= idleDuration)
        {
            SwitchToRandomState();
            idleTimer = 0.0f;
        }
    }

    private void UpdateCircleMotion()
    {
        angle += circleSpeed * Time.deltaTime;
        Vector2 circlePosition = (Vector2)playerTransform.position + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * circleRadius;
        transform.position = circlePosition;
    }



    private void UpdateZigZagMotion()
    {
        Vector2 targetPosition = zigZagPoints[currentZigZagIndex];

        // Move towards the target position
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, zigZagSpeed * Time.deltaTime);

        // Check if the object has reached the target position
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            currentZigZagIndex = (currentZigZagIndex + 1) % 2; // Alternate between 0 and 1

            // Set new zigzag points
            SetNewZigZagPoints();
        }
    }

    private void SetNewZigZagPoints()
    {
        Vector2 playerPosition = playerTransform.position;

        zigZagPoints[0] = new Vector2(playerPosition.x + zigZagDistance, playerPosition.y + zigZagDistance);
        zigZagPoints[1] = new Vector2(playerPosition.x - zigZagDistance, playerPosition.y - zigZagDistance);
    }

    private void SetStarPoints()
    {
        Vector2 playerPosition = playerTransform.position;

        StarPoints[0] = new Vector2(playerPosition.x - StarDistance, playerPosition.y);
        StarPoints[1] = new Vector2(playerPosition.x + StarDistance, playerPosition.y);
        StarPoints[2] = new Vector2(playerPosition.x - StarDistance / 2, playerPosition.y - StarDistance);
        StarPoints[3] = new Vector2(playerPosition.x, playerPosition.y + StarDistance);
        StarPoints[4] = new Vector2(playerPosition.x + StarDistance / 2, playerPosition.y - StarDistance);
    }

    private void UpdateStarMotion()
    {
        Debug.Log("Yirr");
        Vector2 targetPosition = StarPoints[currentStarIndex];

        // Move towards the target position
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, StarSpeed * Time.deltaTime);

        // Check if the object has reached the target position
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            currentStarIndex = (currentStarIndex + 1) % 5; // Cycle through all 5 points

            // Set new zigzag points
            SetStarPoints();
        }
    }
    private void UpdateDoubleDashMotion()
    {
        float doubleDashX = Mathf.PingPong(Time.time * doubleDashSpeed, doubleDashDistance) - doubleDashDistance / 2.0f;
        Vector3 doubleDashPosition = playerTransform.position + new Vector3(doubleDashX, 0.0f, 0.0f);
        transform.position = doubleDashPosition;
    }

    private void SwitchToRandomState()
    {
        currentState = (MovementState)Random.Range(1, System.Enum.GetValues(typeof(MovementState)).Length); // Exclude Idle state
    }
}
