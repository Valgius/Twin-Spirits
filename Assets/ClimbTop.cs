using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbTop : MonoBehaviour
{
    public float climbForce = 5f;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * climbForce);
        }
    }
}
