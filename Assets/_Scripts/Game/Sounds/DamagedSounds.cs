using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedSounds : MonoBehaviour
{
    [Header("Hit Sound")]
    [SerializeField]
    private bool hitSoundTrue = true;

    [SerializeField]
    private int hitSoundIndex = 1;

    [Header("Death Sound")]
    [SerializeField]
    private bool deathSoundTrue = true;

    [SerializeField]
    private int deathSoundIndex = 3;

    void Start()
    {
        GetComponent<Damagable>().onHit.AddListener(HitSound);
        GetComponent<Mortality>().onHealthZero.AddListener(DeathSound);
    }

    private void HitSound()
    {
        if (hitSoundTrue)
            SoundManager.Instance.PlaySound(hitSoundIndex);
    }

    private void DeathSound()
    {
        if (deathSoundTrue)
            SoundManager.Instance.PlaySound(deathSoundIndex);
    }
}
