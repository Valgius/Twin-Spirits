using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCollision : GameBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        _AM.PlaySFX("Player Collision");

        if (collision.gameObject.name == "Frog")
        {
            _AM.PlaySFX("Frog Land");
        }
    }
}
