using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalleryCheck : GameBehaviour
{
    GalleryManager galleryManager;


    void Start()
    {
        galleryManager = FindObjectOfType<GalleryManager>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GalleryAvailableToggle()
    {
        if(galleryManager != null)
        {
            galleryManager.galleryAvailable = true;
        }
    }
}
