using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Queen Bee States/Shoot")]
public class QueenBeeShoot : BossState
{
    [SerializeField]
    private GameObject Bullet;

   

    private int ShootingTimer;
    private int BulletTimer;

    override public void EnterState()
    {
        ShootingTimer = 600;
        BulletTimer = 30;
    }

    override public bool DoState()
    {
        ShootingTimer--;

        BulletTimer--;

        FacePlayer();
        Vector2 moveDir = ((playerTransform.position + new Vector3(0, 6, 0)) - transform.position).normalized;
        rb.AddForce(moveDir * 5);

        if (BulletTimer <= 0)
        {
            ShootBullet();
            BulletTimer = 150;
        }

        if (ShootingTimer <= 0)
            return true;

        return false;
    }

    private void ShootBullet()
    {
        if (Bullet != null)
        {
            GameObject bullet = Instantiate(Bullet, (transform.position - new Vector3(0, 2, 0)), Quaternion.identity);
            Vector2 shootDirection = (playerTransform.position - transform.position).normalized;

            // Calculate the rotation to face the shoot direction
            float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);

            // Apply the rotation to the bullet
            bullet.transform.rotation = rotation;

            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

            if (bulletRb)
            {
                bulletRb.AddForce(shootDirection * 15, ForceMode2D.Impulse);
            }
        }
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
