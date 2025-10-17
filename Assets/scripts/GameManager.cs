using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public VideoPlayer gameOver;
    [Header("Referencias a Managers (Arrastrar desde la Jerarquía)")]
    public ScoreManager scoreManager;
    public HealthManager healthManager;
    public PowerUpManager powerUpManager;
    public UIManager uiManager;
    public AudioManager audioManager;
    public InputManager inputManager;
    public bool isGameOver = false;
    public bool isPaused = false;
    public NoteSpawner noteSpawner;
    [SerializeField] private AudioSource sfxSource;  
    [SerializeField] private AudioClip gameOverClip;  
    [SerializeField] private float gameOverVolume = 1f;

    public SongChart song;


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
        Time.timeScale = 1f;

        // [NUEVO] Preparar audio y spawner
        if (audioManager != null && song != null)
            audioManager.SetupSong(song);

        if (noteSpawner != null && song != null)
            noteSpawner.currentSong = song;

        StartRun();

        uiManager.UpdateScore(scoreManager.currentScore);
        uiManager.UpdateHealth(healthManager.currentHealth, healthManager.maxHealth);
        uiManager.UpdatePowerUp(powerUpManager.currentPowerUpValue, powerUpManager.comboToFill);
        uiManager.UpdateCombo(scoreManager.currentCombo);
        uiManager.UpdateMultiplier(scoreManager.currentMultiplier);
    }

    public void Pausa()
    {
        if(isPaused) return;
        isPaused = true;
        uiManager?.ShowPausePanel(true);
        audioManager?.PauseAll();
        noteSpawner?.StopSpawning();
        Time.timeScale = 0f;
    }

    public void UnPause() {
        if(!isPaused) return;
        isPaused = false;
        uiManager?.ShowPausePanel(false);
        audioManager?.UnpauseAll();
        noteSpawner?.StartSpawning();
        Time.timeScale = 1f;
    }

    public void StartRun()
    {
        if (audioManager != null) audioManager.StartAllSynced(0.2);
        if (noteSpawner != null) noteSpawner.ResetAndStart();

        // Mantienes tus resets de score/vida/UI si corresponde
        //scoreManager?.ResetScore();
        healthManager?.ResetHealth();
        powerUpManager?.ResetPower();
        UpdateAllUI();
        isGameOver = false;
    }

    public void OnNoteHit(int laneID)
    {

        audioManager.HitNote(laneID);
        scoreManager.AddScore();
        healthManager.OnNoteHit();
        powerUpManager.OnNoteHit(scoreManager.currentCombo);

        UpdateAllUI();
    }

    public void OnNoteMiss(int laneID)
    {

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
        noteSpawner?.StopSpawning();
        if (sfxSource && gameOverClip)
            sfxSource.PlayOneShot(gameOverClip, gameOverVolume);
        Time.timeScale = 0f;
        audioManager?.PauseAll();
        if (inputManager!= null)
            inputManager.enabled = false;

        // 3. Muestra el panel de Game Over
        uiManager?.ShowGameOverPanel(true);
        gameOver.Play();
    }
    public void RestartRun()
    {
        gameOver.Stop();
        // 1. Reactiva tiempo y limpia flags
        Time.timeScale = 1f;
        isGameOver = false;

        // 2. Reinicia lógica de juego
        scoreManager.currentScore = 0;
        scoreManager.currentCombo = 0;
        uiManager?.UpdateScore(0);
        scoreManager.SetMultiplier(1);
        healthManager.ResetHealth();   
        powerUpManager.ResetPower();  

        // 3. Elimina notas vivas 
        foreach (var note in GameObject.FindGameObjectsWithTag("Nota"))
            Destroy(note);

        // 4. Reinicia música sincronizada y el spawner
        audioManager?.StopAllAudio();
        audioManager?.StartAllSynced();
        noteSpawner?.ResetAndStart();
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

    public void UpdateAllUI()
    {
        uiManager.UpdateScore(scoreManager.currentScore);
        uiManager.UpdateHealth(healthManager.currentHealth, healthManager.maxHealth);
        uiManager.UpdatePowerUp(powerUpManager.currentPowerUpValue, powerUpManager.comboToFill);
        uiManager.UpdateCombo(scoreManager.currentCombo);
    }

    public void Menu()
    {
        SceneManager.LoadScene("Niveles60");
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pausa();
        }
            
    }
}