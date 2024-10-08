using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterGeyser : GameBehaviour
{
    [SerializeField] private GameObject waterPush;
    [SerializeField] private GameObject currentCurrant;

    [SerializeField] private float pushbackForce = 1f;
    [SerializeField] private float waterTimer = 1f;

    public enum Direction { Left, Right, Up, Down }
    [SerializeField] public Direction direction;

    void Update()
    {
        if (waterTimer > 0f)
        {
            waterTimer -= Time.deltaTime;
        }
        else
            return;
    }


    void ShootProjectile()
    {
        if(waterTimer <= 0)
        {
            
            currentCurrant = Instantiate(waterPush, transform.position, Quaternion.identity, this.transform);
            currentCurrant.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.down * pushbackForce, ForceMode2D.Force);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ShootProjectile();
        }
    }
}
