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

    public GameObject[] minions;

    public GameObject[] superMinions;

    private bool stageTwoTriggered = false;  // To ensure stage two is triggered only once

    public GameObject exitDoor;  
    private SpriteRenderer exitDoorRenderer;  
    private Collider2D exitDoorCollider; 

    void Start()
    {
        animator = GetComponent<Animator>();
        BossPath = GetComponent<BossPath>();  
        BossPath.enabled = false;  // Ensure the script is disabled at start
        bossMusic.SetActive(false);

         if (exitDoor != null)
        {
            exitDoorRenderer = exitDoor.GetComponent<SpriteRenderer>();
            exitDoorCollider = exitDoor.GetComponent<Collider2D>();

            // Initialize door state
            if (exitDoorRenderer != null)
                exitDoorRenderer.color = new Color(1, 1, 1, 0.2f);  
            if (exitDoorCollider != null)
                exitDoorCollider.enabled = false;  // Initially disable the collider
        }
        else
        {
            Debug.LogError("Exit door is not assigned in the Inspector");
        }
    }

    private void Update()
    {
        if (health <= 0) {
            BossPath.canActivateSuperMinions = false;
            DisableSuperMinions();
            bossHealthUI.SetActive(false);
            bossMusic.SetActive(false);
            backgroundMusic.SetActive(true);
            animator.SetBool("IsDead", true);
            BossPath.enabled = false;
            var LightningActivation = GetComponent<LightningActivation>();
            LightningActivation.DeactivateLightningSequence();

        if (exitDoorRenderer != null)
            exitDoorRenderer.color = new Color(1, 1, 1, 1); 
        if (exitDoorCollider != null)
            exitDoorCollider.enabled = true;
        }

        if (health <= 1500 && !stageTwoTriggered) {
            DisableMinions();
            stageTwoTriggered = true;
            animator.SetTrigger("StartIntro2");
            animator.SetBool("StageTwo", true);
            BossPath.StopActivatingMinions();
            BossPath.canActivateSuperMinions = true;
            BossPath.maxSpeed = 20;

            var LightningActivation = GetComponent<LightningActivation>();
            if(LightningActivation != null){
                LightningActivation.ActivateLightningSequence();
            }
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
        if (BossPath != null)
        {
            BossPath.enabled = true;  
        }
    }

    public void ReduceBossHealth()
    {
        health -= 75;
        UpdateHealthUI();
    }

    public void SuperMinionDefeated()
    {
        health -= 100;
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

    private void DisableMinions()
    {
        foreach (GameObject minion in minions)
        {
            Destroy(minion);  
        }
        minions = new GameObject[0];
    }

    private void DisableSuperMinions()
    {
        foreach (GameObject superMinion in superMinions)
        {
            Destroy(superMinion);  
        }
         superMinions = new GameObject[0];
    }
}
