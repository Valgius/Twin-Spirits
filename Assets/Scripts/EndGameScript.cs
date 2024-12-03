using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameScript : GameBehaviour
{
    [SerializeField] private GameObject loadScene;
    [SerializeField] private GameObject playerLeaf;
    [SerializeField] private GameObject playerSea;
    [SerializeField] private GameObject fakeSea;
    [SerializeField] private GalleryManager gallery;
    [SerializeField] private bool leafFinished;
    [SerializeField] private bool seaFinished;
    private SpriteRenderer seaSprite;


    private void Start()
    {
        leafFinished = false;
        seaFinished = false;
        fakeSea.SetActive(false);
        gallery = GameObject.FindGameObjectWithTag("Gallery").GetComponent<GalleryManager>();

        Physics2D.IgnoreCollision(playerSea.GetComponent<BoxCollider2D>(), fakeSea.GetComponent<BoxCollider2D>());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == playerLeaf)
        {
            leafFinished = true;
            CheckEndStatus();
        }

        if (collision.gameObject == playerSea)
        {
            seaFinished = true;
            CheckEndStatus();
            ActivateFakeSea();
        }
    }

    private void CheckEndStatus()
    {
        if (leafFinished && seaFinished)
        {
            loadScene.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            if (gallery != null)
            {
                gallery.galleryAvailable = true;
            }
        }
            
    }

    private void ActivateFakeSea()
    {
        seaSprite = playerSea.GetComponent<SpriteRenderer>();
        seaSprite.enabled = false;
        fakeSea.SetActive(true);
    }
}
