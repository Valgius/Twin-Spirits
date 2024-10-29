using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbTop : MonoBehaviour
{
    public float climbForce = 5f;
    private Rigidbody2D playerRb;

    /// <summary>
    /// Applies force to player when hitting trigger and is moving upwards.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Input.GetAxis("Vertical") == +1)
        {
            playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            playerRb.velocity = new Vector2(playerRb.velocity.x, climbForce);
        }
    }
}
