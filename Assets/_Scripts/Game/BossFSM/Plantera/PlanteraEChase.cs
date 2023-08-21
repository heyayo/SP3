using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Plantera States/E_Chase")]
public class PlanteraEChase : BossState
{
    [SerializeField]
    private GameObject spore;

    private int chaseTimer;
    private int shootTimer;

    override public void EnterState()
    {
        chaseTimer = 450;
        shootTimer = 250;
    }

    override public bool DoState()
    {
        chaseTimer--;
        shootTimer--;

        // Move toward player
        FacePlayer();
        rb.AddForce(dir * 12f);

        if (shootTimer <= 0)
        {
            Instantiate(spore, transform.position + new Vector3(dir.x, dir.y, 0) * 2f, Quaternion.identity);
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
