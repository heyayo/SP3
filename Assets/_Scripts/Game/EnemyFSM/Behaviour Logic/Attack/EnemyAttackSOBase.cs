using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackSOBase : ScriptableObject
{
    protected Enemy enemy;
    protected Transform transform;
    protected GameObject gameObject;

    protected Transform playerTransform;

    public virtual void Init(GameObject gameObject, Enemy enemy)
    {
        Debug.Log("INIT GAMEOBJECT" + this.gameObject);
        this.gameObject = gameObject;
        transform = gameObject.transform;
        this.enemy = enemy;

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public virtual void DoEnterLogic() { Debug.Log("Entered Attack State"); }
    public virtual void DoExitLogic() { ResetValue(); }
    public virtual void DoFrameUpdateLogic() {}
    public virtual void DoPhysicsLogic() { }
    public virtual void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType) { }
    public virtual void ResetValue() { }
}