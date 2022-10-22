using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public List<AudioPlayer> soundsPlaying;
    
    void Start()
    {
        audioSource.Play();
        Destroy(gameObject, audioSource.clip.length + 1.0f);
    }

    private void OnDestroy()
    {
        soundsPlaying.Remove(this);
    }
}
