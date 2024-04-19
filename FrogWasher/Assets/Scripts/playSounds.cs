using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectBehaviour : StateMachineBehaviour
{
    public AudioClip soundClip;  // The AudioClip to play

    // OnStateEnter is called right before this state is entered
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AudioSource audioSource = animator.GetComponent<AudioSource>();
        if (audioSource && soundClip)
        {
            audioSource.clip = soundClip;
            audioSource.Play();
        }
    }

    // OnStateExit is called when leaving the state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AudioSource audioSource = animator.GetComponent<AudioSource>();
        if (audioSource)
        {
            audioSource.Stop();  // Stop the audio source when exiting the state
        }
    }
}
