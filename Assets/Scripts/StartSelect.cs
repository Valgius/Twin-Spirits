using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSelect : ControllerMenuManager
{
    [SerializeField] private GameObject startObj;
    GalleryManager galleryManager;
    [SerializeField] private GameObject button;
    [SerializeField] private GameObject credits;
    void Start()
    {
        galleryManager = FindObjectOfType<GalleryManager>();
        galleryManager.galleryButton = button;
        credits.SetActive(false);
        SetActiveButton(startObj);
        ShowGalleryButton();
        ShowCreditsButton();
    }

    void ShowGalleryButton()
    {
        if (galleryManager.galleryAvailable)
        {
            galleryManager.galleryButton.SetActive(true);
        }
    }

    void ShowCreditsButton()
    {
        if(galleryManager.galleryAvailable)
        {
            credits.SetActive(true);
        }
    }

}
