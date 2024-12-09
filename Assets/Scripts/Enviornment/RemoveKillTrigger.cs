using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveKillTrigger : GameBehaviour
{
    [SerializeField] private GameObject killArea;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            killArea.SetActive(false);
        }
    }
}
