using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EOC States/Attack")]
public class EOCAttack : BossState
{
    private int attackTimer;
    private int attackCount;

    override public void EnterState()
    {
        attackTimer = 50;
        attackCount = 3;
    }

    override public bool DoState()
    {
        attackTimer--;

        if (attackTimer <= 0)
        {
            // Dash
            FacePlayer();
            rb.AddForce(dir * 800);
            attackTimer = 120;
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
