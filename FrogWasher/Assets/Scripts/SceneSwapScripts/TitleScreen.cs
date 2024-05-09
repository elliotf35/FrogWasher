using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuToGame : MonoBehaviour
{
    public Animator transitionAnimator;
    public GameObject currentCanvas; // Current active canvas
    public GameObject levelsCanvas; // Canvas to activate

    public void PlayTutorial(){
        StartCoroutine(TransitionToScene(1));
    }

    public void PlayGame(){
        StartCoroutine(TransitionToScene(2));
    }

    public void Level1(){
        StartCoroutine(TransitionToScene(2));
    }

    public void Level1Boss(){
        StartCoroutine(TransitionToScene(3));
    }

    public void Level2(){
        StartCoroutine(TransitionToScene(4));
    }

        public void Level2Boss(){
        StartCoroutine(TransitionToScene(5));
    }

    IEnumerator TransitionToScene(int sceneIndex)
    {
        transitionAnimator.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadSceneAsync(sceneIndex);
    }

    public void SwitchToLevelsCanvas() {
        if (currentCanvas != null)
            currentCanvas.SetActive(false); // Deactivate the current canvas
        if (levelsCanvas != null)
            levelsCanvas.SetActive(true); // Activate the levels canvas
    }

    public void SwitchToMenuCanvas() {
        if (currentCanvas != null)
            currentCanvas.SetActive(true); // Deactivate the current canvas
        if (levelsCanvas != null)
            levelsCanvas.SetActive(false); // Activate the levels canvas
    }

    public void QuitGame(){
        Application.Quit();
    }
}
