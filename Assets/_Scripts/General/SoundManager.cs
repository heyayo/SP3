using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    static public SoundManager Instance;

    [SerializeField]
    private AudioClip[] audios;

    [SerializeField]
    private AudioClip[] bgms;

    private AudioSource BGM;

    private void Awake()
    {
        Instance = this;

        BGM = GetComponent<AudioSource>();

        // BGM
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
