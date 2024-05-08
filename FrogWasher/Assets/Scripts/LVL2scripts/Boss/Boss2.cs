using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss2 : MonoBehaviour
{
    public int health;
    private Animator animator;
    private bool introStarted = false;
    public Slider healthBar;
    public GameObject bossHealthUI;
    private BossPath BossPath;  

    public GameObject backgroundMusic;
    public GameObject bossMusic;


    void Start()
    {
        animator = GetComponent<Animator>();
        BossPath = GetComponent<BossPath>();  // Get the ChasingEnemy script component
        BossPath.enabled = false;  // Ensure the script is disabled at start
        bossMusic.SetActive(false);
    }

    private void Update()
    {
        if (health <= 0) {
            bossHealthUI.SetActive(false);
            bossMusic.SetActive(false);
            backgroundMusic.SetActive(true);
        }

        healthBar.value = health;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !introStarted)
        {
            introStarted = true; 
            animator.SetTrigger("StartIntro");
            bossHealthUI.SetActive(true);
            SwitchMusic();
        }
    }

    public void EnableFlying()
    {
        animator.SetBool("IsFlying", true);
        if (BossPath!= null)
        {
           BossPath.enabled = true;  
        }
    }

    public void ReduceBossHealth()
    {
        health -= 75;
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (healthBar != null)
        {
            healthBar.value = (float)health / healthBar.maxValue;
        }
    }

    private void SwitchMusic(){
        backgroundMusic.SetActive(false);
        bossMusic.SetActive(true);
    }
}
