using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour {

    public int health;
    public int damage;
    private float timeBtwDamage = 3f;

    public Slider healthBar;
    private Animator anim;
    public bool isDead;
    public GameObject bossHealthUI;
    private Transform playerTransform;
    private bool introStarted = false;
    public GameObject player;

    private void Start()
    {
        anim = GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (!introStarted && playerTransform.position.x > 24)
        {
            anim.SetTrigger("startIntro"); 
            introStarted = true;  
            bossHealthUI.SetActive(true);
        }

        if (health <= 1000) {
            anim.SetTrigger("stageTwo");
        }

        if (health <= 0) {
            anim.SetTrigger("death");
            bossHealthUI.SetActive(false);
        }

        if (timeBtwDamage > 0) {
            timeBtwDamage -= Time.deltaTime;
        }

        if (timeBtwDamage > 0) {
            timeBtwDamage -= Time.deltaTime;
        }

        healthBar.value = health;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isDead)
        {
            if (timeBtwDamage <= 0)
            {
                PlayerKnockback playerKnockback = other.GetComponent<PlayerKnockback>();
                if (playerKnockback != null)
                {
                    playerKnockback.ReduceHealth(damage);
                    timeBtwDamage = 3f;  
                    StartCoroutine(FlashPlayer(3f, 0.1f));
                }
            }
        }
        else if (other.CompareTag("powerwashertip") && !isDead) 
        {
            TakeDamage(20); 
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        healthBar.value = health;
    }

    IEnumerator FlashPlayer(float duration, float interval)
    {
        float endTime = Time.time + duration;
        while (Time.time < endTime)
        {
            TogglePlayerVisibility();
            yield return new WaitForSeconds(interval);
        }
        SetPlayerVisibility(true);
    }

    void TogglePlayerVisibility()
    {
        if (player != null)
        {
            SpriteRenderer spriteRenderer = player.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
                spriteRenderer.enabled = !spriteRenderer.enabled;
        }
    }

    void SetPlayerVisibility(bool visible)
    {
        if (player != null)
        {
            SpriteRenderer spriteRenderer = player.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
                spriteRenderer.enabled = visible;
        }
    }
}
