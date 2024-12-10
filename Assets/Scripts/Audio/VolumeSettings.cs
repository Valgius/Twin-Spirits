using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider ambienceSlider;
    [SerializeField] private Slider SFXSlider;

    private void Start()
    {
        SetSliders();

        if (PlayerPrefs.HasKey("musicVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
            SetAmbienceVolume();
            SetSFXVolume();
        }
    }

    public void SetSliders()
    {
        if (musicSlider == null)
            musicSlider = GameObject.Find("MusicSlider").GetComponent<Slider>();
        if (ambienceSlider == null)
            ambienceSlider = GameObject.Find("AmbienceSlider").GetComponent<Slider>();
        if (SFXSlider == null)
            SFXSlider = GameObject.Find("SFXSlider").GetComponent<Slider>();
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        myMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    public void SetAmbienceVolume()
    {
        float volume = ambienceSlider.value;
        myMixer.SetFloat("Ambience", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("ambienceVolume", volume);
    }

    public void SetSFXVolume()
    {
        float volume = SFXSlider.value;
        myMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    private void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        ambienceSlider.value = PlayerPrefs.GetFloat("ambienceVolume");

        SetMusicVolume();
        SetAmbienceVolume();
        SetSFXVolume();
    }
}
