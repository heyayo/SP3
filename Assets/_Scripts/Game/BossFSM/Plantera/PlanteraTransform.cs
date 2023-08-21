using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Plantera States/Transform")]
public class PlanteraTransform : BossState
{
    override public bool DoState()
    {
        HitboxDamage();

        animator.SetBool("Enraged", true);
        return true;
    }

    public override bool isReadyToTransform()
    {
        return mortality.Health <= mortality.__HealthMax / 2;
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
