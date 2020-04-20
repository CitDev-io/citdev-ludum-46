using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : Singleton<AudioManager>
{
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        EventManager.Instance.OnPlayerStoppedShooting += HandleJump;
    }

    void HandleJump() {
        PlaySound("boom");
    }

    void PlaySound(string name)
    {
        AudioClip audioClip = GetAudioClipByName(name);
        if (audioClip != null) {
            audioSource.PlayOneShot(audioClip);
        }
    }
    AudioClip GetAudioClipByName(string clipName)
    {
        return (AudioClip)Resources.Load("Sounds/" + clipName);
    }
}