using UnityEngine;

public class BoosterAnimation : MonoBehaviour
{
    [Header("局聪皋捞记 技泼")]
    [SerializeField] private Animator animator;

    [Header("家府 技泼")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip boosterSFX;

    public void PlayAnimation()
    {
        animator.SetTrigger("Booster");

        if(!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(boosterSFX);
        }
    }
}
