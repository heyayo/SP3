using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Plantera States/Chase")]
public class PlanteraChase : BossState
{
    [SerializeField]
    private GameObject bulletSeed;

    private int chaseTimer;
    private int shootTimer;

    override public void EnterState()
    {
        chaseTimer = 600;
        shootTimer = 70;
    }

    override public bool DoState()
    {
        chaseTimer--;
        shootTimer--;
        HitboxDamage();

        // Move toward player
        FacePlayer();
        rb.AddForce(dir * 2f);

        if (shootTimer <= 0)
        {
            GameObject bullet = Instantiate(bulletSeed, transform.position + new Vector3(dir.x, dir.y, 0) * 3, transform.rotation);
            bullet.GetComponent<Rigidbody2D>().AddForce(dir * 300f);
            shootTimer = 70;
        }

        if (chaseTimer <= 0)
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
