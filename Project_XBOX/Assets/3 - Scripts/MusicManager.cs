using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private const float MAX_VOLUME = 0.5f;
    private const float MIN_VOLUME = 0f;

    private const float DELAY = 0.01f;
    private const float ADD_VOLUME = 0.0075f;

    // ======================= VARIABLES =======================

    [Header("Threads Music")]
    [SerializeField] private AudioSource gameThread;
    [SerializeField] private AudioSource menuThread;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip bossTheme;
    [SerializeField] private AudioClip tutoTheme;
    [SerializeField] private AudioClip gameTheme;

    // =========================================================

    public void activateMenuThread()
    {
        menuThread.enabled = true;
        menuThread.Play();
        menuThread.mute = false;

        StartCoroutine(IIncreaseVolume(menuThread));
    }

    public void activateGameThread()
    {
        gameThread.mute = false;

        StartCoroutine(IIncreaseVolume(gameThread));
    }

    public void desactivateMenuThread()
    {
        StartCoroutine(IDecreaseVolume(menuThread));
    }

    public void desactivateGameThread()
    {
        StartCoroutine(IDecreaseVolume(gameThread));
    }

    public void setBossTheme()
    {
        gameThread.Stop();
        gameThread.clip = bossTheme;
        gameThread.Play();
    }

    public void setTutoTheme()
    {
        gameThread.Stop();
        gameThread.clip = tutoTheme;
        gameThread.Play();

    }
    public void setGameTheme()
    {
        gameThread.Stop();
        gameThread.clip = gameTheme;
        gameThread.Play();
    }

    private IEnumerator IIncreaseVolume(AudioSource _audio)
    {
        while(_audio.volume < MAX_VOLUME)
        {
            yield return new WaitForSeconds(DELAY);

            _audio.volume += ADD_VOLUME;
        }

        _audio.volume = MAX_VOLUME;
    }

    private IEnumerator IDecreaseVolume(AudioSource _audio)
    {
        while (_audio.volume > MIN_VOLUME)
        {
            yield return new WaitForSeconds(DELAY);

            _audio.volume -= ADD_VOLUME;
        }

        _audio.volume = MIN_VOLUME;

        _audio.mute = true;
    }
}
