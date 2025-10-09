using UnityEngine;
using UnityEngine.UI; // Necesario para controlar Sliders
using TMPro; // Necesario si usas TextMeshPro

public class UIManager : MonoBehaviour
{
    [Header("Referencias de la Interfaz")]
    // Arrastra aquí tus objetos de UI desde la Jerarquía
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;
    public TextMeshProUGUI multiplierText;
    public Slider healthBar;
    public Slider powerUpBar;
    
    // Referencia al ScoreManager para saber cuándo mostrar el combo
    private ScoreManager scoreManager;

    void Start()
    {
        // Buscamos el ScoreManager al empezar
        scoreManager = FindFirstObjectByType<ScoreManager>();
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
            // Solo mostramos el texto del combo si es mayor o igual al umbral
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
            // Solo mostramos el multiplicador si es mayor que 1
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
            // Normalizamos el valor de la vida entre 0 y 1 para el slider
            healthBar.value = (float)currentHealth / maxHealth;
        }
    }

    public void UpdatePowerUp(int currentValue, int maxValue)
    {
        if (powerUpBar != null && maxValue > 0)
        {
            // Normalizamos el valor del poder entre 0 y 1 para el slider
            powerUpBar.value = (float)currentValue / maxValue;
        }
    }
}