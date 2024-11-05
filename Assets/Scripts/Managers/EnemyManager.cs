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
        // Loop through each patrol GameObject
        foreach (GameObject patrol in patrols)
        {
            // Deactivate Enemy Children of the patrol GameObject
            foreach (Transform child in patrol.transform)
            {
                if (child.CompareTag("Enemy"))
                {
                    // Calculate the distance between the enemy and the player
                    float distance = Vector3.Distance(child.transform.position, player.position);

                    //Get Enemy Patrol script
                    EnemyPatrol enemyPatrol = child.GetComponent<EnemyPatrol>();

                    //child.gameObject.SetActive(distance <= activationDistance);

                    if (distance > activationDistance)
                    {
                        if(enemyPatrol != null)
                        {
                            enemyPatrol.ToggleComponents(false);
                        }
                    }
                    else
                    {
                        enemyPatrol.ToggleComponents(true);
                    }
                        
                }
                else
                {
                    // Use continue to skip non-enemy children
                    continue;
                }
            }
        }
    }
}
