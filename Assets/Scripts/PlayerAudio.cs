using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerAudio : MonoBehaviour
{
    [SerializeField] AudioClip tokenCollectedSound;
    [SerializeField] AudioClip jumpingSound;
    [SerializeField] AudioClip landingSound;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip checkpointSound;
    [SerializeField] AudioClip victorySound;
    [SerializeField] float soundVolume = 1f;

    AudioSource audioSource;

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

    public void PlayLanding()
    {
        if (landingSound != null)
        {
            audioSource.PlayOneShot(landingSound, soundVolume);
        }
    }

    public void PlayJumping()
    {
        if (jumpingSound != null)
        {
            audioSource.PlayOneShot(jumpingSound, soundVolume);
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
}
