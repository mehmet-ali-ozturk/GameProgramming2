using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] Transform startingRespawnPoint;
    [SerializeField] PauseManager pauseManager;

    Transform currentRespawnPoint;
    int currentCheckpointNumber;
    float elapsedTime;
    bool levelEnded;

    void Awake()
    {
        currentRespawnPoint = startingRespawnPoint;
    }

    void Update()
    {
        if (!levelEnded)
        {
            elapsedTime += Time.deltaTime;
        }
    }

    public bool SetCheckpoint(Transform checkpoint, int checkpointNumber)
    {
        if (checkpointNumber <= currentCheckpointNumber)
        {
            return false;
        }

        currentRespawnPoint = checkpoint;
        currentCheckpointNumber = checkpointNumber;
        Debug.Log("Checkpoint " + checkpointNumber + " reached");
        return true;
    }

    public void RespawnPlayer(PlayerManager player)
    {
        player.Respawn(
            currentRespawnPoint.position,
            currentRespawnPoint.rotation
        );
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameOver()
    {
        levelEnded = true;
        pauseManager.ShowGameOver();
    }

    public bool Win()
    {
        if (levelEnded)
        {
            return false;
        }

        levelEnded = true;
        pauseManager.ShowVictory(elapsedTime);
        return true;
    }
}
