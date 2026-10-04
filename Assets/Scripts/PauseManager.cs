using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject gameOverMenu;
    [SerializeField] GameObject victoryMenu;
    [SerializeField] GameObject pauseBlur;
    [SerializeField] GameObject staminaBar;
    [SerializeField] TMP_Text victoryTimeText;
    [SerializeField] InputActionReference pauseAction;
    [SerializeField] LevelManager levelManager;
    [SerializeField] PlayerManager playerManager;

    bool isPaused;
    bool isGameOver;
    bool isVictory;

    void Awake()
    {
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        victoryMenu.SetActive(false);
        pauseBlur.SetActive(false);
        staminaBar.SetActive(true);
    }

    void OnEnable()
    {
        pauseAction.action.performed += TogglePause;
        pauseAction.action.Enable();
    }

    void OnDisable()
    {
        pauseAction.action.performed -= TogglePause;
        pauseAction.action.Disable();
        Time.timeScale = 1f;
    }

    void TogglePause(InputAction.CallbackContext context)
    {
        if (isGameOver || isVictory)
        {
            return;
        }

        if (isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    void Pause()
    {
        isPaused = true;
        pauseMenu.SetActive(true);
        pauseBlur.SetActive(true);
        staminaBar.SetActive(false);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        pauseBlur.SetActive(false);
        staminaBar.SetActive(true);
        Time.timeScale = 1f;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LastCheckpoint()
    {
        Resume();
        levelManager.RespawnPlayer(playerManager);
    }

    public void ShowGameOver()
    {
        isGameOver = true;
        isPaused = false;
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(true);
        pauseBlur.SetActive(true);
        staminaBar.SetActive(false);
        Time.timeScale = 0f;
    }

    public void ShowVictory(float elapsedTime)
    {
        isVictory = true;
        isPaused = false;
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        victoryMenu.SetActive(true);
        pauseBlur.SetActive(true);
        staminaBar.SetActive(false);

        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        victoryTimeText.text = "Time: " + minutes.ToString("00") + ":" + seconds.ToString("00");

        Time.timeScale = 0f;
    }

    public void Exit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
