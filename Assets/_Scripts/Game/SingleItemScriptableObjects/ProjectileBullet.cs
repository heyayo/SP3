using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D bulletPrefab;
    [SerializeField] private EnemyAttackSOBase enemyAttackBase;
    [SerializeField] private Enemy enemy;
    private float _bulletSpeed;
    // Start is called before the first frame update
    void Start()
    {
        bulletPrefab = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody2D bullet = GameObject.Instantiate(bulletPrefab, enemyAttackBase.playerTransform.position, Quaternion.identity);
        Vector2 dir = (enemyAttackBase.playerTransform.position - enemy.transform.position).normalized;
        bullet.velocity = dir * _bulletSpeed;

    }
}
