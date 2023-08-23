using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BOC States/Teleport")]
public class BOCTeleport : BossState
{
    private int timeInvisible;
    private bool teleported;
    private int previousRng;
    private float opacity;

    override public void EnterState()
    {
        timeInvisible = 60;
        teleported = false;

        // Opacity is reduced only when the brain is in enraged mode
        if (mortality.Health <= mortality.__HealthMax / 2)
            opacity = 0.6f + (mortality.Health / mortality.__HealthMax) / 2;
        else
            opacity = 1f;
    }

    override public bool DoState()
    {
        // Constantly mvoing to player
        FacePlayer();
        rb.AddForce(dir * 3f);

        // Fading to invisible
        if (sr.color.a > 0 && !teleported)
        {
            sr.color -= new Color(0, 0, 0, 0.025f);
        }
        // Teleporting
        else if (sr.color.a <= 0 && !teleported)
        {
            transform.position = TeleportPosition(RandomTeleportDirection());
            rb.velocity = Vector2.zero;
            teleported = true;
        }
        // Stay invisible for awhile
        else if (timeInvisible > 0)
        {
            timeInvisible--;
        }
        // Becoming visible again
        else if (sr.color.a < opacity)
        {
            timeInvisible--;
            sr.color += new Color(0, 0, 0, 0.025f);
        }
        // Done with teleport
        else
        {
            return true;
        }

        return false;
    }

    override public void ExitState()
    {

    }

    private Vector2 TeleportPosition(Vector2 direction)
    {
        Vector2 tpDirection = direction.normalized;

        return playerTransform.position + new Vector3(tpDirection.x, tpDirection.y, 0) * 15f;
    }

    private Vector2 RandomTeleportDirection()
    {
        int rng;
        rng = Random.Range(1, 5);

        // Don't tp to the same position as before
        while (rng == previousRng)
            rng = Random.Range(1, 5);

        previousRng = rng;

        if (rng == 1)
            return new Vector2(-1, 1);  // Top left
        else if (rng == 2)
            return new Vector2(1, 1);  // Top right
        else if (rng == 3)
            return new Vector2(-1, -1);  // Bottom left
        else
            return new Vector2(1, -1);  // Bottom right
    }

    override protected void FacePlayer()
    {
        dir = (playerTransform.position - transform.position).normalized;
    }
}
