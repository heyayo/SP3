using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EOCTransformSOBase : ScriptableObject
{
    protected Enemy enemy;
    protected Transform transform;
    protected GameObject gameObject;
    protected Rigidbody2D rb;

    protected Transform playerTransform;

    // Private variables
    float rotatedAmount;
    
    public virtual void Init(GameObject gameObject, Enemy enemy)
    {
        this.gameObject = gameObject;
        transform = gameObject.transform;
        this.enemy = enemy;

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        rb = playerTransform.gameObject.GetComponent<Rigidbody2D>();
    }

    public virtual void DoEnterLogic() 
    { 
        rotatedAmount = 0f;
    }
    public virtual void DoExitLogic() { ResetValue(); }
    public virtual void DoFrameUpdateLogic() 
    {
        //Quaternion toRotate = Quaternion.AngleAxis(6.3f - transform.rotation.x, new Vector3(0, 0, 1));
        float rotationSpeed = 1f;
        float rotationAmount = 720f;

        // Slow > fast rotation
        if (rotatedAmount < rotationAmount)
            gameObject.GetComponent<Rigidbody2D>().AddTorque(rotationSpeed);

        rotatedAmount += rotationSpeed;

        if (rotatedAmount >= rotationAmount && gameObject.GetComponent<Rigidbody2D>().angularVelocity <= 10f)
        {
            enemy.enemyAnimator.SetBool("Enraged", true);
            enemy.stateMachine.ChangeState(enemy.GetComponent<EyeOfCthulhu>().enragedChaseState);
        }
    }
    public virtual void DoPhysicsLogic() { }
    public virtual void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType) { }
    public virtual void ResetValue() { }
}
