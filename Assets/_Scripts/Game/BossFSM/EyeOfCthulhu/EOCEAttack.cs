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

        // Based on hp
        if (mortality.Health / mortality.__HealthMax > 0.35f)
            attackCount = 3;
        else if (mortality.Health / mortality.__HealthMax > 0.2f)
            attackCount = 4;
        else if (mortality.Health / mortality.__HealthMax > 0.05f)
            attackCount = 5;
        else
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
            rb.AddForce(dir * 1600);

            // Based on hp
            if (mortality.Health / mortality.__HealthMax > 0.35f)
                attackTimer = 45;
            else if (mortality.Health / mortality.__HealthMax > 0.2f)
                attackTimer = 40;
            else if (mortality.Health / mortality.__HealthMax > 0.05f)
                attackTimer = 30;
            else
                attackTimer = 25;

            attackCount--;
            SoundManager.Instance.PlaySound(6);
        }

        if (attackCount <= 0)
            return true;

        return false;
    }

    override public void ExitState()
    {

    }
}
