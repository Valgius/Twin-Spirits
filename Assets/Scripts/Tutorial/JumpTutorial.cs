using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTutorial : GameBehaviour
{
    Tutorial tutorial;
    // Start is called before the first frame update
    void Start()
    {
        tutorial = FindObjectOfType<Tutorial>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            tutorial.JumpTutorial();
        }
    }
}
