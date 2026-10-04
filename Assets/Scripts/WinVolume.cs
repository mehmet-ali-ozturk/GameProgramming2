using UnityEngine;

public class WinVolume : MonoBehaviour
{
    [SerializeField] LevelManager levelManager;

    void OnTriggerEnter(Collider other)
    {
        PlayerManager player = other.GetComponentInParent<PlayerManager>();

        if (player != null && levelManager.Win())
        {
            PlayerAudio playerAudio = player.GetComponent<PlayerAudio>();

            if (playerAudio != null)
            {
                playerAudio.PlayVictory();
            }
        }
    }
}
