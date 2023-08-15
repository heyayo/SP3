using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Attack-Straight-Single-Projectile", menuName = "Enemy Logic/Attack Logic/Straight Single Projectile")]
public class EnemySingleStraightProjectile : EnemyAttackSOBase
{
    [SerializeField] private Rigidbody2D bulletPrefab;
    [SerializeField] private float _timeBetweenShots = 1.5f;
    [SerializeField] private float _timeTillExit = 2f;
    [SerializeField] private float _distanceToCountExit = 3f;
    [SerializeField] private float _bulletSpeed = 10f;

    private float _timer;
    private float _exitTimer;
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
        enemy.MoveEnemy(Vector2.zero);
        if (_timer > _timeBetweenShots)
        {
            _timer = 0f;
            Vector2 dir = (playerTransform.position - enemy.transform.position).normalized;
            Rigidbody2D bullet = GameObject.Instantiate(bulletPrefab, enemy.transform.position, Quaternion.identity);
            bullet.velocity = dir * _bulletSpeed;
            enemy.enemyAnimator.SetBool("isAttacking", true);
        }
        else
        {
            enemy.enemyAnimator.SetBool("isAttacking", false);
        }
        if (Vector2.Distance(playerTransform.position, enemy.transform.position) > _distanceToCountExit)
        {
            Debug.Log("Player is outside attacking range");
            _exitTimer += Time.deltaTime;
            if (_exitTimer > _timeTillExit) // Check if player is outside striking range for a certain time
            {
                enemy.stateMachine.ChangeState(enemy.chaseState);
            }
        }
        else
        {
            _exitTimer = 0f;
        }
        _timer += Time.deltaTime;
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
