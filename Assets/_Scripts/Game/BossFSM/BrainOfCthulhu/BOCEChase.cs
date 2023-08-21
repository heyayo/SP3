using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BOC States/E_Chase")]
public class BOCEChase : BossState
{
    private int chaseTimer;

    override public void EnterState()
    {
        chaseTimer = 120;
    }

    override public bool DoState()
    {
        chaseTimer--;
        HitboxDamage();

        FacePlayer();
        rb.AddForce(dir * 10f);

        if (chaseTimer <= 0)
            return true;

        return false;
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

    override protected void FacePlayer()
    {
        dir = (playerTransform.position - transform.position).normalized;
    }
}
