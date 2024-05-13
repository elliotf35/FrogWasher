using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    public SpriteRenderer checkpointSprite;  // Ensure this is public or [SerializeField] private
    public float yOffset = 0.5f;  // Offset to adjust the spawn height above the ground

    private AudioSource[] audioSources;
    public AudioClip signSound;

    private bool hasInteracted = false;  // Flag to track whether interaction has occurred

    void Start()
    {
        audioSources = GetComponents<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasInteracted) // Check if player tag and interaction hasn't occurred
        {
            // Set checkpoint with an offset in the y-coordinate
            Vector3 newPosition = new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z);
            CheckpointManager.SetCheckpoint(newPosition);
            
            // Check if the SpriteRenderer is assigned
            if (checkpointSprite != null && audioSources.Length > 0)
            {
                // Change the sorting order
                checkpointSprite.sortingOrder = 1;  
                AudioSource audioSource = audioSources[0];
                audioSource.PlayOneShot(signSound);
            }

            hasInteracted = true; // Set the flag to true after interaction
        }
    }
}
