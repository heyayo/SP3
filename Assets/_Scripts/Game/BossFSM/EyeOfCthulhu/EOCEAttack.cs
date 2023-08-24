using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EOC States/E_Attack")]
public class EOCEAttack : BossState
{
    private int attackTimer;
    private int attackCount;

    override public void EnterState()
    {
        attackTimer = 50;
        attackCount = 6;
    }

    override public bool DoState()
    {
        attackTimer--;

        if (attackTimer <= 0)
        {
            // Dash
            FacePlayer();
            rb.velocity = Vector2.zero;
            rb.AddForce(dir * 1200);
            attackTimer = 40;
            attackCount--;
            SoundManager.Instance.PlaySound(0);
        }

        if (attackCount <= 0)
            return true;

        return false;
    }

    override public void ExitState()
    {

    }
}
