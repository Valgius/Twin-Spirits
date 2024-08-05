using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 5f;
    public Vector2 direction;

    public void Initialize(Vector2 targetPosition)
    {
        // Calculate the direction from the projectile's position to the target position
        direction = (targetPosition - (Vector2)transform.position).normalized;
    }

    private void Start()
    {
        //Destroy Projectile after certin ammount of time
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        //Moves projectile
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Handle collision with player
            Destroy(gameObject);
        }
        else if (other.CompareTag("Ground"))
        {
            // Handle collision with environment
            Destroy(gameObject);
        }
    }
}
