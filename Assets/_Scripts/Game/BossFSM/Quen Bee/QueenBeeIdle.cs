using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Queen Bee States/idle")]
public class QueenBeeIdle : BossState
{
    private float IdleTimer;

    override public void EnterState()
    {
        IdleTimer = 50;

        rb.velocity = Vector2.zero;
    }

    override public bool DoState()
    {
        IdleTimer--;

        FacePlayer();

        Vector3 targetPositionAbove = playerTransform.position + new Vector3(0, 5, 0);
        Vector3 targetPositionBelow = playerTransform.position + new Vector3(0, -5, 0);

        Vector3 moveDir = (targetPositionAbove - transform.position).normalized;

        // Calculate the distance between the object and the target position
        float distanceToTarget = Vector3.Distance(transform.position, targetPositionAbove);

        // Check if the object is within a threshold distance
        if (distanceToTarget < 1f)
        {
            moveDir = (targetPositionBelow - transform.position).normalized;
        }

        rb.AddForce(moveDir * 1.2f);

        if (IdleTimer <= 0)
            return true;

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
