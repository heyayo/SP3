using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EOC States/Summon")]
public class EOCSummon : BossState
{
    [SerializeField]
    private GameObject servantOfCthulhu;

    private int summonTimer;
    private int spawnTimer;

    override public void EnterState()
    {
        summonTimer = 1200;
        spawnTimer = 400;
    }

    override public bool DoState()
    {
        summonTimer--;
        HitboxDamage();

        spawnTimer--;

        FacePlayer();
        Vector2 moveDir = ((playerTransform.position + new Vector3(0, 6, 0)) - transform.position).normalized;
        rb.AddForce(moveDir * 5);

        if (spawnTimer <= 0)
        {
            Instantiate(servantOfCthulhu, transform.position + new Vector3(dir.x, dir.y, 0) * 2f, Quaternion.identity);
            spawnTimer = 150;
        }

        if (summonTimer <= 0)
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
