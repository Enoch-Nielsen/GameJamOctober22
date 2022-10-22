using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private GameObject audioObject;
    public List<AudioClip> soundQueue;
    public List<AudioPlayer> soundsPlaying;
    public float volume;

    private void Update()
    {
        foreach (var clip in soundQueue.ToArray())
        {
            AudioPlayer source = Instantiate(audioObject).GetComponent<AudioPlayer>();
            source.audioSource.clip = clip;
            source.audioSource.volume = volume;
            source.soundsPlaying = soundsPlaying;
            
            soundsPlaying.Add(source);
            soundQueue.Remove(clip);
        }
    }

    public AudioSource GetSound(AudioClip clip)
    {
        foreach (var source in soundsPlaying)
        {
            if (source.audioSource.clip == clip)
            {
                return source.audioSource;
            }
        }

        return new AudioSource();
    }

    public void AddSoundToQueue(AudioClip clip)
    {
        soundQueue.Add(clip);
    }
}
