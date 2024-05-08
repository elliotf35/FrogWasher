using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningActivation : MonoBehaviour
{
    public GameObject[] warnings;
    public GameObject[] lightnings;
    public float warningDuration = 1.5f;
    public Transform playerTransform;  // Assign this in the Inspector
    public float activationDistance = 5.0f;  // Distance within which the lightning activates
    public AudioSource specificAudioSource; 

    private bool isActive = false;

    void Start()
    {
        foreach (var warning in warnings)
            warning.SetActive(false);
        foreach (var lightning in lightnings)
            lightning.SetActive(false);
    }

    public void ActivateLightningSequence()
    {
        isActive = true;
        StartCoroutine(ActivateStrikes());
    }

    IEnumerator ActivateStrikes()
    {
        if (!isActive) yield break;  // Safety check to ensure we're allowed to run this sequence

        // Loop continuously through all warnings and lightnings as long as isActive is true
        while (isActive)
        {
            for (int i = 0; i < warnings.Length; i++)
            {
                // Only activate if the player is within range
                if (Vector3.Distance(playerTransform.position, warnings[i].transform.position) <= activationDistance)
                {
                    warnings[i].SetActive(true);
                    yield return new WaitForSeconds(warningDuration);
                    warnings[i].SetActive(false);
                    lightnings[i].SetActive(true);

                    if (specificAudioSource != null)
                    {
                        specificAudioSource.Play();
                    }
                    yield return new WaitForSeconds(0.8f);
                    lightnings[i].SetActive(false);

                    // Additional wait after lightning deactivates before next loop iteration
                    yield return new WaitForSeconds(1f);
                }
                else
                {
                    // If player is not close, wait a bit before checking the next one
                    yield return new WaitForSeconds(0.05f);
                }
            }
        }
    }

    public void DeactivateLightningSequence()
    {
        StopAllCoroutines();  // To stop all ongoing sequences
        isActive = false;

        // Deactivate all active elements
        foreach (var warning in warnings) warning.SetActive(false);
        foreach (var lightning in lightnings) lightning.SetActive(false);
    }
}
