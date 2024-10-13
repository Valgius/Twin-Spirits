using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterGeyser : GameBehaviour
{
    [SerializeField] private GameObject geyserProjectile;
    private GameObject currentCurrant;

    public float projectileForce = 1f;
    [SerializeField] private float setWaterTimer = 1f;
    [SerializeField] private float defaultWaterTimer = 3f;
    [SerializeField] private float waterTimer = 3f;

    public enum Direction { Left, Right, Up, Down }
    public Direction direction;

    void Update()
    {
        if (waterTimer > 0)
        {
            waterTimer -= Time.deltaTime;
        }
        else
            return;
    }


    IEnumerator ShootProjectile()
    {
        yield return new WaitForSeconds(setWaterTimer);
        currentCurrant = Instantiate(geyserProjectile, transform.position, Quaternion.identity, this.transform);
        waterTimer = defaultWaterTimer;
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && waterTimer <= 0)
        {
            StartCoroutine(ShootProjectile());
        }
    }
}
