using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFlowSound : GameBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
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
