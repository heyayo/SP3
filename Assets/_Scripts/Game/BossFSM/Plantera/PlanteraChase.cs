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

        // Move toward player
        FacePlayer();
        rb.AddForce(dir * 2f);

        if (shootTimer <= 0)
        {
            GameObject bullet = Instantiate(bulletSeed, transform.position + new Vector3(dir.x, dir.y, 0) * 3, transform.rotation);
            bullet.GetComponent<Rigidbody2D>().AddForce(dir * 300f);
            shootTimer = 70;
            SoundManager.Instance.PlaySound(5);
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
}
