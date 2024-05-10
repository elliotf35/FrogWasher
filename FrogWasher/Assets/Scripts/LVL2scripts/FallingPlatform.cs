using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    private float fallDelay = 2f;
    private float reappearDelay = 7f; // Time after which the platform reappears

    [SerializeField] private Rigidbody2D rb;

    private Vector2 originalPosition; // To store the original position of the platform

    private Collider2D platformCollider;
    private SpriteRenderer spriteRenderer; // To manipulate the platform's sprite

    void Start()
    {
        originalPosition = transform.position;
        platformCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Fall());
        }
    }

    private IEnumerator Fall()
    {
        yield return new WaitForSeconds(fallDelay);
        StartCoroutine(FadeOut()); // Start fading out
        RemoveBoxCollider();
        rb.bodyType = RigidbodyType2D.Dynamic; // Platform starts to fall

        yield return new WaitForSeconds(reappearDelay);

        // Reset platform to original position and state
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.bodyType = RigidbodyType2D.Static;
        transform.position = originalPosition;
        StartCoroutine(FadeIn()); // Start fading in
        RestoreBoxCollider();
    }

    IEnumerator FadeOut()
    {
        float duration = 1f; // Duration of the fade
        float currentTime = 0f;
        while (currentTime < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, currentTime / duration);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0f); // Ensure it's fully transparent
    }

    IEnumerator FadeIn()
    {
        float duration = 1f; // Duration of the fade
        float currentTime = 0f;
        while (currentTime < duration)
        {
            float alpha = Mathf.Lerp(0f, 1f, currentTime / duration);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f); // Ensure it's fully visible
    }

    void RemoveBoxCollider()
    {
        if (platformCollider != null)
        {
            platformCollider.enabled = false;
        }
    }

    void RestoreBoxCollider()
    {
        if (platformCollider != null)
        {
            platformCollider.enabled = true;
        }
    }
}
