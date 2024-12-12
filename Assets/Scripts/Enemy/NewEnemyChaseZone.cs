using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEnemyChaseZone : GameBehaviour
{
    [SerializeField] private GameObject enemy;
    NewFish fish;
    public bool canFollow;

    private void Start()
    {
        fish = enemy.GetComponent<NewFish>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            print("Zone Entered");
            canFollow = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            print("Zone Exited");
            canFollow = false;
        }
    }
}
