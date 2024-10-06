
using UnityEngine;
using System;

public class AudioManager : Singleton<AudioManager>
{
    public Sound[] musicSounds, SFXSounds;


    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    public void Start()
    {
        PlayMusic("Treetops");
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }

        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
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

    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }

    public void ToggleSFX()
    {
        SFXSource.mute = !SFXSource.mute;
    }

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SFXVolume(float volume)
    {
        SFXSource.volume = volume;
    }
}
