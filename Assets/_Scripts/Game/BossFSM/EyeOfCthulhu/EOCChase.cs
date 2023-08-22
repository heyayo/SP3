using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EOC States/Chase")]
public class EOCChase : BossState
{
    private int chaseTimer;
    private int dashTimer;

    override public void EnterState()
    {
        chaseTimer = 320;
        dashTimer = 75;
    }

    override public bool DoState()
    {
        chaseTimer--;
        dashTimer--;

        // Move toward player
        if (dashTimer <= 0)
        {
            FacePlayer();
            rb.AddForce(dir * 200f);
            dashTimer = 70;
        }

        if (chaseTimer <= 0)
            return true;

        return false;
    }

    override public void ExitState()
    {

    }
}
