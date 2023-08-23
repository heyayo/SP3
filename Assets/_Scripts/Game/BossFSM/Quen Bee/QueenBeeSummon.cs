using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Queen Bee States/Summon")]
public class QueenBeeSummon : BossState
{
    [SerializeField]
    private GameObject servantOfCthulhu;

    private int summonTimer;
    private int spawnTimer;

    override public void EnterState()
    {
        summonTimer = 500;
        spawnTimer = 100;
    }

    override public bool DoState()
    {
        summonTimer--;

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
    override protected void FacePlayer()
    {
        dir = (playerTransform.position - transform.position).normalized;
        if (dir.x < 0)
        {
            sr.flipX = true;
        }

        if (dir.x > 0)
        {
            sr.flipX = false;
        }


    }
}
