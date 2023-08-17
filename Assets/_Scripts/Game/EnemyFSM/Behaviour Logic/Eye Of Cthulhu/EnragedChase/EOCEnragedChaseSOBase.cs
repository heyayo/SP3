using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EOCEnragedChaseSOBase : ScriptableObject
{
    protected Enemy enemy;
    protected Transform transform;
    protected GameObject gameObject;
    protected Rigidbody2D rb;

    protected Transform playerTransform;
    
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
    }
    public virtual void DoExitLogic() { ResetValue(); }
    public virtual void DoFrameUpdateLogic() 
    {
        

    }
    public virtual void DoPhysicsLogic()
    {
        float movementSpeed = 10f;

        //Debug.Log(playerTransform);
       // Debug.Log(enemy.gameObject.transform.position);
        Vector2 dir = (new Vector2(1, 1)).normalized;

        // Face the player
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        enemy.gameObject.transform.rotation = Quaternion.Euler(0, 0, angle);

        // Move towards the player slowly
        //Rigidbody2D go_rb = enemy.gameObject.GetComponent<Rigidbody2D>();
        //if (go_rb.velocity.magnitude < (dir * movementSpeed).magnitude)
        //    go_rb.AddForce(dir * movementSpeed);

        enemy.MoveEnemy(dir * movementSpeed); // Move enemy towards vector
    }
    public virtual void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType) { }
    public virtual void ResetValue() { }
}
