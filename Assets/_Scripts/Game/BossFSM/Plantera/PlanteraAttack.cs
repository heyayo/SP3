using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Plantera States/Attack")]
public class PlanteraAttack : BossState
{
    private int attackTimer;
    private int attackCount;

    override public void EnterState()
    {
        attackTimer = 120;
        attackCount = 3;
    }

    override public bool DoState()
    {
        attackTimer--;
        FacePlayer();
        HitboxDamage();

        if (attackTimer <= 0)
        {
            // Dash
            rb.velocity = Vector2.zero;
            rb.AddForce(dir * 700);
            attackTimer = 120;
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
