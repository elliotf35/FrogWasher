using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneResetter2D : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the colliding object has the tag "Respawn"
        if (other.gameObject.CompareTag("Respawn"))
        {
            // Reset the current scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
