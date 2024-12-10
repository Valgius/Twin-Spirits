using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraChange : GameBehaviour
{
    [SerializeField] private CinemachineVirtualCamera vcam;
    Animator animator;

    void Start()
    {
        animator = vcam.GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            animator.SetTrigger("PanDown");
        }
    }
}
