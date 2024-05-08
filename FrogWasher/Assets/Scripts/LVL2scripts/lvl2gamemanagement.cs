using UnityEngine;
using UnityEngine.UI;

public class initializationsettings2 : MonoBehaviour
{
    public static initializationsettings2 Instance; 
    public int totalFrogsDefeated = 0; 
    public Text respawnCounterText; 
    public GameObject fakeDoor; // Reference to the non-collidable door GameObject
    public GameObject realDoor; 
    private bool doorOpened = false;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instances
            return;
        }
        Instance = this;
        // DontDestroyOnLoad(gameObject); // Removed to allow the object to be destroyed on scene load
    }
    void Start()
    {
        // Ensure door references are properly set up
        if (fakeDoor != null)
            fakeDoor.SetActive(true); // Show the fake door initially
        if (realDoor != null)
            realDoor.SetActive(false); // Hide the real door initially

        Application.targetFrameRate = 100;

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
        if (fakeDoor != null)
            fakeDoor.SetActive(false); // Deactivate the fake door
        if (realDoor != null)
            realDoor.SetActive(true); // Activate the real door

        doorOpened = true; // Prevent reopening the door again
    }
}
