using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState
{
    protected EnemyStateMachine _stateMachine;

    public EnemyState(EnemyStateMachine stateMachine)
    {
        this._stateMachine = stateMachine;
    }

    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void FrameUpdate();
    public abstract void PhysicsUpdate();
    public abstract void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType);
}
 