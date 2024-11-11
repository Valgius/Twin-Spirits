using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.EventSystems;

public class WaterFlow : GameBehaviour
{
    public Vector2 currentDirection;

    [SerializeField] private float flowForce = 20f;
    private void Start()
    {
        ResetCurrentDirection();
    }

    public Vector2 GetCurrentDirection()
    {
        return currentDirection * flowForce;   
    }

    public void ResetCurrentDirection()
    {
        currentDirection = new Vector2(this.transform.up.x, transform.up.y);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _AM.PlaySFX("Water Current");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _AM.StopSFX();
        }
    }
}
