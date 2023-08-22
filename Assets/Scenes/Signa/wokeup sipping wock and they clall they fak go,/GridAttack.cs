using UnityEngine;
using System.Collections;

public class GridAttack : MonoBehaviour
{
    public GameObject gridAttackAnimationPrefab; // Reference to the grid attack animation prefab
    public float attackDuration = 5f;
    public float attackInterval = 1f;

    private bool isAttacking = false;
    private GameObject gridAttackAnimationInstance;
    private Transform playerTransform;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        StartGridAttack();
    }

    private void StartGridAttack()
    {
        if (gridAttackAnimationPrefab != null)
        {
            gridAttackAnimationInstance = Instantiate(gridAttackAnimationPrefab, playerTransform.position, Quaternion.identity);
            
        }
    }

    
}