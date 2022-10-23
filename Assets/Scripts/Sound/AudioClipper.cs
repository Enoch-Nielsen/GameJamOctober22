using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClipper
{
    public AudioClip clip;
    public bool loop;

    public AudioClipper(AudioClip clip, bool loop)
    {
        this.clip = clip;
        this.loop = loop;
    }
}
