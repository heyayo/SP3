using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minotaur : Enemy
{
    [field:SerializeField] public MinoHealState healingState { get; set; }
    [SerializeField] private EnemyHealSOBase EnemyHealingState;
    public EnemyHealSOBase enemyHealingStateInstance { get; set; }

    private void Awake()
    {
        base.Awake(); 
        enemyHealingStateInstance = Instantiate(EnemyHealingState);
    }
    // Start is called before the first frame update
    void Start()
    {   
        base.Start();
        enemyHealingStateInstance.Init(gameObject, this);
    }
}
