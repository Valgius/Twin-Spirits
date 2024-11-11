
using UnityEngine;
using System;
using UnityEngine.Audio;
using System.Collections;

public class AudioManager : Singleton<AudioManager>
{
    public Sound[] AmbienceSounds, SFXSounds;

    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource ambienceSource;
    [SerializeField] AudioSource SFXSource;

    [SerializeField] AudioMixer audioMixer;
    //[SerializeField] string musicGroupName = "Music";

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(SFXSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }

        SFXSource.PlayOneShot(s.clip);
    }

    public void PlayAmbience(string name)
    {
        Sound s = Array.Find(AmbienceSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }

        else
        {
            ambienceSource.clip = s.clip;
            ambienceSource.Play();
        }
    }

    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }

    public void ToggleSFX()
    {
        SFXSource.mute = !SFXSource.mute;
    }
    public void ToggleAmbience()
    {
        ambienceSource.mute = !ambienceSource.mute;
    }

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SFXVolume(float volume)
    {
        SFXSource.volume = volume;
    }

    public void AmbienceVolume(float volume)
    {
        ambienceSource.volume = volume;
    }

    public void TransitionToSnapshot(string snapshotName, float transitionTime)
    {
        StartCoroutine(FadeOutAndIn(snapshotName, transitionTime));
    }

    private IEnumerator FadeOutAndIn(string snapshotName, float transitionTime)
    {
        // Fade out
        float startVolume = musicSource.volume;
        for (float t = 0; t < transitionTime; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(startVolume, 0, t / transitionTime);
            yield return null;
        }
        musicSource.volume = 0; // Ensure volume is 0

        // Transition to snapshot
        AudioMixerSnapshot snapshot = audioMixer.FindSnapshot(snapshotName);
        if (snapshot != null)
        {
            snapshot.TransitionTo(transitionTime);
            Debug.Log($"Transitioned to snapshot: {snapshotName}");
        }
        else
        {
            Debug.Log($"Snapshot not found: {snapshotName}");
        }

        // Fade in
        for (float t = 0; t < transitionTime; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(0, startVolume, t / transitionTime);
            yield return null;
        }
        musicSource.volume = startVolume;
    }

    public void StopSFX()
    {
        SFXSource.Stop();
    }
}
