using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWaterAsset : MonoBehaviour
{
    private float lifetime;
    private float speed;
    private Vector2 direction;

    public void Initialize(Vector2 targetPosition, float waterLifetime, float waterSpeed)
    {
        // Calculate the direction from the current position to the target position
        direction = (targetPosition - (Vector2)transform.position).normalized;
        lifetime = waterLifetime;
        speed = waterSpeed;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Destroy Projectile after certin ammount of time
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        // Move the projectile
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }
}
