using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossState : ScriptableObject
{
    [SerializeField]
    public bool isTransformState = false;

    // Private Variables
    protected Mortality mortality;
    protected Transform playerTransform;
    protected Mortality playerMortality;
    protected bool enraged;
    protected LayerMask playerLayer;

    // Private variables
    protected Vector2 dir;
    protected Rigidbody2D rb;
    protected Animator animator;
    protected Transform transform;
    protected SpriteRenderer sr;
    protected DamageSource damageSource;

    public void InitState(Mortality mortality, Transform playerTransform, Mortality playerMortality, 
        Rigidbody2D rb, Transform transform, LayerMask playerLayer, Animator animator, SpriteRenderer sr, DamageSource damageSource)
    {
        // GetComponents
        this.mortality = mortality;
        this.playerTransform = playerTransform;
        this.playerMortality = playerMortality;
        this.rb = rb;
        this.transform = transform;
        this.playerLayer = playerLayer;
        this.animator = animator;
        this.sr = sr;
        this.damageSource = damageSource;
    }

    virtual public void EnterState()
    {
    }

    virtual public bool DoState() 
    {
        return false;
    }

    virtual public void ExitState()
    {
    }

    // Default this to true if this is not a tranformation state
    virtual public bool isReadyToTransform()
    {
        return true;
    }

    // ****************************************************
    // ****************************************************
    // Common Functions
    // ****************************************************
    // ****************************************************

    virtual protected void FacePlayer()
    {
        dir = (playerTransform.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90);
    }
}
