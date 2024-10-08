using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeyserProjectile : GameBehaviour
{
    [SerializeField] private float timer = 1.2f;
    [SerializeField] private float force = 4;

    WaterGeyser geyser;
    
    
    void Start()
    {
        geyser = transform.parent.GetComponent<WaterGeyser>();
        //this.transform.rotation = this.GetComponentInParent<Transform>().rotation;
        this.transform.rotation = transform.parent.rotation;
    }

    
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        
        switch (geyser.direction)
        {
            case WaterGeyser.Direction.Left:
                this.GetComponent<Rigidbody2D>().velocity = new Vector2(-10, 0);
                break;
                case WaterGeyser.Direction.Right:
                this.GetComponent<Rigidbody2D>().velocity = new Vector2(10, 0);
                break;
                case WaterGeyser.Direction.Up:
                this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 10);
                break;
                case WaterGeyser.Direction.Down:
                this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -10);
                break;
        }
        
    }

    
}
