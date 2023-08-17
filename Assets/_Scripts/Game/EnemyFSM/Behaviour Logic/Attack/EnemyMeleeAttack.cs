using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
[CreateAssetMenu(fileName = "Melee-Attack", menuName = "Enemy Logic/Attack Logic/Standard Melee Attack")]
public class EnemyMeleeAttack : EnemyAttackSOBase
{
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
        CHENNN();
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

    private async Task CHENNN()
    {
        enemy.enemyAnimator.SetBool("isAttacking", true);
        await Task.Delay(1000);
        enemy.enemyAnimator.SetBool("isAttacking", false);
    }
}
