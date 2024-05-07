using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Frog2 : MonoBehaviour
{
    public float health = 250;
    public GameObject healthBarPrefab;
    private GameObject healthBar;
    public Image healthBarForeground;
    private float maxHealth = 250;
    private PlayerProjectileRender ppr;
    public GameObject pwt;
    public Rigidbody2D rb;
    private float initialHealthBarWidth;
    private bool initialWidthSet = false;
    private Animator animator;
    public PlayerKnockback playerKnockback;
    [SerializeField] private Transform spawnerTransform;

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

    private void UpdateHealthBar()
    {
        if (healthBarForeground != null)
        {
            float healthRatio = Mathf.Clamp(health / maxHealth, 0f, 1f);
            healthBarForeground.rectTransform.sizeDelta = new Vector2(initialHealthBarWidth * healthRatio, healthBarForeground.rectTransform.sizeDelta.y);
        }
    }

    private void DisableFrog()
    {
        animator.SetBool("IsDying", true);
        if (playerKnockback != null)
        {
            playerKnockback.IncreaseHealth(1);
        }
        initializationsettings2.Instance.IncrementFrogCount(); 
        RemoveBoxCollider();
        StartCoroutine(RespawnafterDelay(0.8f));
    }

    private IEnumerator RespawnafterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (spawnerTransform != null)
        {
            transform.position = spawnerTransform.position;
        }
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
