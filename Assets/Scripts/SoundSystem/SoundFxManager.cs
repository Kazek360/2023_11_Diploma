using System;
using System.Collections;
using System.Collections.Generic;
using Shared;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundFxManager : Singleton<SoundFxManager>
{
    [SerializeField] private AudioSource soundFxObject;

    /*
     * Dla pojedyńczego dźwięku
     */
    public void PlaySoundFxClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = Instantiate(soundFxObject, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();
        
        Destroy(audioSource.gameObject, audioSource.clip.length);
    }
    
    /*
     * Randomowy dźwięk jeśli doda się ich więcej.
     */
    public void PlayRandomSoundFxClip(AudioClip[] audioClip, Transform spawnTransform, float volume)
    {
        int rand = Random.Range(0, audioClip.Length);
        AudioSource audioSource = Instantiate(soundFxObject, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClip[rand];
        audioSource.volume = volume;
        audioSource.Play();
        
        Destroy(audioSource.gameObject, audioSource.clip.length);
    }
}
