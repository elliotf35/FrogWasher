using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseButton : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    public void Pause(){
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        AudioListener.pause = true; 
    }

    public void Home(){
        CheckpointManager.ResetToDefaultCheckpoint();
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
        AudioListener.pause = false; 
    }

    public void Resume(){
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        AudioListener.pause = false; 
    }

    public void Restart(){
        CheckpointManager.ResetToDefaultCheckpoint();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
        AudioListener.pause = false; 
    }
}
