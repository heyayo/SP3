using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadFollow : MonoBehaviour
{
    private Transform playerTag; // Reference to the player's tag object
    public float orbitRadius = 5f; // Radius of the semicircle orbit
    public float orbitSpeed = 1f; // Speed of the boss's orbit
    public float waypointSwitchInterval = 5f; // Interval to switch between states

    private Vector2 initialPosition; // Initial position of the boss

    [SerializeField] GameObject Skelly;
    private enum BossState
    {
        Orbiting,
        FollowingPlayer
    }

    private BossState currentState;
    private float stateTimer = 0f;

    private void Start()
    {
        Skelly.GetComponent<Mortality>().onHealthZero.AddListener(Death);

        playerTag = PlayerManager.Instance.transform;
        initialPosition = transform.position;
        currentState = BossState.Orbiting;
    }

    private void Update()
    {
        stateTimer += Time.deltaTime;

        // Check if it's time to switch states
        if (stateTimer >= waypointSwitchInterval)
        {
            stateTimer = 0f;

            // Switch between states
            if (currentState == BossState.Orbiting)
            {
                currentState = BossState.FollowingPlayer;
            }
            else
            {
                currentState = BossState.Orbiting;
            }
        }

        // Update behavior based on the current state
        switch (currentState)
        {
            case BossState.Orbiting:
                UpdateOrbitingState();
                break;

            case BossState.FollowingPlayer:
                UpdateFollowingPlayerState();
                break;
        }
    }

    private void UpdateOrbitingState()
    {
      


        if (Skelly != null)
        {
            // Get the Animator component from the GameObject with the tag "SkeletronHead"
            Animator headAnimator = Skelly.GetComponent<Animator>();

            if (headAnimator != null)
            {

                // Set the "spin" parameter to true
                headAnimator.SetBool("spin", false);

            }
        }
        // Calculate the new position for orbiting
        float angle = Time.time * orbitSpeed;
        float x = Mathf.Sin(angle) * orbitRadius;
        float y = Mathf.Abs(Mathf.Cos(angle)) * orbitRadius;

        Vector2 newPosition = (Vector2)playerTag.position + new Vector2(x, y);
        transform.position = newPosition;
    }

    private void UpdateFollowingPlayerState()
    {
       

        if (Skelly != null)
        {
            // Get the Animator component from the GameObject with the tag "SkeletronHead"
            Animator headAnimator = Skelly.GetComponent<Animator>();

            if (headAnimator != null)
            {
               
                    // Set the "spin" parameter to true
                    headAnimator.SetBool("spin", true);
                
            }
        }

        // Set the waypoint to the player's position
        transform.position = playerTag.position;
    }

    private void Death()
    {
        BossManager.Instance.KillBoss("Skeletron");
        Destroy(gameObject.transform.parent.gameObject);
    }
}