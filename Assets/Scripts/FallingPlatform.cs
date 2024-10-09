using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float fallDelay = 1f;
    
    private float destroyDelay = 2f;

    public float respawnDelay = 2f;

    [SerializeField] private Rigidbody2D rb;

    public Vector3 startingPos;

    public void Start()
    {
        startingPos = gameObject.transform.position;
        Debug.Log(startingPos);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(PlatformFall());
        }
    }

    private IEnumerator PlatformFall()
    {
        yield return new WaitForSeconds(fallDelay);
        rb.bodyType = RigidbodyType2D.Dynamic;
        
        StartCoroutine(PlatformRespawn());

    }

    private IEnumerator PlatformRespawn()
    {
        yield return new WaitForSeconds(respawnDelay);
       
        rb.bodyType = RigidbodyType2D.Static;
        gameObject.transform.position = startingPos;
        
    }
}
