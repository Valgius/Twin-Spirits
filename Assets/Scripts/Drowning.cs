using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Drowning : MonoBehaviour
{
    PlayerController playControl;
    [SerializeField] private CanvasGroup fade;
    [SerializeField] private float fadeSpeed = 0.5f;

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
        }
        else
            fade.alpha -= Time.deltaTime * fadeSpeed;
    }
}
