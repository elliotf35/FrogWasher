using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Frog : MonoBehaviour
{
    public float health = 150;
    private PlayerProjectileRender ppr;
    public GameObject pwt;
    private Rigidbody2D rb; // Add a reference to Rigidbody2D component

    void Start()
    {
        ppr = pwt.GetComponent<PlayerProjectileRender>();
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
    }

    void FixedUpdate()
    {
        if (health > 0) // Check if the frog's health is greater than zero before moving
        {
            if (ppr.firing && IsWithin(ppr.secondPoint))
            {
                TakeDamage(1); // Reduce health
            }
        }
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        Debug.Log($"Frog hit! New health: {health}"); // Log health reduction

        // Check for zero or negative health
        if (health <= 0)
        {
            DisableFrog();
        }
    }

    private void DisableFrog()
    {
        BoxCollider2D bc = GetComponent<BoxCollider2D>();
        bc.enabled = false;
        Debug.Log("Frog health is zero, disabling collider.");

        // Increase gravity to make the frog fall quickly
        rb.gravityScale = 1000;

        // Stop frog's movement when health reaches zero
        rb.velocity = Vector2.zero;

        // Start coroutine to destroy the frog after a delay
        StartCoroutine(DestroyAfterDelay(3f));
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Destroy the game object
        Destroy(gameObject);
    }


    private bool IsWithin(Vector2 point)
    {
        Collider2D collider = GetComponent<Collider2D>();
        Vector2 closestPoint = collider.ClosestPoint(point);
        float distance = Vector2.Distance(point, closestPoint);
        return distance < 0.1f; // Use a small but nonzero distance
    }
}
