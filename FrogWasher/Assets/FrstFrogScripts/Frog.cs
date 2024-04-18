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
    private bool initialWidthSet = false; // Flag to check if initial width has been set
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

        healthBar = Instantiate(healthBarPrefab, transform.position, Quaternion.identity);
        healthBar.transform.SetParent(transform, false);
        healthBar.transform.localPosition = new Vector3(0, 0.15f, 0);
        healthBar.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

        healthBarForeground = healthBar.GetComponentInChildren<Image>();
        patrolScript = GetComponent<enemyPatrol>();
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
        if (initialWidthSet) // Only update health bar if the initial width has been set
        {
            UpdateHealthBar();
        }
        if (health <= 0)
        {
            DisableFrog();
        }
        else
        {
            Knockback(damageDirection);
            if (patrolScript != null)
            {
                patrolScript.ApplySlow(0.5f, 2f); // Apply a slow of 50% for 2 seconds
            }
        }
    }

    IEnumerator EnableMovementAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (patrolScript != null)
            patrolScript.canMove = true; // Re-enable movement in the patrol script
    }

    private void Knockback(Vector2 damageDirection)
    {
        if (canMove)
        {
            rb.AddForce(-damageDirection.normalized * knockbackStrength, ForceMode2D.Impulse);
            if (patrolScript != null)
                patrolScript.ApplySlow(0.25f, knockbackDuration); 
        }
    }

    IEnumerator TempDisableMovement(float duration)
    {
        float previousSpeed = patrolScript.speed;  
        patrolScript.speed *= 0.25f;  
        yield return new WaitForSeconds(duration);
        patrolScript.speed = previousSpeed;  
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

        RemoveBoxCollider();
        rb.gravityScale = 20;
        rb.velocity = Vector2.zero;

        if (patrolScript != null)
        {
            patrolScript.canMove = false;
            patrolScript.rb.velocity = Vector2.zero;
        }

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
