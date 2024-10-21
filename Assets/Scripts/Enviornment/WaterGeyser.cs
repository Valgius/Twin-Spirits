using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterGeyser : GameBehaviour
{
    [SerializeField] private GameObject geyserProjectile;

    public float projectileForce = 1f;
    //[SerializeField] private float setWaterTimer = 1f;
    [SerializeField] private float defaultWaterTimer = 3f;
    [SerializeField] private float waterTimer = 3f;

    public enum Direction { Left, Right, Up, Down }
    public Direction direction;

    private void Start()
    {
        projectileForce = 75;
    }

    void Update()
    {
        if (waterTimer > 0)
        {
            waterTimer -= Time.deltaTime;
        }
        else
            ShootProjectile();
    }


    private void ShootProjectile()
    {
        Instantiate(geyserProjectile, transform.position, Quaternion.identity, this.transform);
        waterTimer = defaultWaterTimer;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ShootProjectile();
            waterTimer = 0.15f;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            defaultWaterTimer = 0.15f;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            defaultWaterTimer = 1f;
        }
    }
}
