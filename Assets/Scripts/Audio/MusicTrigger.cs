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
    public PlayerSwitch playerSwitch;

    public AreaType myArea;

    public string playerSea;
    public string playerLeaf;


    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.name == "PlayerLeaf")
        {
            if (playerSwitch.isLeafActive == true)
            {
                ChangeMusic();
            }
            else
                return;
        }

        if (other.gameObject.name == "PlayerSea")
        {
            if (playerSwitch.isLeafActive == false)
            {
                ChangeMusic();
            }
            else
                return;
        }
    }

    private void ChangeMusic()
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
}
