using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skip : GameBehaviour
{
    public GameObject skipButton;
    GalleryManager _gm;

    void Start()
    {
        _gm = FindObjectOfType<GalleryManager>();
        if(_gm != null && _gm.galleryAvailable == true)
        {
            skipButton.SetActive(true);
        }
        else if (_gm != null && _gm.galleryAvailable == false)
        {
            skipButton.SetActive(false);
        }
            
    }
}
