using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minotaur : Enemy
{
    // CREATING ADDITIONAL STATES FOR SEPERATE ENEMIES
    #region Additional State Machine Variables
    [field: SerializeField] public EnemyHealingState healingState { get; set; }
    #endregion
    #region Additional SO Variables
    [SerializeField] private EnemyHealSOBase EnemyHealingState;
    public EnemyHealSOBase enemyHealingStateInstance { get; set; }
    #endregion

    private void Awake()
    {
        InitAwake(); 
        enemyHealingStateInstance = Instantiate(EnemyHealingState);
        healingState = new EnemyHealingState(this, stateMachine);
    }
    // Start is called before the first frame update
    void Start()
    {   
        InitStart();
        enemyHealingStateInstance.Init(gameObject, this);
    }
}
