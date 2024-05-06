using UnityEngine;
using UnityEngine.UI;
using System.Collections; 

public class Frog2 : MonoBehaviour
{
    public float health = 250;
    public GameObject healthBarPrefab; // Assign the health bar prefab in the inspector
    private GameObject healthBar; // This will be the instantiated health bar
    public Image healthBarForeground; // This will reference the foreground image of the instantiated prefab
    private float maxHealth = 250;
    private PlayerProjectileRender ppr;
    public GameObject pwt;
    public Rigidbody2D rb;
    private float initialHealthBarWidth;
    private bool initialWidthSet = false; 
    private Animator animator;
    public PlayerKnockback playerKnockback;
    [SerializeField] private Transform spawnerTransform; // Make it private or public based on your needs

    // Knockback variables
    public float knockbackStrength = .01f;
    private bool canMove = true;

    void Start()
    {
        ppr = pwt.GetComponent<PlayerProjectileRender>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

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
    }

    private void Knockback(Vector2 damageDirection)
    {
        if (canMove)
        {
            Vector2 knockbackForce = damageDirection.normalized * knockbackStrength;
            rb.AddForce(knockbackForce, ForceMode2D.Impulse);
        }
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
            animator.SetBool("IsDying", true);  
        }

        if (playerKnockback != null)
        {
            playerKnockback.IncreaseHealth(1);  
        }

        RemoveBoxCollider();

        StartCoroutine(RespawnafterDelay(.8f));
    }


    private System.Collections.IEnumerator RespawnafterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (spawnerTransform != null)
        {
            transform.position = spawnerTransform.position;
        }
        transform.position = spawnerTransform.position;
        health = maxHealth;
        animator.SetBool("IsDying", false);
        RestoreBoxCollider();
        SetupHealthBar();
    }

    void RemoveBoxCollider()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }
    }

    void RestoreBoxCollider()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        if (collider != null)
        {
            collider.enabled = true;
        }
    }
    
}
