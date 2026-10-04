using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] float maxHealth = 100f;
    [SerializeField] float maxStamina = 100f;
    [SerializeField] float staminaRecoveryPerSecond = 20f;
    [SerializeField] float staminaRecoveryDelay = 1f;
    [SerializeField] int maxTokens = 3;
    [SerializeField] LevelManager levelManager;

    float currentHealth;
    float currentStamina;
    float healthLogTimer;
    float staminaRecoveryTimer;
    int currentTokens;
    Rigidbody body;
    PlayerController playerController;
    PlayerAudio playerAudio;

    public float CurrentHealth => currentHealth;
    public float CurrentStamina => currentStamina;
    public float MaxStamina => maxStamina;
    public int CurrentTokens => currentTokens;
    public int MaxTokens => maxTokens;
    public bool IsDead => currentHealth <= 0f;

    void Awake()
    {
        body = GetComponent<Rigidbody>();
        playerController = GetComponent<PlayerController>();
        playerAudio = GetComponent<PlayerAudio>();
        currentHealth = maxHealth;
        currentStamina = maxStamina;
    }

    void Update()
    {
        healthLogTimer += Time.deltaTime;

        if (healthLogTimer >= 1f)
        {
            Debug.Log("Player Health: " + currentHealth + " | Stamina: " + currentStamina);
            healthLogTimer = 0f;
        }

        if (staminaRecoveryTimer > 0f)
        {
            staminaRecoveryTimer -= Time.deltaTime;
        }
        else
        {
            currentStamina = Mathf.Min(currentStamina + staminaRecoveryPerSecond * Time.deltaTime, maxStamina);
        }
    }

    public bool UseStamina(float amount)
    {
        if (amount <= 0f)
        {
            return true;
        }

        if (currentStamina < amount)
        {
            return false;
        }

        currentStamina -= amount;
        staminaRecoveryTimer = staminaRecoveryDelay;
        return true;
    }

    public bool AddToken()
    {
        if (currentTokens >= maxTokens)
        {
            return false;
        }

        currentTokens++;
        Debug.Log("Token collected: " + currentTokens + "/" + maxTokens);
        return true;
    }

    bool SpendToken()
    {
        if (currentTokens <= 0)
        {
            return false;
        }

        currentTokens--;
        Debug.Log("Token spent: " + currentTokens + "/" + maxTokens);
        return true;
    }

    public void TakeDamage(float amount)
    {
        if (IsDead || amount <= 0f)
        {
            return;
        }

        currentHealth = Mathf.Max(currentHealth - amount, 0f);
        Debug.Log("Player took " + amount + " damage");

        if (IsDead)
        {
            Die();
        }
    }

    public void Kill()
    {
        if (IsDead)
        {
            return;
        }

        currentHealth = 0f;
        Die();
    }

    public void Respawn(Vector3 position, Quaternion rotation)
    {
        body.position = position;
        body.rotation = rotation;
        body.linearVelocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;

        playerController.ResetMovement();

        currentHealth = maxHealth;
        currentStamina = maxStamina;
    }

    void Die()
    {
        playerAudio.PlayDeath();

        if (SpendToken())
        {
            levelManager.RespawnPlayer(this);
        }
        else
        {
            levelManager.GameOver();
        }
    }
}
