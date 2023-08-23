using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStrikingDistanceCheck : MonoBehaviour
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
<<<<<<< HEAD
        _enemy.SetStrikingDistance(true);
=======
        if (collision.gameObject == player)
        {
            Debug.Log(_enemy);
            _enemy.SetStrikingDistance(true);
        }
>>>>>>> 4cfa3bfae696f1fa0b9409ab793a230af9ba60c0
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        _enemy.SetStrikingDistance(false);
    }
}
