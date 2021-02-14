using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;


[System.Serializable]
public class AudioData
{

    public string id;
    public AudioClip File;

    public AudioSource audioSource;

    [Range(0f,1f)]
    public float volume = 1f;
    [Range(0.3f, 3f)]
    public float pitch = 1f;
    public bool onAwake = false;
    public bool loop = false;

}
