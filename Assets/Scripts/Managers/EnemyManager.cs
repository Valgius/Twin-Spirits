using Unity.VisualScripting;
using UnityEngine;

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

    public GameObject[] patrols;

    private void Start()
    {
        // Find all GameObjects with the "EnemyPatrol" tag
        patrols = GameObject.FindGameObjectsWithTag("EnemyPatrol");
    }


    void Update()
    {
        // Loop through each patrol GameObject and deactivate when far away.
        foreach (GameObject patrol in patrols)
        {
            if (patrol.transform.CompareTag("Enemy"))
            {
                continue;
            }
            ToggleEnemies(patrol);
        }
    }

    void ToggleEnemies(GameObject patrol)
    {
        // Calculate the distance between the enemy and the player (Edit: turns out that this is checking the distance of the enemies parent object and not the enemy)
        float distance = Vector2.Distance(patrol.transform.position, player.position);
        bool isActive = distance <= activationDistance;

        //If isActive, enemy is active. If not, then the enemy is inactive.
        if (isActive)
        {
            patrol.SetActive(true);
        }
        else
        {
            patrol.SetActive(false);
        }
    }

}
