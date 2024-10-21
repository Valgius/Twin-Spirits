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

    private GameObject[] patrols;

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
                if (child.CompareTag("Enemy"))
                {
                    // Calculate the distance between the enemy and the player
                    float distance = Vector3.Distance(child.transform.position, player.position);

                    //child.gameObject.SetActive(distance <= activationDistance);

                    ToggleComponents(distance <= activationDistance, child.gameObject);
                }
                else return;
            }
        }
    }
    private void ToggleComponents(bool value, GameObject child)
    {
        // Get components
        Rigidbody2D rb = child.GetComponent<Rigidbody2D>();
        EnemyPatrol enemyPatrol = child.GetComponent<EnemyPatrol>();
        SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
        BoxCollider2D boxCollider2d = child.GetComponent<BoxCollider2D>();

        // Disable components
        rb.isKinematic = !rb.isKinematic;
        spriteRenderer.enabled = !spriteRenderer.enabled;
        boxCollider2d.enabled = !boxCollider2d.enabled;
        enemyPatrol.myPatrol = PatrolType.Patrol;
    }
}
