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

    // Private Variables
    private bool died;  // Make sure the sound only plays one time

    void Start()
    {
        GetComponent<Damagable>().onHit.AddListener(HitSound);
        GetComponent<Mortality>().onHealthZero.AddListener(DeathSound);

        died = false;
    }

    private void HitSound()
    {
        if (hitSoundTrue)
            SoundManager.Instance.PlaySound(hitSoundIndex);
    }

    private void DeathSound()
    {
        if (deathSoundTrue && !died)
        {
            SoundManager.Instance.PlaySound(deathSoundIndex);
            died = true;
        }
    }
}
