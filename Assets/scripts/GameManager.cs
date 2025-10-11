using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Referencias a Managers (Arrastrar desde la Jerarquía)")]
    public ScoreManager scoreManager;
    public HealthManager healthManager;
    public PowerUpManager powerUpManager;
    public UIManager uiManager;
    public AudioManager audioManager;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Debug.Log("<color=purple>GAME MANAGER: Instancia creada correctamente.</color>");
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
            Debug.LogError("ERROR: Faltan referencias de Managers en el GameManager! Arrástralos en el Inspector.");
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
        Debug.Log($"<color=#8B0000>GAME MANAGER: OnNoteMiss RECIBIDO para carril {laneID}</color>");

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
        Debug.Log("GAME OVER!");
        Time.timeScale = 0f;
    }
    
    private void UpdateAllUI()
    {
        uiManager.UpdateScore(scoreManager.currentScore);
        uiManager.UpdateHealth(healthManager.currentHealth, healthManager.maxHealth);
        uiManager.UpdatePowerUp(powerUpManager.currentPowerUpValue, powerUpManager.comboToFill);
        uiManager.UpdateCombo(scoreManager.currentCombo);
    }
}