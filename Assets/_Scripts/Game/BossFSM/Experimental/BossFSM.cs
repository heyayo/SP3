using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFSM : MonoBehaviour
{
    [SerializeField]
    private BossState[] states;

    private int currentIndex;
    private BossState currentState;
    private int resetToState = 1;

    // Private Variables
    protected Mortality mortality;
    protected Transform playerTransform;
    protected Mortality playerMortality;
    protected bool enraged;
    protected LayerMask playerLayer;

    // Private variables
    Vector2 dir;
    Rigidbody2D rb;
    private Animator animator;

    private void Start()
    {
        // GetComponents
        mortality = gameObject.GetComponent<Mortality>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerMortality = GameObject.FindGameObjectWithTag("Player").GetComponent<Mortality>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Settling FSM
        currentIndex = 1;
        currentState = states[currentIndex - 1];
        currentState.InitState(mortality, playerTransform, playerMortality, rb, transform, playerLayer, animator);
    }

    private void FixedUpdate()
    {
        Debug.Log(currentState);

        if (currentState.DoState())
        {
            currentState.ExitState();

            // After transformation, set the next state to be the default to cycle back to
            if (canTransform())
            {
                resetToState = currentIndex;
                currentIndex++;
            }
            // If reached the final state in the list or if the next state is for transformation
            else if (currentIndex == states.Length || states[currentIndex].isTransformState)
            {
                currentIndex = resetToState;
            }            // Incrementing the states
            else
            {
                currentIndex++;
            }

            currentState = states[currentIndex - 1];
            currentState.InitState(mortality, playerTransform, playerMortality, rb, transform, playerLayer, animator);
        }
    }

    private bool canTransform()
    {
        for (int i = resetToState - 1; i < states.Length; i++)
        {
            if (states[i].isTransformState)
            {
                return states[i].isReadyToTransform();
            }
        }

        return false;
    }
}
