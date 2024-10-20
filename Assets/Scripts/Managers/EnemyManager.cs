using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public enum PatrolType
{
    Patrol,
    Detect,
    Chase,
    Attack,
    Die
}

public enum EnemyType
{
    Fish,
    Frog,
    Spider,
}

public class EnemyManager : Singleton<EnemyManager>
{
    public Transform player;
    public float activationDistance = 0f;

    void Update()
    {
        // Find all GameObjects with the "EnemyPatrol" tag
        GameObject[] patrols = GameObject.FindGameObjectsWithTag("EnemyPatrol");

        // Loop through each patrol GameObject
        foreach (GameObject patrol in patrols)
        {
            // Deactivate all children of the patrol GameObject
            foreach (Transform child in patrol.transform)
            {
                // Calculate the distance between the enemy and the player
                float distance = Vector3.Distance(child.position, player.position);

                child.gameObject.SetActive(distance <= activationDistance);
            }
        }
    }


}
