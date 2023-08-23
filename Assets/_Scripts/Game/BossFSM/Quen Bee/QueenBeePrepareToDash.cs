using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Queen Bee States/PrepareToDash")]
public class QueenBeePrepareToDash : BossState
{
    private float PrepareTimer;

    override public void EnterState()
    {
        PrepareTimer = 100;

        rb.velocity = Vector2.zero;
    }

    override public bool DoState()
    {
        PrepareTimer--;

        FacePlayer();

        Vector3 playerPosition = playerTransform.position;
        Vector3 desiredPosition;

        // Calculate the desired position based on proximity to player's left or right side
        if (transform.position.x < playerPosition.x)
        {
            desiredPosition = new Vector3(playerPosition.x - 5, transform.position.y, transform.position.z);
        }
        else
        {
            desiredPosition = new Vector3(playerPosition.x + 5, transform.position.y, transform.position.z);
        }

        Vector3 moveDir = (desiredPosition - transform.position).normalized;

        rb.AddForce(moveDir * 5);

        if ((desiredPosition - transform.position).magnitude > 3)
        {
           
                rb.velocity = Vector2.zero;
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