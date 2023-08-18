using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MinoChase", menuName = "Enemy Logic/Chase Logic/Mino Chase")]
public class MinotaurChaseSOChild : EnemyChaseSOBase
{
    [SerializeField] private float _movementSpeed = 5f;
    private Vector2 _moveDir;
    public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        Debug.Log("ENTERED MINOTAUR CHASE SO CHILD");
        minotaur = gameObject.GetComponent<Minotaur>();
    }   

    public override void DoExitLogic()
    {
        base.DoExitLogic();
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();
        if (!minotaur.isAggroed)
        {
            minotaur.stateMachine.ChangeState(minotaur.healingState);
        }
    }

    public override void DoPhysicsLogic()
    {
        base.DoPhysicsLogic();
        _moveDir = (playerTransform.position - enemy.transform.position).normalized; // Find vector between enemy and player
        enemy.MoveEnemy(_moveDir * _movementSpeed); // Move enemy towards vector
    }

    public override void Init(GameObject gameObject, Enemy enemy)
    {
        base.Init(gameObject, enemy);
    }

    public override void InitMinotaur(Minotaur minotaur)
    {
        base.InitMinotaur(minotaur);
    }

    public override void ResetValue()
    {
        base.ResetValue();
    }
}
