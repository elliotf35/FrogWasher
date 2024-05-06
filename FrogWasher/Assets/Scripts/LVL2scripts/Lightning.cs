using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class RandomTriggerAnimator : MonoBehaviour
{
    public Animator animator;
    public float minTime = 2.0f;
    public float maxTime = 5.0f;

    void Start()
    {
        StartCoroutine(TriggerRandomly());
    }

    IEnumerator TriggerRandomly()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minTime, maxTime));
            animator.SetTrigger("RandomTrigger");
        }
    }
}
