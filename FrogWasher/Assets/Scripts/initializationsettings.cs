using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class initializationsettings : MonoBehaviour
{
    void Awake()
    {
        if (FindObjectsOfType<initializationsettings>().Length > 1)
        {
            Destroy(gameObject); 
        }
        else
        {
            DontDestroyOnLoad(gameObject);  
        }
    }

    void Start() {
        Application.targetFrameRate = 100;
    }

}
