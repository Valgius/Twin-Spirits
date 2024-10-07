using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.EventSystems;

public class WaterGeysers : GameBehaviour
{
    private Vector2 currentDirection;

    [SerializeField] private float geyserForce = 20f;
    private void Start()
    {
        currentDirection = new Vector2(this.transform.up.x, transform.up.y);
    }

    public Vector2 GetCurrentDirection()
    {
        return currentDirection * geyserForce;   
    }
}
