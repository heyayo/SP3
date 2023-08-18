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
        attackCount = 6;
    }

    override public bool DoState()
    {
        attackTimer--;
        HitboxDamage();

        if (attackTimer <= 0)
        {
            // Dash
            FacePlayer();
            rb.velocity = Vector2.zero;
            rb.AddForce(dir * 1200);
            attackTimer = 40;
            attackCount--;
        }

        if (attackCount <= 0)
            return true;

        return false;
    }

    override public void ExitState()
    {

    }

    private void HitboxDamage()
    {
        // Hitbox stats
        Vector2 hitboxPos = transform.position - new Vector3(0, 0.5f, 0);
        float hitboxRadius = 1.2f;
        Collider2D col = Physics2D.OverlapCircle(hitboxPos, hitboxRadius, playerLayer);
        if (col != null)
            playerMortality.ApplyHealthDamage(10);
    }
}
