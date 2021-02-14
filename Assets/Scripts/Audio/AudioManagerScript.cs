using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManagerScript : MonoBehaviour
{

    [SerializeField] AudioData[] AudioSet = new AudioData[0];

    private void Awake()
    {
        foreach(AudioData audio in AudioSet)
        {
            audio.audioSource = gameObject.AddComponent<AudioSource>();
            audio.audioSource.clip = audio.File;    
            audio.audioSource.playOnAwake = audio.onAwake;
            audio.audioSource.volume = audio.volume;
            audio.audioSource.pitch = audio.pitch;
            audio.audioSource.playOnAwake = audio.onAwake;
            audio.audioSource.loop = audio.loop;
        }
    }

    public void Start()
    {
        
    }

    public void play(string id)
    {
        print("PLAY SOMETHING GODDAMNIT");
        AudioData data =  Array.Find(AudioSet, audioData => audioData.id == id);
        data.audioSource.Play();
    }

    public void test()
    {

    }

}
