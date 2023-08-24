using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Queen Bee States/Attack")]
public class QueenBeeAttack : BossState
{
    private int attackTimer;
    private int attackCount;

    override public void EnterState()
    {
        attackTimer = 50;
        attackCount = 2;
    }

    override public bool DoState()
    {
        attackTimer--;

        if (attackTimer <= 0)
        {
            animator.SetBool("Dash", true);
            FacePlayer();
            rb.AddForce(dir * 1200);
            attackTimer = 120;
            attackCount--;
            SoundManager.Instance.PlaySound(0);
;       }

        if (attackCount <= 0)
        {
            animator.SetBool("Dash", false);
            return true;
        }

            return false;
    }

    override public void ExitState()
    {

    }

    override protected void FacePlayer()
    {
        dir = (playerTransform.position - transform.position).normalized;
        if (dir.x < 0)
        {
            sr.flipX = true;
        }

        if (dir.x > 0)
        {
            sr.flipX = false;
        }


    }
}
