using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BOC States/E_Attack")]
public class BOCEAttack : BossState
{
    [SerializeField]
    private GameObject brainOfCthulhuClone;

    private int attackTimer;
    private int attackCount;
    private int previousRng;
    private float opacity;

    override public void EnterState()
    {
        attackTimer = 50;
        attackCount = 0;
        opacity = 0.6f + (mortality.Health / mortality.__HealthMax) / 2;
    }

    override public bool DoState()
    {
        // Charge at player
        if (attackTimer > 0)
        {
            attackTimer--;

            FacePlayer();
            rb.AddForce(dir * 10f);

            return false;
        }

        // Finish this state
        if (attackCount >= 3)
            return true;

        // Fading to invisible
        if (sr.color.a > 0)
        {
            sr.color -= new Color(0, 0, 0, 0.025f);
            return false;
        }

        // Teleport
        transform.position = TeleportPosition(RandomTeleportDirection());
        sr.color = new Color(255, 255, 255, opacity);
        rb.velocity = Vector2.zero;

        // Top left clone
        if (previousRng != 1)
        {
            GameObject clone = Instantiate(brainOfCthulhuClone, new Vector3(TeleportPosition(new Vector2(-1, 1)).x,
                TeleportPosition(new Vector2(-1, 1)).y, 0), Quaternion.identity);
            // Changing it's visibility based on remaining health
            clone.GetComponent<BOCCloneFSM>().opacity = opacity - 1 / (mortality.__HealthMax / mortality.Health);
        }

        // Top Right clone
        if (previousRng != 2)
        {
            GameObject clone = Instantiate(brainOfCthulhuClone, new Vector3(TeleportPosition(new Vector2(1, 1)).x,
                TeleportPosition(new Vector2(1, 1)).y, 0), Quaternion.identity);
            // Changing it's visibility based on remaining health
            clone.GetComponent<BOCCloneFSM>().opacity = opacity - 1 / (mortality.__HealthMax / mortality.Health);
        }

        // Bottom left clone
        if (previousRng != 3)
        {
            GameObject clone = Instantiate(brainOfCthulhuClone, new Vector3(TeleportPosition(new Vector2(-1, -1)).x,
                TeleportPosition(new Vector2(-1, -1)).y, 0), Quaternion.identity);
            // Changing it's visibility based on remaining health
            clone.GetComponent<BOCCloneFSM>().opacity = opacity - 1 / (mortality.__HealthMax / mortality.Health);
        }

        // Bottom right clone
        if (previousRng != 4)
        {
            GameObject clone = Instantiate(brainOfCthulhuClone, new Vector3(TeleportPosition(new Vector2(1, -1)).x,
                TeleportPosition(new Vector2(1, -1)).y, 0), Quaternion.identity);
            // Changing it's visibility based on remaining health
            clone.GetComponent<BOCCloneFSM>().opacity = opacity - 1 / (mortality.__HealthMax / mortality.Health);
        }

        attackTimer = 200;
        attackCount++;

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
