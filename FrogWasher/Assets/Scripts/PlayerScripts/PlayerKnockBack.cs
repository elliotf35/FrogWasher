using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerKnockback : MonoBehaviour
{
    public float knockbackStrength = 10f;
    public float verticalBoost = 1f;
    public float immunityDuration = 1f;  // Duration of immunity and non-collision
    public int maxHealth = 6;            // Maximum health points
    private int currentHealth;           // Current health points
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool canBeKnockedBack = true;  // Flag to control knockback application
    public HealthDisplay healthDisplay;
    public AudioSource audioSource;
    public AudioClip hitSound;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;  // Initialize health
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Frog enemy = collision.gameObject.GetComponent<Frog>();
        if (enemy != null && canBeKnockedBack)
        {
            Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized;
            Vector2 forceDirection = new Vector2(knockbackDirection.x, verticalBoost).normalized;
            rb.AddForce(forceDirection * knockbackStrength, ForceMode2D.Impulse);

            ReduceHealth(2);  // Reduce health by 2 on each collision

            StartCoroutine(Invulnerability());
        }
    }

    void ReduceHealth(int damage)
    {
        currentHealth -= damage;
        healthDisplay.UpdateHealth(currentHealth);

        // Play hit sound
        if (hitSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hitSound);
        }

        if (currentHealth <= 0)
        {
            HandleDefeat();  // Handle the player's defeat
        }
    }


    void HandleDefeat()
    {
        Debug.Log("Player Defeated - Restarting Game");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reloads the current scene
    }

    IEnumerator Invulnerability()
    {
        canBeKnockedBack = false;  // Disable knockback
        animator.SetBool("isHurt", true);
        yield return new WaitForSeconds(.1f);  // Just enough time for the animation to trigger
        animator.SetBool("isHurt", false);

        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"), true);

        StartCoroutine(BlinkEffect(immunityDuration));
        yield return new WaitForSeconds(immunityDuration);

        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"), false);
        canBeKnockedBack = true;  // Re-enable knockback
        spriteRenderer.enabled = true; // Ensure sprite is visible after blinking
    }
    public void IncreaseHealth(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth); // Ensure health does not exceed maximum
        healthDisplay.UpdateHealth(currentHealth); // Update the UI
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
