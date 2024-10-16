using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFxManager : MonoBehaviour
{
    public static SoundFxManager instance;
    [SerializeField] private AudioSource soundFxObject;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlaySoundFxClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = Instantiate(soundFxObject, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();
        
        Destroy(audioSource.gameObject, audioSource.clip.length);
    }
}
