using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasAttack : MonoBehaviour
{

    public float duration = 5f;          // Total time the gas cloud will be active
    public Vector3 maxSize = new Vector3 (2f,2f,2f);          // Maximum size of the gas cloud

    private float elapsedTime = 0f;
    private Vector3 initialScale;

    void Start()
    {
        initialScale = transform.localScale;

        Destroy(gameObject, duration); // Destroy the gas cloud after its duration
    }

    void Update()
    {
        if (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            transform.localScale = Vector3.Lerp(initialScale, maxSize, t);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        print("hit");
        if (collider.CompareTag("Player"))
        {
            print("player hit");
            collider.GetComponent<PlayerHealth>().EnemyHit();
        }
    }
}
