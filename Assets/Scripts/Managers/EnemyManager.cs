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
        patrols = GameObject.FindGameObjectsWithTag("Enemy");
    }


    void Update()
    {
        // Loop through each patrol GameObject and deactivate when far away.
        foreach (GameObject patrol in patrols)
        {
            ToggleEnemies(patrol);
        }
    }

    void ToggleEnemies(GameObject patrol)
    {
        // Calculate the distance between the enemy and the player
        float distance = Vector2.Distance(patrol.transform.position, player.position);
        bool isActive = distance <= activationDistance;

        EnemyPatrol ePatrol = patrol.GetComponent<EnemyPatrol>();
        NewFish fish = patrol.GetComponent<NewFish>();

        //If isActive, enemy is active. If not, then the enemy is inactive.
        if (isActive && ePatrol != null)
        {
            ePatrol.ToggleComponents(true);
        }
        else if (!isActive && ePatrol != null)
        {
            ePatrol.ToggleComponents(false);
        }
        if(isActive && fish != null)
        {
            fish.ToggleComponents(true);
        }
        else if(!isActive && fish != null)
        {
            fish.ToggleComponents(false);
        }

    }

}
