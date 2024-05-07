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

    void Start()
    {
        // Store the original position of the platform at start
        originalPosition = transform.position;
        platformCollider = GetComponent<Collider2D>();
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
        RemoveBoxCollider();
        rb.bodyType = RigidbodyType2D.Dynamic; // Platform starts to fall

        // Wait for the reappear delay
        yield return new WaitForSeconds(reappearDelay);

        // Reset platform to original position and state
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.bodyType = RigidbodyType2D.Static;
        transform.position = originalPosition;
        RestoreBoxCollider();
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

    void Update()
    {
        
    }
}
