using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static Vector3 CurrentCheckpoint; // Store the last checkpoint position
    public static bool IsCheckpointActive = false; // Flag to check if checkpoint is active

    public static void SetDefaultCheckpoint(Vector3 defaultPosition)
    {
        if (!IsCheckpointActive) // Only set if no checkpoint has been activated
            CurrentCheckpoint = defaultPosition;
    }

    public static void SetCheckpoint(Vector3 checkpointPosition)
    {
        CurrentCheckpoint = checkpointPosition;
        IsCheckpointActive = true;
    }

    public static void ClearCheckpoint()
    {
        IsCheckpointActive = false;
    }

    // Add this method to reset the checkpoint data
    public static void ResetToDefaultCheckpoint()
    {
        IsCheckpointActive = false; // Ensure that the default checkpoint will be used upon restart
    }
}
