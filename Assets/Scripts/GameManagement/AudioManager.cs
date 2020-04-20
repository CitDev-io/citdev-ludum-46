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
        EventManager.Instance.OnPlayerJumpSuccessful += HandleJump;
        EventManager.Instance.OnPlayerLanded += HandleLand;
        EventManager.Instance.OnPlayerShotFailedNoEnergy += HandleShotFailedNoEnergy;
        EventManager.Instance.OnPlayerStartedShooting += HandleStartedShooting;
        EventManager.Instance.OnPlayerStoppedShooting += HandleStoppedShooting;
    }

    void HandleJump() {
        PlaySound("Jumping");
    }

    void HandleLand() {
        PlaySound("Landing");
    }

    void HandleShotFailedNoEnergy() {
        Debug.Log("NO ENERGY");
        PlaySound("Firing_OutOfEnergy");
    }

    void HandleStartedShooting() {
        Debug.Log("FIRING");
        PlaySound("Firing_WithEnergy");
    }

    void HandleStoppedShooting() {
        Debug.Log("STOP FIRING");
        PlaySound("Firing_Stops");
    }

    void PlaySound(string name)
    {
        AudioClip audioClip = GetAudioClipByName(name);
        Debug.Log(audioClip);
        if (audioClip != null) {
            audioSource.PlayOneShot(audioClip);
        } else {
            Debug.Log("null audio clip: " + name);
        }
    }
    AudioClip GetAudioClipByName(string clipName)
    {
        return (AudioClip)Resources.Load("Sounds/" + clipName);
    }
}