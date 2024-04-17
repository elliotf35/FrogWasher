using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class initializationsettings : MonoBehaviour
{
    void Awake()
    {
        if (FindObjectsOfType<initializationsettings>().Length > 1)
        {
            Destroy(gameObject);  // Destroys the current object if another one exists
        }
        else
        {
            DontDestroyOnLoad(gameObject);  // Keeps the original GameObject alive across scenes
        }
    }

    void Start() {
        Application.targetFrameRate = 100;
    }

    void Update()
    {
        // This function can remain empty if not needed
    }
}
