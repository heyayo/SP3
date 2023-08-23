using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAggroCheck : MonoBehaviour
{
    public GameObject player { get; set; }
    private Enemy _enemy;

    private void Awake()
    {
        player = PlayerManager.Instance.gameObject;
        _enemy = GetComponentInParent<Enemy>(); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _enemy.SetAggroStatus(true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
         _enemy.SetAggroStatus(false);
    }
}
