using UnityEngine;
using UnityEngine.UI;

public class Frog : MonoBehaviour
{
    public float health = 370;
    public GameObject healthBarPrefab; // Assign the health bar prefab in the inspector
    private GameObject healthBar; // This will be the instantiated health bar
    public Image healthBarForeground; // This will reference the foreground image of the instantiated prefab
    private float maxHealth = 400;
    private PlayerProjectileRender ppr;
    public GameObject pwt;
    private Rigidbody2D rb;
    private float initialHealthBarWidth;
    private bool initialWidthSet = false; // Flag to check if initial width has been set

    void Start()
    {
        ppr = pwt.GetComponent<PlayerProjectileRender>();
        rb = GetComponent<Rigidbody2D>();

        healthBar = Instantiate(healthBarPrefab, transform.position, Quaternion.identity);
        healthBar.transform.SetParent(transform, false);
        healthBar.transform.localPosition = new Vector3(0, 0.15f, 0);
        healthBar.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

        healthBarForeground = healthBar.GetComponentInChildren<Image>();
    }

    void Update()
    {
        // Set initial width on the first Update call after instantiation
        if (!initialWidthSet && healthBarForeground != null)
        {
            initialHealthBarWidth = healthBarForeground.rectTransform.sizeDelta.x;
            initialWidthSet = true;
            Debug.Log("Initial Health Bar Width Set: " + initialHealthBarWidth);
        }
    }

    public void TakeDamage(float damageAmount)
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
    }

    private void UpdateHealthBar()
    {
        if (healthBarForeground != null)
        {
            // Ensure health does not exceed maxHealth
            if (health > maxHealth) {
                health = maxHealth;
            }

            // Calculate health ratio and clamp it to ensure it's never more than 1
            float healthRatio = Mathf.Clamp(health / maxHealth, 0f, 1f);

            // Update the width of the health bar
            healthBarForeground.rectTransform.sizeDelta = new Vector2(initialHealthBarWidth * healthRatio, healthBarForeground.rectTransform.sizeDelta.y);;
        }
    }


    private void DisableFrog()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        rb.gravityScale = 40;
        rb.velocity = Vector2.zero;
        StartCoroutine(DestroyAfterDelay(3f));
    }

    private System.Collections.IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(healthBar); // Also destroy the health bar
        Destroy(gameObject);
    }
}
