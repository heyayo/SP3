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

        spawnTimer--;

        FacePlayer();
        Vector2 moveDir = ((playerTransform.position + new Vector3(0, 8, 0)) - transform.position).normalized;
        rb.AddForce(moveDir * 5);

        if (spawnTimer <= 0)
        {
            Instantiate(servantOfCthulhu, transform.position + new Vector3(dir.x, dir.y, 0) * 2.5f, Quaternion.identity);
            spawnTimer = 150;
        }

        if (summonTimer <= 0)
            return true;

        return false;
    }

    override public void ExitState()
    {

    }
}
