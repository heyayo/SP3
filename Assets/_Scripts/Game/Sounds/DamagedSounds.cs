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
        GetComponent<Damagable>().hit.AddListener(HitSound);
        GetComponent<Mortality>().onHealthZero.AddListener(DeathSound);
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
