using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BOC States/Chase")]
public class BOCChase : BossState
{
    private int chaseTimer;

    override public void EnterState()
    {
        chaseTimer = 150;
    }

    override public bool DoState()
    {
        chaseTimer--;

        FacePlayer();
        rb.AddForce(dir * 3f);

        if (chaseTimer <= 0)
            return true;

        return false;
    }

    override public void ExitState()
    {

    }

    override protected void FacePlayer()
    {
        dir = (playerTransform.position - transform.position).normalized;
    }
}
