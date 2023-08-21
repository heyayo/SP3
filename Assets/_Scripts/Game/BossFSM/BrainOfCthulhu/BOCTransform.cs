using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BOC States/Transform")]
public class BOCTransform : BossState
{
    private float opacity;

    override public void EnterState()
    {
        opacity = 0.7f;
    }

    override public bool DoState()
    {
        HitboxDamage();

        sr.color = new Color(255, 255, 255, opacity);  // Set to opaque
        animator.SetBool("Enraged", true);
        return true;
    }

    public override bool isReadyToTransform()
    {
        return mortality.Health <= mortality.__HealthMax / 2;
    }

    override public void ExitState()
    {

    }

    private void HitboxDamage()
    {
        // Hitbox stats
        Vector2 hitboxPos = transform.position - new Vector3(0, 0, 0);
        float hitboxRadius = 3f;
        Collider2D col = Physics2D.OverlapCircle(hitboxPos, hitboxRadius, playerLayer);
        if (col != null)
            playerMortality.ApplyHealthDamage(10);
    }
}
