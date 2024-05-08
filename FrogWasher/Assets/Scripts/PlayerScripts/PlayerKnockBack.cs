using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerKnockback : MonoBehaviour
{
    public float knockbackStrength = 10f;
    public float verticalBoost = 1f;
    public float immunityDuration = 1f;  
    public int maxHealth = 6;            
    private int currentHealth;           
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool canBeKnockedBack = true;  
    public HealthDisplay healthDisplay;
    public AudioSource audioSource;
    public AudioClip hitSound;
    public float externalForceX;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;  
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Lightning") && canBeKnockedBack)
        {
            ReduceHealth(1);  
            StartCoroutine(Invulnerability());  
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Frog enemy = collision.gameObject.GetComponent<Frog>();
        if (enemy != null && canBeKnockedBack)
        {
            Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized;
            Vector2 forceDirection = new Vector2(knockbackDirection.x, verticalBoost).normalized;
            rb.AddForce(forceDirection * knockbackStrength, ForceMode2D.Impulse);

            ReduceHealth(2);  

            StartCoroutine(Invulnerability());
        }

        Frog2 enemy2 = collision.gameObject.GetComponent<Frog2>();
        if (enemy2 != null && canBeKnockedBack)
        {
            Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized;
            Vector2 forceDirection = new Vector2(knockbackDirection.x, verticalBoost).normalized;
            rb.AddForce(forceDirection * knockbackStrength, ForceMode2D.Impulse);

            ReduceHealth(2);  

            StartCoroutine(Invulnerability());
        }

        minion Minion = collision.gameObject.GetComponent<minion>();
        if (Minion != null && canBeKnockedBack)
        {
            Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized;
            Vector2 forceDirection = new Vector2(knockbackDirection.x, verticalBoost).normalized;
            rb.AddForce(forceDirection * knockbackStrength, ForceMode2D.Impulse);

            ReduceHealth(2); 

            StartCoroutine(Invulnerability());
        }
    }

    public void AddExternalForceX(float force)
    {
        externalForceX = force;
    }

    public void ReduceHealth(int damage)
    {
        if (!canBeKnockedBack) return;

        currentHealth -= damage;
        healthDisplay.UpdateHealth(currentHealth);

  
        if (hitSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hitSound);
        }

        if (currentHealth <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);  
        }
    }

    IEnumerator Invulnerability()
    {
        canBeKnockedBack = false; 
        animator.SetBool("isHurt", true);
        yield return new WaitForSeconds(.1f);  
        animator.SetBool("isHurt", false);

        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"), true);

        StartCoroutine(BlinkEffect(immunityDuration));
        yield return new WaitForSeconds(immunityDuration);

        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"), false);
        canBeKnockedBack = true;  
        spriteRenderer.enabled = true; 
    }
    public void IncreaseHealth(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth); 
        healthDisplay.UpdateHealth(currentHealth); 
    }

    IEnumerator BlinkEffect(float duration)
    {
        float endTime = Time.time + duration;
        while (Time.time < endTime)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(.1f);
        }
        spriteRenderer.enabled = true; 
    }

}
