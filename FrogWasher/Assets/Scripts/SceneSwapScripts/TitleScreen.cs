using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuToGame : MonoBehaviour
{
    public Animator transitionAnimator;

    public void PlayTutorial(){
        StartCoroutine(TransitionToScene(1));
    }

    public void PlayGame(){
        StartCoroutine(TransitionToScene(2));
    }

    IEnumerator TransitionToScene(int sceneIndex)
    {
      
    transitionAnimator.SetTrigger("Start");

    yield return new WaitForSeconds(1f);

    SceneManager.LoadSceneAsync(sceneIndex);

    }

    public void QuitGame(){
        Application.Quit();
    }
}
