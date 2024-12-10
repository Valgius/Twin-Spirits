using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : GameBehaviour
{
    public float fallDelay = 1f;
    
    //private float destroyDelay = 2f;
    Animator animator;

    public float respawnDelay = 2f;

    [SerializeField] private Rigidbody2D rb;

    public Vector3 startingPos;

    public void Start()
    {
        startingPos = gameObject.transform.position;
        animator = GetComponent<Animator>();
       // Debug.Log(startingPos);
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
        animator.SetTrigger("StartFall");
        rb.bodyType = RigidbodyType2D.Dynamic;
        _AM.PlaySFX("Platform Fall");
        
        StartCoroutine(PlatformRespawn());
    }

    private IEnumerator PlatformRespawn()
    {
        yield return new WaitForSeconds(respawnDelay);
       
        rb.bodyType = RigidbodyType2D.Static;
        gameObject.transform.position = startingPos;
        
    }
}
