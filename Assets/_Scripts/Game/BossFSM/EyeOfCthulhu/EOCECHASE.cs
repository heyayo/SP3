using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EOC States/E_Chase")]
public class EOCEChase : BossState
{
    private int chaseTimer;
    private int dashTimer;

    override public void EnterState()
    {
        chaseTimer = 350;
        dashTimer = 120;
    }

    override public bool DoState()
    {
        chaseTimer--;
        dashTimer--;

        // Move toward player
        if (dashTimer <= 0)
        {
            rb.AddForce(dir * 250f);
            dashTimer = 60;
        }
        // Wait awhile before continuing to pursue player
        else if (dashTimer < 45)
        {
            FacePlayer();
            rb.AddForce(dir * 10f);
        }

        if (chaseTimer <= 0)
            return true;

        return false;
    }

    override public void ExitState()
    {

    }
}
