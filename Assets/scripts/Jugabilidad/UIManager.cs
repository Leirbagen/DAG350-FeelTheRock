using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;
using System.Collections.Generic;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [Header("Referencias de la Interfaz")]
    public GameObject gameOverPanel;
    public GameObject pausePanel;
    public GameObject finishPanel;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI scoreTextPause;
    public TextMeshProUGUI scoreTextFinish;
    public TextMeshProUGUI comboText;
    public TextMeshProUGUI multiplierText;
    [Header("TipoDeHit")]
    public TextMeshProUGUI bienTexto;
    public TextMeshProUGUI perfectoTexto;
    public TextMeshProUGUI falloTexto;
    public Slider healthBar;
    public Slider powerUpBar;
    public StarUI starUI;
    public StarUI starUIPause;
    public StarUI starUIFinish;
    public GameManager gameManager;

    private Coroutine bienCoroutine;
    private Coroutine perfectoCoroutine;
    private Coroutine falloCoroutine;



    private ScoreManager scoreManager;
    private int star1Threshold;
    private int star2Threshold;
    private int star3Threshold;

    private void Awake()
    {
        scoreManager = FindFirstObjectByType<ScoreManager>();
        gameManager = FindFirstObjectByType<GameManager>();
    }
    void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

    }
    public void UpdateStarsUI()
    {
        if (gameManager == null || scoreManager == null) return;
        star1Threshold = gameManager.star1Threshold;
        star2Threshold = gameManager.star2Threshold;
        star3Threshold = gameManager.star3Threshold;
        if (star1Threshold == 0 && star2Threshold == 0 && star3Threshold == 0) return;
        int stars = gameManager.EvaluateStars(scoreManager.currentScore, star1Threshold, star2Threshold, star3Threshold);
        starUI?.SetStars(stars);
        starUIPause?.SetStars(stars);
        starUIFinish?.SetStars(stars);
    }
    public void ShowGameOverPanel(bool show)
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(show);
    }
    public void ShowPausePanel(bool show)
    {
        if (pausePanel != null)
            pausePanel.SetActive(show);
            if (show) UpdateStarsUI(); 
            
    }
    public void ShowFinishPanel(bool show)
    {
        if(finishPanel != null)
            finishPanel.SetActive(show);
            if (show) UpdateStarsUI();
    }
    public void ShowHitText(string tipo)
    {
        switch (tipo)
        {
            case "Bien":
                if (bienCoroutine != null)
                    StopCoroutine(bienCoroutine);
                bienCoroutine = StartCoroutine(MostrarBien());
                break;

            case "Perfecto":
                if (perfectoCoroutine != null)
                    StopCoroutine(perfectoCoroutine);
                perfectoCoroutine = StartCoroutine(MostrarPerfecto());
                break;
        }
    }

    public void restartStarts()
    {
        starUI.HideAll();
        starUIPause.HideAll();
        starUIFinish.HideAll();
    }

    public void showMissHit()
    {
        if (falloCoroutine != null)
            StopCoroutine(falloCoroutine);
        falloCoroutine = StartCoroutine(MostrarFallo());
    }



    public void UpdateScore(int score)
    {
        if (scoreText != null)
            scoreText.text = "PUNTAJE: " + score;
        if (scoreTextPause != null)
            scoreTextPause.text = ""+score;
        if (scoreTextFinish != null)   
            scoreTextFinish.text = "" + score;
        UpdateStarsUI();
    }

    public void UpdateCombo(int combo)
    {
        if (comboText != null && scoreManager != null)
        {
            if (combo >= scoreManager.comboToShowThreshold)
            {
                comboText.gameObject.SetActive(true);
                comboText.text = ""+combo;
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
                multiplierText.text = "X" + multiplier;
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
        }
    }

    private IEnumerator MostrarBien()
    {
        bienTexto.gameObject.SetActive(true);
        bienTexto.alpha = 1f;
        bienTexto.transform.localScale = Vector3.one * 1.3f;

        // Pequeño "pop"
        float t = 0f;
        while (t < 0.15f)
        {
            t += Time.deltaTime;
            bienTexto.transform.localScale = Vector3.Lerp(Vector3.one * 1.3f, Vector3.one, t / 0.15f);
            yield return null;
        }

        // Mantiene visible un momento
        yield return new WaitForSeconds(0.4f);

        // Fade out suave
        float fade = 0.4f;
        float elapsed = 0f;
        Color c = bienTexto.color;
        while (elapsed < fade)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, elapsed / fade);
            bienTexto.color = c;
            yield return null;
        }

        bienTexto.gameObject.SetActive(false);
    }

    private IEnumerator MostrarPerfecto()
    {
        perfectoTexto.gameObject.SetActive(true);
        perfectoTexto.alpha = 1f;
        perfectoTexto.transform.localScale = Vector3.one * 1.4f;

        // Pop animado
        float t = 0f;
        while (t < 0.15f)
        {
            t += Time.deltaTime;
            perfectoTexto.transform.localScale = Vector3.Lerp(Vector3.one * 1.4f, Vector3.one, t / 0.15f);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        // Fade out
        float fade = 0.4f;
        float elapsed = 0f;
        Color c = perfectoTexto.color;
        while (elapsed < fade)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, elapsed / fade);
            perfectoTexto.color = c;
            yield return null;
        }

        perfectoTexto.gameObject.SetActive(false);
    }
    private IEnumerator MostrarFallo()
    {
        falloTexto.gameObject.SetActive(true);
        falloTexto.alpha = 1f;
        falloTexto.transform.localScale = Vector3.one * 1.4f;

        // Pop animado
        float t = 0f;
        while (t < 0.15f)
        {
            t += Time.deltaTime;
            falloTexto.transform.localScale = Vector3.Lerp(Vector3.one * 1.4f, Vector3.one, t / 0.15f);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        // Fade out
        float fade = 0.4f;
        float elapsed = 0f;
        Color c = falloTexto.color;
        while (elapsed < fade)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, elapsed / fade);
            falloTexto.color = c;
            yield return null;
        }

        falloTexto.gameObject.SetActive(false);
    }
}