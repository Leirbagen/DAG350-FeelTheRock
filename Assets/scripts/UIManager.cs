using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;

public class UIManager : MonoBehaviour
{
    [Header("Referencias de la Interfaz")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;
    public TextMeshProUGUI multiplierText;
    public Slider healthBar;
    public Slider powerUpBar;
    
    private ScoreManager scoreManager;

    void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        scoreManager = FindFirstObjectByType<ScoreManager>();
    }

    public void ShowGameOverPanel(bool show)
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(show);
    }

    public void UpdateScore(int score)
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    public void UpdateCombo(int combo)
    {
        if (comboText != null && scoreManager != null)
        {
            if (combo >= scoreManager.comboToShowThreshold)
            {
                comboText.gameObject.SetActive(true);
                comboText.text = "Combo x" + combo;
            }
            else
            {
                comboText.gameObject.SetActive(false);
            }
        }
    }

    public void UpdateMultiplier(int multiplier)
    {
        if (multiplierText != null)
        {
            if (multiplier > 1)
            {
                multiplierText.gameObject.SetActive(true);
                multiplierText.text = "x" + multiplier;
            }
            else
            {
                multiplierText.gameObject.SetActive(false);
            }
        }
    }

    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        if (healthBar != null)
        {
            float healthValue = (float)currentHealth / maxHealth;
            healthBar.value = healthValue;

        }
    }

    public void UpdatePowerUp(int currentValue, int maxValue)
    {
        if (powerUpBar != null && maxValue > 0)
        {
            float powerUpValue = (float)currentValue / maxValue;
            powerUpBar.value = powerUpValue;

            // --- MENSAJE DE DIAGNÓSTICO ---
            //Debug.Log($"<color=lime>UI MANAGER: Actualizando PowerUpBar. currentValue={currentValue}, maxValue={maxValue}. Valor final: {powerUpValue}</color>");
        }
    }
}