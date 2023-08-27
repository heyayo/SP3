using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShenronSummoner : MonoBehaviour
{
    public static ShenronSummoner Instance { get; private set; }
    
    private Image _screen;
    private Animator _animator;

    private void Awake()
    {
        _screen = GetComponent<Image>();
        _animator = GetComponent<Animator>();

        if (Instance != null)
        {
            Debug.LogError("Multiple Shenron Summmoners In Scene");
            Debug.Break();
        }
        Instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        DisableScreen();
    }

    private void WishFulfilled()
    {
        DisableScreen();
    }
    
    public void DisableScreen()
    {
        _screen.enabled = false;
        _animator.enabled = false;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    public void EnableScreen()
    {
        _screen.enabled = true;
        _animator.enabled = true;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
        _animator.Play("ShenronSummon");
    }

    public void InfiniteEnergyWish()
    {
        PlayerManager.Instance.MortalityScript.ApplyAffliction(new InfiniteEnergy(120,PlayerManager.Instance.MortalityScript));
        WishFulfilled();
    }
}

public class InfiniteEnergy : Affliction
{
    public InfiniteEnergy(float pLifetime, Mortality pTarget) : base(pLifetime, pTarget)
    {
        icon = Resources.Load<Sprite>("Sprites/infiniteenergy");
    }

    public override void Begin()
    {
    }

    public override void Update()
    {
        target.ActiveEnergy = target.__ActiveEnergyMax;
        target.StoredEnergy = target.__StoredEnergyMax;
    }

    public override void End()
    {
    }
}
