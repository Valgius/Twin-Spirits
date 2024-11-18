using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCollision : GameBehaviour
{
    private PlayerController playerController;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerController = collision.gameObject.GetComponent<PlayerController>();

            if (!playerController.isSwimming)
                _AM.PlaySFX("Player Collision");
        }

        if (collision.gameObject.name == "Frog")
        {
            _AM.PlaySFX("Frog Land");
        }
    }
}
