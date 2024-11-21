using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalleryManager : MonoBehaviour
{
    public bool galleryAvailable;
    public GameObject galleryButton;

    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Gallery");

        if(objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public void CheckGallery()
    {
        if (galleryAvailable)
        {
            galleryButton.SetActive(true);
        }
    }
}
