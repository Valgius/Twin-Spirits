using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterGeyser : GameBehaviour
{
    [SerializeField] private GameObject geyserProjectile;
    private GameObject currentCurrant;

    public float projectileForce = 1f;
    [SerializeField] private float setWaterTimer = 1f;
    private float waterTimer = 1f;

    public enum Direction { Left, Right, Up, Down }
    public Direction direction;

    void Update()
    {
        if (waterTimer > 0)
        {
            waterTimer -= Time.deltaTime;
        }
        else
            ShootProjectile();
    }


    void ShootProjectile()
    {
        currentCurrant = Instantiate(geyserProjectile, transform.position, Quaternion.identity, this.transform);
        waterTimer = setWaterTimer;
    }
}
