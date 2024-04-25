using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour {

    public int health;
    public int damage;
    private float timeBtwDamage = 1.5f;

    public Slider healthBar;
    private Animator anim;
    public bool isDead;

    private Transform playerTransform;
    private bool introStarted = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (!introStarted && playerTransform.position.x > 24)
        {
            anim.SetTrigger("startIntro"); 
            introStarted = true;  
        }

        if (health <= 1000) {
            anim.SetTrigger("stageTwo");
        }

        if (health <= 0) {
            anim.SetTrigger("death");
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
                    timeBtwDamage = 1.5f;  
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
}
