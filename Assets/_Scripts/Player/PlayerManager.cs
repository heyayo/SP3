using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Mortality))]
[RequireComponent(typeof(Interactor))]
[RequireComponent(typeof(Movement))]
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    
    private Mortality _mortality;
    private Interactor _interactor;
    private Movement _movement;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple Players Detected, MULTIPLAYER NOT ACTIVE");
            Debug.Break();
        }
        Instance = this;
        
        _mortality = GetComponent<Mortality>();
        _interactor = GetComponent<Interactor>();
        _movement = GetComponent<Movement>();

        gameObject.tag = "Player";
    }

    public void FreezePlayer()
    {
        _mortality.enabled = false;
        _movement.enabled = false;
        _interactor.enabled = false;
    }

    public void UnFreezePlayer()
    {
        _mortality.enabled = true;
        _movement.enabled = true;
        _interactor.enabled = true;
    }
}
