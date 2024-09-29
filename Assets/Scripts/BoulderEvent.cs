using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderEvent : MonoBehaviour
{
    private float fallDelay = 1f;
    private float destroyDelay = 2f;
    private Rigidbody2D boulderRb;

    [SerializeField] private GameObject boulder;
    [SerializeField] private GameObject stalacitestart;
    [SerializeField] private GameObject stalaciteEnd;


    private void Start()
    {
        boulderRb = boulder.GetComponent<Rigidbody2D>();
        stalacitestart.SetActive(true);
        stalaciteEnd.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(BoulderFall());
        }
    }

    private IEnumerator BoulderFall()
    {
        yield return new WaitForSeconds(fallDelay);
        boulderRb.bodyType = RigidbodyType2D.Dynamic;
        Destroy(boulder, destroyDelay);
        stalacitestart.SetActive(false);
        stalaciteEnd.SetActive(true);
    }
}
