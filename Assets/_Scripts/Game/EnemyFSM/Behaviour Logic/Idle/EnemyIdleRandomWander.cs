using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Idle - Random Patrol", menuName = "Enemy Logic/Idle Logic/Random patrol")]
public class EnemyIdleRandomWander : EnemyIdleSOBase
{
    [SerializeField] private float randomMovementRange = 5f;
    [SerializeField] private float randomMovementSpeed = 2f;

    private Vector3 _targetPos;
    private Vector3 _direction;
    public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        _targetPos = GetRandomPointInCircle();
        enemy.enemyAnimator.SetBool("isWalking", true); // Play walk animation when idle state is initiated
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
        enemy.enemyAnimator.SetBool("isWalking", false);
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();
 
        _direction = (_targetPos - enemy.transform.position).normalized; // Calculate the unit vector from player to enemy
        enemy.MoveEnemy(_direction * randomMovementSpeed);
        if ((enemy.transform.position - _targetPos).sqrMagnitude < 0.01f)
        {
            _targetPos = GetRandomPointInCircle();
        }
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

    private Vector3 GetRandomPointInCircle()
    {
        return enemy.transform.position + (Vector3)UnityEngine.Random.insideUnitCircle * randomMovementRange;
    }
}
