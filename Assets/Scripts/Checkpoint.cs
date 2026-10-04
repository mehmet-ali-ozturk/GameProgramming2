using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] LevelManager levelManager;
    [SerializeField] Transform respawnPoint;
    [SerializeField] int checkpointNumber = 1;

    void OnTriggerEnter(Collider other)
    {
        PlayerManager player = other.GetComponentInParent<PlayerManager>();

        if (player != null && levelManager.SetCheckpoint(respawnPoint, checkpointNumber))
        {
            PlayerAudio playerAudio = player.GetComponent<PlayerAudio>();

            if (playerAudio != null)
            {
                playerAudio.PlayCheckpoint();
            }
        }
    }
}
