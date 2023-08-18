using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Healing", menuName = "Enemy Logic/Healing Logic/Enemy Heal - Post Damage")]
public class EnemyHeal : EnemyHealSOBase
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
        Debug.Log("YESSIRRRRR");
    }

    public override void DoPhysicsLogic()
    {
        base.DoPhysicsLogic();
    }

    public override void Init(GameObject gameObject, Minotaur minotaur)
    {
        base.Init(gameObject, minotaur);
    }

    public override void ResetValue()
    {
        base.ResetValue();
    }
}
