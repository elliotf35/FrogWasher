using UnityEngine;

public class Frog : MonoBehaviour
{
    public float health = 150;
    private PlayerProjectileRender ppr;
    public GameObject pwt;
    private Rigidbody2D rb; // Add a reference to Rigidbody2D component

    private void Start()
    {
        ppr = pwt.GetComponent<PlayerProjectileRender>();
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
    }

    private void FixedUpdate()
    {
        if (health > 0) // Check if the frog's health is greater than zero before moving
        {
            if (ppr.firing && IsWithin(ppr.secondPoint))
            {
                TakeDamage(1); // Reduce health
            }
        }
    }

    private void OnGUI()
    {
        DrawHealthBarAboveFrog();
    }

    private void DrawHealthBarAboveFrog()
    {
        // Calculate position to draw health bar above the frog
        Vector3 healthBarPos = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 1.5f);

        // Set maximum width for the health bar
        float maxHealthBarWidth = 100f; // Adjust this value to your preference

        // Calculate the width of the health bar based on current health
        float barWidth = Mathf.Clamp(health * (maxHealthBarWidth / 150f), 0f, maxHealthBarWidth); // Assuming max health is 150

        // Set color of background health bar
        GUI.color = new Color(1f, 1f, 1f, 1f); // White with alpha 1 (fully opaque)

        // Draw background of health bar
        GUI.Box(new Rect(healthBarPos.x - maxHealthBarWidth / 2, Screen.height - healthBarPos.y - 20, maxHealthBarWidth, 20), "");

        // Set color of health bar
        GUI.color = new Color(1f, 0f, 0f, 1f); // Red with alpha 1 (fully opaque)

        // Draw foreground of health bar
        GUI.Box(new Rect(healthBarPos.x - maxHealthBarWidth / 2, Screen.height - healthBarPos.y - 20, barWidth, 20), "");
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

    private System.Collections.IEnumerator DestroyAfterDelay(float delay)
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
