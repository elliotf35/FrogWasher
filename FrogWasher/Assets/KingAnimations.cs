using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private BoxCollider2D boxCollider;
    private float previousYPosition;

    void Start()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();  // Get the BoxCollider2D component
        previousYPosition = boxCollider.bounds.center.y;  // Initialize with the y position of the collider
    }

    void Update()
    {
        // Check for horizontal movement
        float currentYPosition = boxCollider.bounds.center.y;
        bool isMoving = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);

        // Check if the collider's y position is increasing (jumping)
        bool isJumping = currentYPosition > previousYPosition && Mathf.Abs(boxCollider.bounds.center.y - previousYPosition) > 0.01f;

        // Check if the collider's y position is decreasing (falling)
        bool isFalling = currentYPosition < previousYPosition && Mathf.Abs(boxCollider.bounds.center.y - previousYPosition) > 0.01f;

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
        previousYPosition = currentYPosition;
    }
}
