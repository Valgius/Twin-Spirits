
using UnityEngine;
using System;

public class AudioManager : Singleton<AudioManager>
{
    public Sound[] musicSounds, AmbienceSounds, SFXSounds;

    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource ambienceSource;
    [SerializeField] AudioSource SFXSource;

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
            return;
        }

        // Check if the current clip is already playing
        if (musicSource.isPlaying && musicSource.clip == s.clip)
        {
            Debug.Log($"{name} is already playing.");
            return; // Don't play the song again if it's already playing
        }

        // Set the clip and play it
        musicSource.clip = s.clip;
        musicSource.Play();
        Debug.Log($"Playing: {name}");
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(SFXSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }

        else
        {
            SFXSource.PlayOneShot(s.clip);

        }
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
}
