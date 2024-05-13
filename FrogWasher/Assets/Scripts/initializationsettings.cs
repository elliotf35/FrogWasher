using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class initializationsettings : MonoBehaviour
{
    public GameObject player;
    private Vector3 defaultStartPosition;

    void Awake() {
        // Capture the initial start position from the editor placement
        defaultStartPosition = player.transform.position;
        CheckpointManager.SetDefaultCheckpoint(defaultStartPosition);
    }

    void Start()
    {
        Application.targetFrameRate = 100;
        SetPlayerPosition();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetPlayerPosition();
    }

    private void SetPlayerPosition()
    {
        if (CheckpointManager.IsCheckpointActive)
        {
            player.transform.position = CheckpointManager.CurrentCheckpoint;
            Debug.Log("Player position set to checkpoint: " + CheckpointManager.CurrentCheckpoint);
        }
        else
        {
            // Set to the default start position captured at Awake
            player.transform.position = defaultStartPosition;
            Debug.Log("Player position set to default start position: " + defaultStartPosition);
        }
    }
}
