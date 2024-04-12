using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private Vector3 previousPosition;

    void Start()
    {
        animator = GetComponent<Animator>();
        previousPosition = transform.position;
    }

    void Update()
    {
        // Check for horizontal movement
        bool isMoving = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);

        // Check if the character's y position is increasing (jumping)
        bool isJumping = transform.position.y > previousPosition.y;

        // Check if the character's y position is decreasing (falling)
        bool isFalling = transform.position.y < previousPosition.y;

        // Prioritize vertical movements over horizontal movement
        if (isJumping || isFalling)
        {
            animator.SetBool("IsRunning", false); // Override isMoving if jumping or falling
            animator.SetBool("IsJumping", isJumping);
            animator.SetBool("IsFalling", isFalling);
        }
        else
        {
            animator.SetBool("IsRunning", isMoving);
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsFalling", false);
        }

        // Update previous position for the next frame
        previousPosition = transform.position;
    }
}
