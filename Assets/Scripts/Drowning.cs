using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Drowning : GameBehaviour
{
    PlayerController playControl;
    [SerializeField] private CanvasGroup fade;
    [SerializeField] private float fadeSpeed = 0.5f;

    [SerializeField] private float soundRate = 0.5f;
    float soundCooldown;

    void Start()
    {
        playControl = GameObject.Find("PlayerSea").GetComponent<PlayerController>();
        fade.alpha = 0;
    }


    void Update()
    {
        if (playControl.isActiveAndEnabled && playControl.breathTimer < 10)
        {
            fade.alpha += Time.deltaTime * fadeSpeed;
            DrowningAudio();
        }
        else
            fade.alpha -= Time.deltaTime * fadeSpeed;
    }

    private void DrowningAudio()
    {
        //Footstep Audio
        soundCooldown -= Time.deltaTime;
        if (soundCooldown < 0)
        {
            soundCooldown = soundRate;
            _AM.PlaySFX("Player Drown");
        }
    }
}
