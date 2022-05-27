using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //public AudioSource audioSource = new AudioSource;

    public void AudioPlay()
    {
        AudioSource au_source = GetComponent<AudioSource>();
        au_source.PlayOneShot(au_source.clip);
        //Debug.Log("play");
    }

    public float GetLength()
    {
        AudioSource au_source = GetComponent<AudioSource>();
        return au_source.clip.length;
    }
}
