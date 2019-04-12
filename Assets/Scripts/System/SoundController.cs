using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioSource Source { get; private set; }
    void Awake()
    {
        Source = transform.GetComponent<AudioSource>();
    }

    public void Setup(AudioClip clip)
    {
        Source.clip = clip;
        Source.Play();
    }

    void OnDestroy()
    {
        if (Source.isPlaying)
        {
            Source.Stop();
        }
    }
    
}
