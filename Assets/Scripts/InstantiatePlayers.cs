using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiatePlayers : MonoBehaviour
{
    public GameObject playerLeaf;
    public GameObject playerSea;

    public Vector2 leafSpawn;
    public Vector2 seaSpawn;

    private void Start()
    {
        Instantiate(playerLeaf, new Vector2(0, 0), Quaternion.identity );
        Instantiate(playerSea, new Vector2(0, 0), Quaternion.identity);
    }
}
