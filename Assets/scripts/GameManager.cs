using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Referencias a Managers (Arrastrar desde la Jerarquía)")]
    public ScoreManager scoreManager;
    public HealthManager healthManager;
    public PowerUpManager powerUpManager;
    public UIManager uiManager;
    public AudioManager audioManager;
    public InputManager inputManager;
    public bool isGameOver = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //Debug.Log("<color=purple>GAME MANAGER: Instancia creada correctamente.</color>");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Time.timeScale = 1f;

        if (uiManager == null || scoreManager == null || healthManager == null || powerUpManager == null || audioManager == null)
        {
            //Debug.LogError("ERROR: Faltan referencias de Managers en el GameManager! Arrástralos en el Inspector.");
            return;
        }

        uiManager.UpdateScore(scoreManager.currentScore);
        uiManager.UpdateHealth(healthManager.currentHealth, healthManager.maxHealth);
        uiManager.UpdatePowerUp(powerUpManager.currentPowerUpValue, powerUpManager.comboToFill);
        uiManager.UpdateCombo(scoreManager.currentCombo);
        uiManager.UpdateMultiplier(scoreManager.currentMultiplier);
    }

    public void OnNoteHit(int laneID)
    {
        // Mensaje para confirmar que la llamada fue recibida
        Debug.Log($"<color=#006400>GAME MANAGER: OnNoteHit RECIBIDO para carril {laneID}</color>");

        audioManager.HitNote(laneID);
        scoreManager.AddScore();
        healthManager.OnNoteHit();
        powerUpManager.OnNoteHit(scoreManager.currentCombo);

        UpdateAllUI();
    }

    public void OnNoteMiss(int laneID)
    {
        // Mensaje para confirmar que la llamada fue recibida
        //Debug.Log($"<color=#8B0000>GAME MANAGER: OnNoteMiss RECIBIDO para carril {laneID}</color>");

        audioManager.MissNote(laneID);
        audioManager.PlayMissSound();
        scoreManager.BreakCombo();
        healthManager.OnNoteMiss();
        powerUpManager.OnNoteMiss();

        UpdateAllUI();
    }
    
    public void OnMultiplierChanged(int newMultiplier)
    {
        scoreManager.SetMultiplier(newMultiplier);
        uiManager.UpdateMultiplier(newMultiplier);
    }
    
    public void OnGameOver()
    {
        if(isGameOver)
            return;
        isGameOver = true;
        Time.timeScale = 0f;
        audioManager?.PauseAll();
        if (inputManager!= null)
            inputManager.enabled = false;

        // 3. Muestra el panel de Game Over
        uiManager?.ShowGameOverPanel(true);
    }
    public void RestartRun()
    {
        // 1. Reactiva tiempo y limpia flags
        Time.timeScale = 1f;
        isGameOver = false;

        // 2. Reinicia lógica de juego
        scoreManager.currentScore = 0;
        scoreManager.currentCombo = 0;
        scoreManager.SetMultiplier(1);

        healthManager.ResetHealth();   // 🔧 Asegúrate de tener este método
        powerUpManager.ResetPower();   // 🔧 También este

        // 3. Elimina notas vivas (si no usas pool)
        foreach (var note in GameObject.FindGameObjectsWithTag("Nota"))
            Destroy(note);

        // 4. Reinicia música sincronizada
        audioManager?.StopAllAudio();
        audioManager?.StartAllSynced();

        // 5. Oculta panel y reactiva input
        uiManager?.ShowGameOverPanel(false);
        if (inputManager != null)
            inputManager.enabled = true;
    }

    public void irAlMenu(string sceneName = "Menu")
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    private void UpdateAllUI()
    {
        uiManager.UpdateScore(scoreManager.currentScore);
        uiManager.UpdateHealth(healthManager.currentHealth, healthManager.maxHealth);
        uiManager.UpdatePowerUp(powerUpManager.currentPowerUpValue, powerUpManager.comboToFill);
        uiManager.UpdateCombo(scoreManager.currentCombo);
    }
}