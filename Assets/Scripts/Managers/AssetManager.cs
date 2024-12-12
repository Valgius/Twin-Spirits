using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManager : Singleton<AssetManager>
{
    public GameObject[] assetList;
    public GameObject[] coreList;

    public float activationDistance = 0f;

    public Transform player;

    public bool useScript;

    private void Start()
    {
        // Find all GameObjects with the "EnemyPatrol" tag
        assetList = GameObject.FindGameObjectsWithTag("Asset");
    }

    // Update is called once per frame
    void Update()
    {
        // Loop through each GameObject with asset tag and deactivate when far away.
        foreach (GameObject asset in assetList)
        {
            if(useScript)
                ToggleAssets(asset);
            else
                AssetActive (asset, true);
        }

        // Loop through each platform/climb and deactivate when far away.
        //foreach (GameObject asset in coreList)
        //{
        //    // Loop through each child of the asset.
        //    foreach (Transform child in asset.transform)
        //    {
        //        if (useScript)
        //            ToggleAssets(child.gameObject);
        //        else
        //            AssetActive (child.gameObject, true) ;
        //    }
        //}
    }

    void ToggleAssets(GameObject asset)
    {
        // Calculate the distance between the enemy and the player
        float distance = Vector2.Distance(asset.transform.position, player.position);

        if (distance <= activationDistance)
            AssetActive(asset, true);
        else
            AssetActive(asset, false);
    }

    void AssetActive(GameObject asset, bool active)
    {
        asset.SetActive(active);
    }

    //To turn off this script for camera pans trigger this bool through _AsM.ToggleScriptUse(true/false)
    public void ToggleScriptUse(bool active)
    {
        useScript = active;
    }
}
