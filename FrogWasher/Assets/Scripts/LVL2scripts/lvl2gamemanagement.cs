using UnityEngine;
using UnityEngine.UI;

public class initializationsettings2 : MonoBehaviour
{
    public static initializationsettings2 Instance; 
    public int totalFrogsDefeated = 0; 
    public Text respawnCounterText; 
    public GameObject door; // Reference to the door GameObject
    private BoxCollider2D doorCollider;
    private SpriteRenderer doorSpriteRenderer;
    private bool doorOpened = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); 
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); 
    }

    void Start()
    {
        // Ensure door references are properly set up
        if (door != null)
        {
            doorCollider = door.GetComponent<BoxCollider2D>();
            doorSpriteRenderer = door.GetComponent<SpriteRenderer>();
            // Initial setup for disabled door
            if (doorCollider != null)
                doorCollider.enabled = false;
            if (doorSpriteRenderer != null)
                doorSpriteRenderer.color = new Color(1, 1, 1, .6f); // Transparent initially
        }
    }

    public void IncrementFrogCount()
    {
        totalFrogsDefeated++;
        UpdateRespawnText();

        if (!doorOpened && totalFrogsDefeated >= 10)
        {
            OpenDoor();
        }
    }

    private void UpdateRespawnText()
    {
        if (respawnCounterText != null)
        {
            respawnCounterText.alignment = TextAnchor.MiddleCenter; // Ensures centered text alignment
            respawnCounterText.text = $"Defeat 10 Angel Frogs To Open Door!\n{totalFrogsDefeated}/10";
        }
    }

    private void OpenDoor()
    {
        if (doorCollider != null)
            doorCollider.enabled = true; // Activate the door collider

        if (doorSpriteRenderer != null)
            doorSpriteRenderer.color = new Color(1, 1, 1, 1); // Set alpha to 1

        doorOpened = true; // Prevent reopening the door again
    }
}
