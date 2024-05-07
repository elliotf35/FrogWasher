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
    private MovingPlatform MovingPlatform;  

    void Start()
    {
        animator = GetComponent<Animator>();
        MovingPlatform = GetComponent<MovingPlatform>();  // Get the ChasingEnemy script component
        MovingPlatform.enabled = false;  // Ensure the script is disabled at start
    }

    private void Update()
    {
        if (health <= 0) {
            bossHealthUI.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !introStarted)
        {
            introStarted = true; 
            animator.SetTrigger("StartIntro");
            bossHealthUI.SetActive(true);
        }
    }

    public void EnableFlying()
    {
        animator.SetBool("IsFlying", true);
        if (MovingPlatform != null)
        {
            MovingPlatform.enabled = true;  
        }
    }
}
