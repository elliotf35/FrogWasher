using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnockback : MonoBehaviour
{
    public float knockbackStrength = 10f;
    public float verticalBoost = 1f;
    public float immunityDuration = 1f;  // Duration of immunity and non-collision
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool canBeKnockedBack = true;  // Flag to control knockback application

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (rb == null) {
            Debug.LogError("PlayerKnockback: No Rigidbody2D found on the player.");
        }
        if (animator == null) {
            Debug.LogError("PlayerKnockback: No Animator found on the player.");
        }
        if (spriteRenderer == null) {
            Debug.LogError("PlayerKnockback: No SpriteRenderer found on the player.");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Frog enemy = collision.gameObject.GetComponent<Frog>();
        if (enemy != null && canBeKnockedBack)
        {
            // Calculate the direction from the enemy to the player
            Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized;

            // Adding more horizontal push by increasing the x component and applying vertical boost
            Vector2 forceDirection = new Vector2(knockbackDirection.x, verticalBoost).normalized * knockbackStrength;

            // Ensure the force is pushing the player away, adjust x to always push away
            forceDirection.x *= -1;

            // Apply the knockback force
            rb.AddForce(forceDirection, ForceMode2D.Impulse);

            StartCoroutine(Invulnerability());
        }
    }

    IEnumerator Invulnerability()
    {
        canBeKnockedBack = false;  // Disable knockback
        animator.SetBool("isHurt", true);
        yield return new WaitForSeconds(.1f);  // Just enough time for the animation to trigger
        animator.SetBool("isHurt", false);

        // Ignore collisions with the enemy layer
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"), true);

        StartCoroutine(BlinkEffect(immunityDuration));
        yield return new WaitForSeconds(immunityDuration);

        // Re-enable collisions with the enemy layer
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"), false);
        canBeKnockedBack = true;  // Re-enable knockback
        spriteRenderer.enabled = true; // Ensure sprite is visible after blinking
    }

    IEnumerator BlinkEffect(float duration)
    {
        float endTime = Time.time + duration;
        while (Time.time < endTime)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(.1f);
        }
        spriteRenderer.enabled = true; // Ensure sprite is visible after blinking
    }
}