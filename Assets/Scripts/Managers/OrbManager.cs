using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrbManager : MonoBehaviour
{
    [Header("- Player -")]
    public GameObject playerLeaf;
    public GameObject playerSea;
    private PlayerController playerLeafController;
    private PlayerController playerSeaController;

    [Header("- Panel -")]
    public GameObject orbPanelLeaf;
    public GameObject orbPanelSea;
    private Image orbPanelLeafImage;
    private Image orbPanelSeaImage;

    [Header("- Orb -")]
    public GameObject orbLeaf;
    public GameObject orbSea;
    public Sprite orbLeafImage;
    public Sprite orbSeaImage;

    // Start is called before the first frame update
    void Start()
    {
        //Deactivate both orb Panels
        orbPanelLeaf.SetActive(false);
        orbPanelSea.SetActive(false);

        // Get the Image components from the panels
        orbPanelLeafImage = orbPanelLeaf.GetComponent<Image>();
        orbPanelSeaImage = orbPanelSea.GetComponent<Image>();

        //Get the player Controller Scripts from both players
        playerLeafController = playerLeaf.GetComponent<PlayerController>();
        playerSeaController = playerSea.GetComponent<PlayerController>();


    }

    private void SetOrbPanelActive(GameObject panel, Image panelImage, Sprite orbImage)
    {
        panel.SetActive(true);
        if (panelImage != null && orbImage != null)
        {
            panelImage.sprite = orbImage;
        }
    }

    public void LeafLeafOrbCollision()
    {
        SetOrbPanelActive(orbPanelLeaf, orbPanelLeafImage, orbLeafImage);
        orbLeaf.SetActive(false);
        playerLeafController.ToggleHasLeafOrb();
    }

    public void LeafSeaOrbCollision()
    {
        SetOrbPanelActive(orbPanelLeaf, orbPanelLeafImage, orbSeaImage);
        orbSea.SetActive(false);
        playerLeafController.ToggleHasSeaOrb();
    }

    public void SeaLeafOrbCollision()
    {
        SetOrbPanelActive(orbPanelSea, orbPanelSeaImage, orbLeafImage);
        orbLeaf.SetActive(false);
        playerSeaController.ToggleHasLeafOrb();
    }

    public void SeaSeaOrbCollision()
    {
        SetOrbPanelActive(orbPanelSea, orbPanelSeaImage, orbSeaImage);
        orbSea.SetActive(false);
        playerSeaController.ToggleHasSeaOrb();
    }
}
