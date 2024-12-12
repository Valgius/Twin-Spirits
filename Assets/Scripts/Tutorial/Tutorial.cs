using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tutorial : GameBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private TMP_Text checkpointText;
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetTrigger("Enable");
        text.text = "Press A and D to move left or right.";
    }

    public void JumpTutorial()
    {
        animator.SetTrigger("Enable");
        text.text = "Press Space to Jump.";
    }

    public void DoubleJumpTutorial()
    {
        animator.SetTrigger("Enable");
        text.text = "Leaf Orb obtained! Double Jump unlocked!";
    }

    public void DashTutorial()
    {
        animator.SetTrigger("Enable");
        text.text = "Sea Orb obtained! Press Shift to dash while underwater.";
    }

    public void CheckpointTutorial()
    {
        animator.SetTrigger("Enable");
        text.text = "Checkpoint Reached!";
    }

    public void SwimTutorial()
    {
        animator.SetTrigger("Enable");
        text.text = "Use WASD to swim in any direction.";
    }

    public void CheckpointTeleportTutorial(bool enable)
    {
        animator.SetBool("CheckpointHit", enable);
        checkpointText.text = "Press E to teleport between checkpoints.";
    }
}
