using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFSM : MonoBehaviour
{
    [Header("Init Boss")]
    [SerializeField] 
    private BossInit bossInit;
    
    [HideInInspector]
    public string bossName;

    [Header("States")]
    [SerializeField] 
    private BossState[] states;

    [Header("Sounds")]
    [SerializeField]
    protected int hitSoundIndex = 1;

    [SerializeField]
    protected int deathSound = 3;

    // Audio Source
    protected AudioSource audioSource;

    // Indexes
    private int currentIndex;
    private BossState currentState;
    private int resetToState;

    // Private Variables
    protected Mortality mortality;
    protected Transform playerTransform;
    protected Mortality playerMortality;
    protected LayerMask playerLayer;
    protected DamageSource damageSource;

    // Private variables
    //Vector2 dir;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sr;

    private void Start()
    {
        // GetComponents
        mortality = gameObject.GetComponent<Mortality>();
        playerTransform = PlayerManager.Instance.transform;
        playerMortality = PlayerManager.Instance.GetComponent<Mortality>();
        rb = GetComponent<Rigidbody2D>();
        damageSource = GetComponent<DamageSource>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        // Init boss stuff
        if (bossInit != null)
        {
            bossInit.Init(gameObject.transform);
            bossInit.BossInitialize();
        }

        // Settling FSM and indexes
        currentIndex = 1;
        currentState = states[currentIndex - 1];
        currentState.EnterState();
        resetToState = currentIndex;

        foreach (BossState state in states)
        {
            state.InitState(mortality, playerTransform, playerMortality, rb, transform, playerLayer, animator, sr);
        }

        GetComponent<Damagable>().hit.AddListener(HitSound);
        mortality.onHealthZero.AddListener(Death);
    }

    private void FixedUpdate()
    {
        // Always check if can transform
        doTransform();

        if (currentState.DoState())
        {
            currentState.ExitState();
            currentIndex++;

            // If reached the final state in the list or if the next state is for transformation
            if (currentIndex - 1 == states.Length || states[currentIndex - 1].isTransformState)
            {
                currentIndex = resetToState;
            }

            currentState = states[currentIndex - 1];
            currentState.EnterState();
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

    private void doTransform()
    {
        if (canTransform())
        {
            for (int i = currentIndex; i < states.Length; i++)
            {
                if (states[i].isTransformState)
                {
                    // Changing the index numbers
                    currentIndex = i + 1;
                    resetToState = currentIndex + 1;

                    // Going into state
                    currentState = states[currentIndex - 1];
                    currentState.EnterState();

                    //currentState.isTransformState = false;  // So it no longer checks for transformation on this state
                    break;
                }
            }
        }
    }
    
    private void HitSound()
    {
        SoundManager.Instance.PlaySound(hitSoundIndex);
    }

    private void Death()
    {
        SoundManager.Instance.PlaySound(deathSound);
        mortality.onHealthZero.AddListener(()=>{BossManager.Instance.KillBoss(bossName);});
        Destroy(gameObject);
    }
}
