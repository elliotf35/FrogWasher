using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDash : MonoBehaviour
{
    public float dashDistance = 5f;   // Distance to dash
    public float dashTime = 0.1f;     // Duration of the dash (primarily for animation)
    public float dashCooldown = 5f;   // Cooldown duration in seconds
    private float nextDashTime = 0f;  // When the next dash is allowed

    public Image dashCooldownImage;   // UI Image to show cooldown
    private Color defaultColor;       // Default color of the cooldown image

    private Rigidbody2D rb;
    private Animator animator;
    private bool isFacingRight = true;  // Variable to track the direction the player is facing
    private BoxCollider2D collider; // Reference to the player's collider

    public bool IsDashing { get; private set; } // Publicly accessible IsDashing property

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>(); // Get the BoxCollider2D component
        defaultColor = dashCooldownImage.color; // Save the default color at start
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= nextDashTime)
        {
            StartCoroutine(Dash());
            StartCoroutine(FlashCooldown());
            nextDashTime = Time.time + dashCooldown;  // Set the next available dash time
        }

        // Update the direction the player is facing based on their horizontal movement
        if (Input.GetAxisRaw("Horizontal") > 0)
            isFacingRight = true;
        else if (Input.GetAxisRaw("Horizontal") < 0)
            isFacingRight = false;

        // Continuously update the UI fill amount outside the coroutine
        if (dashCooldownImage != null)
        {
            dashCooldownImage.fillAmount = (nextDashTime - Time.time) / dashCooldown;
        }
    }

    IEnumerator Dash()
    {
        IsDashing = true;  // Set the isDashing flag
        animator.SetBool("isDashing", true);
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"), true);
        collider.enabled = false;  // Disable the collider

        // Teleport the player
        Vector3 dashPosition = transform.position + new Vector3(dashDistance * (isFacingRight ? 1 : -1), 0, 0);
        rb.MovePosition(dashPosition);

        yield return new WaitForSeconds(dashTime);

        IsDashing = false;  // Reset the isDashing flag
        animator.SetBool("isDashing", false);
        yield return new WaitForSeconds(0.8f); // Wait a little longer before re-enabling the collider
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"), false);
        collider.enabled = true;  // Re-enable the collider
    }

    IEnumerator FlashCooldown()
    {
        float endTime = Time.time + dashCooldown;
        while (Time.time < endTime)
        {
            float alpha = Mathf.Lerp(0f, 1f, Mathf.PingPong(Time.time * 2, 1));  // Create a fading effect
            dashCooldownImage.color = new Color(defaultColor.r, defaultColor.g, defaultColor.b, alpha);
            yield return null;
        }
        dashCooldownImage.color = defaultColor;  // Ensure it's set back to default after cooldown
    }
}
