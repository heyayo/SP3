using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Chase - Run Away", menuName = "Enemy Logic/Chase Logic/Run Away")]
public class EnemyChaseRunAway : EnemyChaseSOBase
{
    [SerializeField] private float fleeSpeed = 10f;

    public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic(); 

        Vector2 runDir = -(playerTransform.position - transform.position).normalized;
        enemy.MoveEnemy(runDir * fleeSpeed);
    }

    public override void DoPhysicsLogic()
    {
        base.DoPhysicsLogic();
    }

    public override void Init(GameObject gameObject, Enemy enemy)
    {
        base.Init(gameObject, enemy);
    }

    public override void ResetValue()
    {
        base.ResetValue();
    }
}
