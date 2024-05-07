using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionlvl2 : MonoBehaviour
{
    public Animator transitionAnimator;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
              StartCoroutine(TransitionToScene(4));
        }
    }

    IEnumerator TransitionToScene(int sceneIndex)
    {
      
    transitionAnimator.SetTrigger("Start");

    yield return new WaitForSeconds(1f);

    SceneManager.LoadSceneAsync(sceneIndex);

    }
}