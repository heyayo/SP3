using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    static public SoundManager Instance;

    [SerializeField]
    private AudioClip[] audios;

    [SerializeField]
    private AudioClip[] bgms;

    private AudioSource BGM;

    private void OnEnable()
    {
        Instance = this;

        // BGM
        BGM = gameObject.AddComponent<AudioSource>();
        BGM.loop = true;
        PlayBGM(0);
    }

    public void PlaySound(int index)
    {
        if (audios[index] != null)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = audios[index];
            audioSource.Play();
        }
    }

    public void PlayBGM(int index)
    {
        if (bgms[index] != null)
        {
            BGM.Stop();
            BGM.clip = bgms[index];
            BGM.Play();
        }
    }
}
