using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Attack-Straight-Single-Projectile", menuName = "Enemy Logic/Attack Logic/Straight Single Projectile")]
public class EnemySingleStraightProjectile : EnemyAttackSOBase
{
    [SerializeField] private Rigidbody2D bulletPrefab;
    [SerializeField] private float _timeBetweenShots = 2f;
    [SerializeField] private float _timeTillExit = 3f;
    [SerializeField] private float _distanceToCountExit = 4f;
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
        bulletPrefab.gameObject.SetActive(false);
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
        bulletPrefab.gameObject.SetActive(false);
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();
        bulletPrefab.gameObject.SetActive(true);
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
    }
    public override void DoPhysicsLogic()
    {
        base.DoPhysicsLogic();
        enemy.MoveEnemy(Vector2.zero);
        if (_timer > _timeBetweenShots)
        {
            _timer = 0f;
            Rigidbody2D bullet = GameObject.Instantiate(bulletPrefab, enemy.transform.position, Quaternion.identity);
            Vector2 dir = (playerTransform.position - enemy.transform.position).normalized;
            bullet.velocity = dir * _bulletSpeed;
        }
        _timer += Time.deltaTime;
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
