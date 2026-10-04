using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager;
    [SerializeField] Slider staminaBar;
    [SerializeField] TMP_Text tokenText;

    void Update()
    {
        staminaBar.value = playerManager.CurrentStamina / playerManager.MaxStamina;
        tokenText.text = "Tokens: " + playerManager.CurrentTokens + "/" + playerManager.MaxTokens;
    }
}
