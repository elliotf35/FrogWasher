using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private BoxCollider2D boxCollider;
    private PlayerDash playerDash;
    private float previousYPosition;
    private bool isOnPlatform;  // To track whether the player is on a platform

    void Start()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();  // Get the BoxCollider2D component
        previousYPosition = boxCollider.bounds.center.y;  // Initialize with the y position of the collider
        playerDash = GetComponent<PlayerDash>();
        isOnPlatform = false;
    }

    void Update()
    {
        // Check for horizontal movement
        float currentYPosition = boxCollider.bounds.center.y;
        bool isMoving = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);

        // Check if the collider's y position is increasing (jumping) or decreasing (falling)
        bool isJumping = !isOnPlatform && currentYPosition > previousYPosition && Mathf.Abs(currentYPosition - previousYPosition) > 0.01f;
        bool isFalling = !isOnPlatform && currentYPosition < previousYPosition && Mathf.Abs(currentYPosition - previousYPosition) > 0.01f;

        if (playerDash != null && playerDash.IsDashing) // Check if the dash script exists and the player is dashing
        {
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsFalling", false);
        }
        else{
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
        }

        // Update previous position for the next frame
        previousYPosition = currentYPosition;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlatformCollider"))
        {
            isOnPlatform = true;  // Player is on the platform
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlatformCollider"))
        {
            isOnPlatform = false;  // Player has left the platform
        }
    }
}
