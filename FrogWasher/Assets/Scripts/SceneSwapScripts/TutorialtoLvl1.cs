using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public Animator transitionAnimator;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(TransitionToScene(0));
        }
    }

    IEnumerator TransitionToScene(int sceneIndex)
    {
      
    transitionAnimator.SetTrigger("Start");

    yield return new WaitForSeconds(1f);

    SceneManager.LoadSceneAsync(sceneIndex);

    }
}