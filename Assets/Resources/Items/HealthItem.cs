using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Items/Consumables/HealthItem")]
public class HealthItem : Item
{
    [Header("Health Item")]
    [SerializeField]
    private float healAmount;

    [SerializeField]
    private int consumeSoundIndex = 10;

    public override void Use()
    {
        Mortality mortality = PlayerManager.Instance.GetComponent<Mortality>();
        mortality.Health += healAmount;

        if (mortality.Health > mortality.__HealthMax)
            mortality.Health = mortality.__HealthMax;

        SoundManager.Instance.PlaySound(consumeSoundIndex);
        consumed.Invoke();
    }
}
