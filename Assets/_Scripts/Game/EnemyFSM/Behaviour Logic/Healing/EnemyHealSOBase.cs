using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealSOBase : ScriptableObject
{
    protected Minotaur minotaur; 
    protected GameObject gameObject;
    [HideInInspector] protected Transform transform;

    public Transform playerTransform;

    public virtual void Init(GameObject gameObject, Minotaur minotaur)
    {
        this.gameObject = gameObject;
        transform = gameObject.transform;
        this.minotaur = minotaur;

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public virtual void DoEnterLogic() { Debug.Log("Entered Healing State"); }
    public virtual void DoExitLogic() { ResetValue(); }
    public virtual void DoFrameUpdateLogic()
    {

    }
    public virtual void DoPhysicsLogic() { }
    public virtual void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType) { }
    public virtual void ResetValue() { }
    public Minotaur ReturnMinotaur() { return minotaur; }
}
