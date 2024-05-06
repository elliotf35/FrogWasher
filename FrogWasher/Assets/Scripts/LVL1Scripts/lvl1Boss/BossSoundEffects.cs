using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnEnter : StateMachineBehaviour
{
    public AudioClip soundClip;
    private AudioSource audioSource;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!audioSource)
        {
            audioSource = animator.GetComponent<AudioSource>();
        }
        audioSource.PlayOneShot(soundClip);
    }
}
