using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Frog : MonoBehaviour
{
    public float health = 150;
    private PlayerProjectileRender ppr;
    public GameObject pwt;

    void Start()
    {
        ppr = pwt.GetComponent<PlayerProjectileRender>();
    }

    void FixedUpdate()
    {
        if (ppr.firing && IsWithin(ppr.secondPoint))
        {
            TakeDamage(1); // Reduce health
        }

        if (health <= 0)
        {
            DisableFrog();
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
    }

    private bool IsWithin(Vector2 point)
    {
        Collider2D collider = GetComponent<Collider2D>();
        Vector2 closestPoint = collider.ClosestPoint(point);
        float distance = Vector2.Distance(point, closestPoint);
        return distance < 0.1f; // Use a small but nonzero distance
    }
}
