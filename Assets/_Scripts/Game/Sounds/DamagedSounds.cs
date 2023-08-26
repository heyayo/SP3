using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedSounds : MonoBehaviour
{
    [SerializeField]
    private int hitSoundIndex = 1;

    [SerializeField]
    private int deathSoundIndex = 3;

    void Start()
    {
        Mortality mortality = GetComponent<Mortality>();

        mortality.onHealthAdjust.AddListener(HitSound);
        mortality.onHealthZero.AddListener(DeathSound);
    }

    private void HitSound()
    {
        SoundManager.Instance.PlaySound(hitSoundIndex);
    }

    private void DeathSound()
    {
        SoundManager.Instance.PlaySound(deathSoundIndex);
    }
}
