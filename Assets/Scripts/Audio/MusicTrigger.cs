using System.Collections;
using UnityEngine;
using static System.TimeZoneInfo;

public enum BackgroundType
{
    Treetops,
    Grasslands,
    Undercove,
    None
}

public enum AmbienceType
{
    Forest,
    Stream,
    Caves,
    Waterfall,
    None
}

public class MusicTrigger : GameBehaviour
{
    public BackgroundType backgroundArea;
    public AmbienceType ambienceArea;

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
        switch (backgroundArea)
        {
            case BackgroundType.Treetops:
                _AM.TransitionToSnapshot("Treetops", 1);
                break;

            case BackgroundType.Grasslands:
                _AM.TransitionToSnapshot("Grasslands", 1);
                break;

            case BackgroundType.Undercove:
                _AM.TransitionToSnapshot("Undercove", 1);
                break;
        }

        switch (ambienceArea)
        {
            case AmbienceType.Forest:
                _AM.PlayAmbience("Forest");
                break;

            case AmbienceType.Stream:
                _AM.PlayAmbience("Stream");
                break;

            case AmbienceType.Caves:
                _AM.PlayAmbience("Caves");
                break;

            case AmbienceType.Waterfall:
                _AM.PlayAmbience("Waterfall");
                break;
        }
    }

    public IEnumerator MusicStart()
    {
        yield return new WaitForSeconds(0.1f);
        ChangeMusicLeaf();
    }
}
