using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AreaType
{
    Treetops,
    Grasslands,
    Undercove
}

public class MusicTrigger : GameBehaviour
{
    public AreaType myArea;

    public bool leafInZone;
    public bool seaInZone;
    private bool musicStarted;

    private void Start()
    {
        musicStarted = false;
        StartCoroutine(MusicStart());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "PlayerLeaf")
        {
            leafInZone = true;
            ChangeMusicLeaf();
        }

        if (other.gameObject.name == "PlayerSea")
        {
            seaInZone = true;
            ChangeMusicSea();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "PlayerLeaf")
        {
            leafInZone = false;
        }

        if (other.gameObject.name == "PlayerSea")
        {
            seaInZone = false;
        }
    }

    public void ChangeMusicLeaf()
    {
        if (leafInZone && musicStarted)
        {
            PlayMusicForArea();
        }
    }

    public void ChangeMusicSea()
    {
        if (seaInZone && musicStarted)
        {
            PlayMusicForArea();
        }
    }

    private void PlayMusicForArea()
    {
        switch (myArea)
        {
            case AreaType.Treetops:
                _AM.PlayMusic("Treetops");
                break;

            case AreaType.Grasslands:
                _AM.PlayMusic("Grasslands");
                break;

            case AreaType.Undercove:
                _AM.PlayMusic("Undercove");
                break;
        }
    }

    public IEnumerator MusicStart()
    {
        yield return new WaitForSeconds(0.1f);
        musicStarted = true;
        ChangeMusicLeaf();
    }
}
