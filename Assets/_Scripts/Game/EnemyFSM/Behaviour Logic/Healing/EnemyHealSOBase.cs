using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealSOBase : ScriptableObject
{
    protected Enemy enemy;
    protected GameObject gameObject;
    [HideInInspector] protected Transform transform;

    public Transform playerTransform;

    public virtual void Init(GameObject gameObject, Enemy enemy)
    {
        this.gameObject = gameObject;
        transform = gameObject.transform;
        this.enemy = enemy;

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public virtual void DoEnterLogic() { Debug.Log("Entered Healing State"); }
    public virtual void DoExitLogic() { ResetValue(); }
    public virtual void DoFrameUpdateLogic()
    {
        //if (!enemy.isAggroed)
        //{
        //    Debug.Log("Entered Chase State");
        //    enemy.stateMachine.ChangeState(enemy.idleState);
        //}
    }
    public virtual void DoPhysicsLogic() { }
    public virtual void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType) { }
    public virtual void ResetValue() { }
}
