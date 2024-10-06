using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public Slider _musicSlider, _SFXSlider;

    public void ToggleMusic()
    {
        _AM.ToggleMusic();
    }

    public void ToggleSFX()
    {
        _AM.ToggleSFX();
    }

    public void MusicVolume()
    {
        _AM.MusicVolume(_musicSlider.value);
    }

    public void SFXVolume()
    {
        _AM.SFXVolume(_musicSlider.value);
    }
}
