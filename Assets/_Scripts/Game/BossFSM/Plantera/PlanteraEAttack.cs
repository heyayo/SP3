using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Plantera States/E_Attack")]
public class PlanteraEAttack : BossState
{
    private int attackTimer;
    private int attackCount;

    override public void EnterState()
    {
        attackTimer = 140;
        attackCount = 4;
    }

    override public bool DoState()
    {
        attackTimer--;
        HitboxDamage();

        // Move toward player
        FacePlayer();
        rb.AddForce(dir * 12f);

        if (attackTimer <= 0)
        {
            // Dash
            rb.AddForce(dir * 600);

            // Reset
            attackTimer = 140;
            attackCount--;
        }

        if (attackCount <= 0)
            return true;

        return false;
    }

    override public void ExitState()
    {

    }

    override protected void FacePlayer()
    {
        dir = (playerTransform.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
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
