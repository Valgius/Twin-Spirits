using System.Collections;
using UnityEngine;
using static System.TimeZoneInfo;

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
        if (leafInZone)
        {
            PlayMusicForArea();
        }
    }

    public void ChangeMusicSea()
    {
        if (seaInZone)
        {
            PlayMusicForArea();
        }
    }

    private void PlayMusicForArea()
    {
        switch (myArea)
        {
            case AreaType.Treetops:
                _AM.TransitionToSnapshot("Treetops", 1);
                _AM.PlayAmbience("Trees");
                break;

            case AreaType.Grasslands:
                _AM.TransitionToSnapshot("Grasslands", 1);
                _AM.PlayAmbience("Ground");
                break;

            case AreaType.Undercove:
                _AM.TransitionToSnapshot("Undercove", 1);
                _AM.PlayAmbience("Caves");
                break;
        }
    }

    public IEnumerator MusicStart()
    {
        yield return new WaitForSeconds(0.1f);
        ChangeMusicLeaf();
    }
}
