using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerAudio : MonoBehaviour
{
    [SerializeField] AudioClip tokenCollectedSound;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip checkpointSound;
    [SerializeField] AudioClip victorySound;
    [SerializeField] AudioClip walkingSound;
    [SerializeField] AudioClip shootingSound;
    [SerializeField] float walkingStepInterval = 0.5f;
    [SerializeField] float sprintingStepInterval = 0.3f;
    [SerializeField] float soundVolume = 1f;

    AudioSource audioSource;
    float footstepTimer;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayTokenCollected()
    {
        if (tokenCollectedSound != null)
        {
            audioSource.PlayOneShot(tokenCollectedSound, soundVolume);
        }
    }

    public void PlayDeath()
    {
        if (deathSound != null)
        {
            audioSource.PlayOneShot(deathSound, soundVolume);
        }
    }

    public void PlayCheckpoint()
    {
        if (checkpointSound != null)
        {
            audioSource.PlayOneShot(checkpointSound, soundVolume);
        }
    }

    public void PlayVictory()
    {
        if (victorySound != null)
        {
            audioSource.PlayOneShot(victorySound, soundVolume);
        }
    }

    public void PlayShooting()
    {
        if (shootingSound != null)
        {
            audioSource.PlayOneShot(shootingSound, soundVolume);
        }
    }

    public void UpdateFootsteps(bool isWalking, bool isSprinting)
    {
        if (!isWalking)
        {
            footstepTimer = 0f;
            return;
        }

        footstepTimer -= Time.fixedDeltaTime;

        if (footstepTimer <= 0f && walkingSound != null)
        {
            audioSource.PlayOneShot(walkingSound, soundVolume);
            footstepTimer = isSprinting ? sprintingStepInterval : walkingStepInterval;
        }
    }
}
