using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class AudioManagerScript : MonoBehaviour
{

    public static AudioManagerScript instance;

    [SerializeField] AudioData[] AudioSet = new AudioData[0];

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

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
        foreach(AudioData audio in AudioSet)
        {
            if (audio.audioSource.playOnAwake)
            {
                play(audio.id);
            }
        }
    }

    public void play(string id)
    {
        print($"Playing Audio {id}");
        AudioData data =  Array.Find(AudioSet, audioData => audioData.id == id);
        data.audioSource.Play();
    }

    public void test()
    {

    }

}
