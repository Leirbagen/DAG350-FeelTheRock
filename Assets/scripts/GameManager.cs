using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Referencias a Managers")]
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
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Reseteamos el tiempo por si acaso se quedó pausado en una partida anterior
        Time.timeScale = 1f;

        // Validamos que todos los managers estén asignados para evitar errores
        if (uiManager == null || scoreManager == null || healthManager == null || powerUpManager == null)
        {
            Debug.LogError("ERROR: Faltan referencias de Managers en el GameManager!");
            return;
        }

        // Inicializamos los valores de la UI al empezar
        uiManager.UpdateScore(scoreManager.currentScore);
        uiManager.UpdateHealth(healthManager.currentHealth, healthManager.maxHealth);
        uiManager.UpdatePowerUp(powerUpManager.currentPowerUpValue, powerUpManager.comboToFill);
        uiManager.UpdateCombo(scoreManager.currentCombo);
        uiManager.UpdateMultiplier(scoreManager.currentMultiplier);
    }

    // Método centralizado para cuando se acierta una nota
    public void OnNoteHit(int laneID)
    {
        audioManager.HitNote(laneID);
        scoreManager.AddScore();
        healthManager.OnNoteHit();
        powerUpManager.OnNoteHit(scoreManager.currentCombo);

        // Actualizamos toda la UI
        uiManager.UpdateScore(scoreManager.currentScore);
        uiManager.UpdateHealth(healthManager.currentHealth, healthManager.maxHealth);
        uiManager.UpdatePowerUp(powerUpManager.currentPowerUpValue, powerUpManager.comboToFill);
        uiManager.UpdateCombo(scoreManager.currentCombo);
    }

    // Método centralizado para cuando se falla una nota
    public void OnNoteMiss(int laneID)
    {
        audioManager.MissNote(laneID);
        audioManager.PlayMissSound();
        scoreManager.BreakCombo();
        healthManager.OnNoteMiss();
        powerUpManager.OnNoteMiss();

        // Actualizamos toda la UI
        uiManager.UpdateHealth(healthManager.currentHealth, healthManager.maxHealth);
        uiManager.UpdatePowerUp(powerUpManager.currentPowerUpValue, powerUpManager.comboToFill);
        uiManager.UpdateCombo(scoreManager.currentCombo);
    }
    
    // El PowerUpManager nos avisará cuando el multiplicador cambie
    public void OnMultiplierChanged(int newMultiplier)
    {
        scoreManager.SetMultiplier(newMultiplier);
        uiManager.UpdateMultiplier(newMultiplier);
    }
    
    // El HealthManager nos avisará cuando el juego termine
    public void OnGameOver()
    {
        // Aquí irá la lógica para mostrar la ventana de "Game Over"
        Debug.Log("GAME OVER!");
        // Detenemos el tiempo para pausar el juego
        Time.timeScale = 0f;
    }
}