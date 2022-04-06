using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip[] clips;
    private AudioSource audioSource;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void playAudioClip(int index)
    {
        if (clips[index] != null)
        {
            audioSource.volume = 0.5f;
            audioSource.pitch = 1;
            audioSource.PlayOneShot(clips[index]);
        }
        else
            Debug.Log("Attention, clip vide.");
    }

    public void playAudioClipWithPitch(int index, float pitch)
    {
        if (clips[index] != null)
        {
            audioSource.volume = 0.5f;
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(clips[index]);
        }
        else
            Debug.Log("Attention, clip vide.");
    }

    public void playAudioClipWithVolume(int index, float volume)
    {
        if (clips[index] != null)
        {
            audioSource.volume = volume;
            audioSource.pitch = 1;
            audioSource.PlayOneShot(clips[index]);
        }
        else
            Debug.Log("Attention, clip vide.");
    }
}
