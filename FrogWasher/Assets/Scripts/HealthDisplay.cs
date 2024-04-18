using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // This is necessary for working with UI elements like Image

public class HealthDisplay : MonoBehaviour
{
    public Image[] halfHearts;  // Array to store each half-heart image

    public void UpdateHealth(int currentHealth)
    {
        // Ensure each half-heart image is enabled or disabled based on current health
        for (int i = 0; i < halfHearts.Length; i++)
        {
            halfHearts[i].enabled = i < currentHealth;
        }
    }
}
