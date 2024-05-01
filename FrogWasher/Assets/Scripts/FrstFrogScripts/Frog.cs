using UnityEngine;
using UnityEngine.UI;
using System.Collections; 

public class Frog : MonoBehaviour
{
    public float health = 370;
    public GameObject healthBarPrefab; // Assign the health bar prefab in the inspector
    private GameObject healthBar; // This will be the instantiated health bar
    public Image healthBarForeground; // This will reference the foreground image of the instantiated prefab
    private float maxHealth = 400;
    private PlayerProjectileRender ppr;
    public GameObject pwt;
    public Rigidbody2D rb;
    private float initialHealthBarWidth;
    private bool initialWidthSet = false; 
    private enemyPatrol patrolScript; 
    private Animator animator;
    public PlayerKnockback playerKnockback;

    // Knockback variables
    public float knockbackStrength = 5f;
    public float knockbackDuration = 0.5f;
    private bool canMove = true;

    void Start()
    {
        ppr = pwt.GetComponent<PlayerProjectileRender>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        patrolScript = GetComponent<enemyPatrol>();

        SetupHealthBar();
    }

    void SetupHealthBar()
    {
        healthBar = Instantiate(healthBarPrefab, transform.position + Vector3.up * 0.15f, Quaternion.identity, transform);
        healthBar.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        healthBarForeground = healthBar.GetComponentInChildren<Image>();
        initialHealthBarWidth = healthBarForeground.rectTransform.sizeDelta.x;
        initialWidthSet = true;
    }

    void Update()
    {
        // Set initial width on the first Update call after instantiation
        if (!initialWidthSet && healthBarForeground != null)
        {
            initialHealthBarWidth = healthBarForeground.rectTransform.sizeDelta.x;
            initialWidthSet = true;
        }
    }

    public void TakeDamage(float damageAmount, Vector2 damageDirection)
    {
        health -= damageAmount;
        UpdateHealthBar();

        if (health <= 0)
        {
            DisableFrog();
        }
        else
        {
            Knockback(damageDirection);
            // Ensure slowdown is clearly applied, consider adjusting the factor and duration.
            if (patrolScript != null)
            {
                patrolScript.ApplySlow(0.5f, 2f); // Slows down to 50% of original speed for 2 seconds
            }
        }
    }


    private void ApplyDamageEffects(Vector2 damageDirection)
    {
        Knockback(damageDirection);
        patrolScript?.ApplySlow(0.5f, 2f);  // Apply a slow of 50% for 2 seconds
    }

    private void Knockback(Vector2 damageDirection)
    {
        if (canMove)
        {
            // Adjust the knockback strength based on current health to reduce as health decreases
            float adjustedKnockbackStrength = knockbackStrength * (health / maxHealth);
            Vector2 knockbackForce = -damageDirection.normalized * adjustedKnockbackStrength;
            rb.AddForce(knockbackForce, ForceMode2D.Impulse);
        }
    }


    IEnumerator TempDisableMovement(float duration, float slowFactor)
    {
        var currentSpeed = patrolScript.speed;
        patrolScript.speed *= slowFactor;
        yield return new WaitForSeconds(duration);
        patrolScript.speed = currentSpeed; // Restore the exact current speed, not the originalSpeed
    }


    private void UpdateHealthBar()
    {
        if (healthBarForeground != null)
        {
            if (health > maxHealth) {
                health = maxHealth;
            }

            float healthRatio = Mathf.Clamp(health / maxHealth, 0f, 1f);
            healthBarForeground.rectTransform.sizeDelta = new Vector2(initialHealthBarWidth * healthRatio, healthBarForeground.rectTransform.sizeDelta.y);;
        }
    }

    private void DisableFrog()
    {
        if (animator != null)
        {
            animator.SetBool("IsDying", true);  // Trigger the death animation
        }

        if (playerKnockback != null)
        {
            playerKnockback.IncreaseHealth(1);  // Call to increase health
        }

        if (patrolScript != null)
        {
            patrolScript.canMove = false;
        }

        RemoveBoxCollider();
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;

        StartCoroutine(DestroyAfterDelay(.8f)); // Delay destruction to show any death animations
    }


    private System.Collections.IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(healthBar);
        Destroy(gameObject);
    }

    void RemoveBoxCollider()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        if (collider != null)
        {
            Destroy(collider);
        }
    }
    
}
