using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSelect : ControllerMenuManager
{
    [SerializeField] private GameObject startObj;
    GalleryManager galleryManager;
    [SerializeField] private GameObject button;
    void Start()
    {
        galleryManager = FindObjectOfType<GalleryManager>();
        galleryManager.galleryButton = button;
        SetActiveButton(startObj);
        ShowGalleryButton();
    }

    void ShowGalleryButton()
    {
        if (galleryManager.galleryAvailable)
        {
            galleryManager.galleryButton.SetActive(true);
        }
    }

}
