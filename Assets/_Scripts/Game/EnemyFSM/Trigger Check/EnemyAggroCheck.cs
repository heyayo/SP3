using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAggroCheck : MonoBehaviour
{
    public GameObject player { get; set; }
    private Enemy _enemy;

    private void Awake()
    {
        _enemy = GetComponentInParent<Enemy>(); 
    }
    private void Start()
    {
        player = PlayerManager.Instance.gameObject;
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
