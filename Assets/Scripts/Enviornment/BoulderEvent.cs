using System.Collections;
using UnityEngine;


public class BoulderEvent : GameBehaviour
{
    private float fallDelay = 1f;
    private float destroyDelay = 2f;
    private bool eventTriggered;
    private Rigidbody2D boulderRb;
    
    [SerializeField] private GameObject boulder;
    [SerializeField] private GameObject stalacitestart;
    [SerializeField] private GameObject stalaciteEnd;

    private void Start()
    {
        boulderRb = boulder.GetComponent<Rigidbody2D>();
        stalacitestart.SetActive(true);
        stalaciteEnd.SetActive(false);
        eventTriggered = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !eventTriggered)
        {
            StartCoroutine(BoulderFall());
        }
        else
            return;
    }

    private IEnumerator BoulderFall()
    {
        _AM.PlaySFX("Boulder Fall");
        yield return new WaitForSeconds(fallDelay);
        boulderRb.bodyType = RigidbodyType2D.Dynamic;
        yield return new WaitForSeconds(destroyDelay);
        boulder.SetActive(false);
        stalacitestart.SetActive(false);
        stalaciteEnd.SetActive(true);
        eventTriggered = true;
    }
}
