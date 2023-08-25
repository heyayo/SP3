using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EOC States/E_Chase")]
public class EOCEChase : BossState
{
    private int chaseTimer;

    override public void EnterState()
    {
        if (mortality.Health / mortality.__HealthMax > 0.35f)
            chaseTimer = 350;
        else if (mortality.Health / mortality.__HealthMax > 0.2f)
            chaseTimer = 275;
        else if (mortality.Health / mortality.__HealthMax > 0.05f)
            chaseTimer = 200;
        else
            chaseTimer = 0;
    }

    override public bool DoState()
    {
        chaseTimer--;

        FacePlayer();
        rb.AddForce(dir * 10f);

        if (chaseTimer <= 0)
            return true;

        return false;
    }

    override public void ExitState()
    {

    }
}
