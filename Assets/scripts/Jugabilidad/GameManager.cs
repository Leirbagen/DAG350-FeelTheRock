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
    bool levelFinishedRaised = false;
    string currentScene;



    public int star1Threshold;
    public int star2Threshold;
    public int star3Threshold;

    public string levelID;
    public SongChart song;


    string ResolveLevelId(SongChart song)
    {
        if (song != null && !string.IsNullOrEmpty(song.levelID)) return song.levelID;
        if (song != null) return song.name; // fallback: nombre del asset
        return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name; // último recurso
    }

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
        //  Preparar audio y spawner
        if (audioManager != null && song != null)
            audioManager.SetupSong(song);

        if (noteSpawner != null && song != null)
            noteSpawner.currentSong = song;

        currentScene = SceneManager.GetActiveScene().name;
        star1Threshold = song.star1Threshold;
        star2Threshold = song.star2Threshold;
        star3Threshold = song.star3Threshold;

        levelID = ResolveLevelId(song);

        // Prepara video y arranca cuando esté listo
        if (gameOver != null)
        {
            gameOver.prepareCompleted += OnVideoReady;
            gameOver.Prepare(); // empieza a decodificar primer frame
        }
       
        Time.timeScale = 1f;

        

        StartRun();

        uiManager.UpdateScore(scoreManager.currentScore);
        uiManager.UpdateHealth(healthManager.currentHealth, healthManager.maxHealth);
        uiManager.UpdatePowerUp(powerUpManager.currentPowerUpValue, powerUpManager.comboToFill);
        uiManager.UpdateCombo(scoreManager.currentCombo);
        uiManager.UpdateMultiplier(scoreManager.currentMultiplier);
        uiManager.UpdateStarsUI();
    }
    private void OnVideoReady(VideoPlayer vp)
    {
        gameOver.Play(); // o PlayDelayed(0.5f) si quieres alinear con el lead-in
    }
    public void Pausa()
    {
        if(isPaused) return;
        isPaused = true;
        uiManager?.ShowPausePanel(true);
        audioManager?.PauseAll();
        noteSpawner?.StopSpawning();
        if(uiManager?.bienTexto.gameObject.activeSelf == true)
        {
            uiManager.bienTexto.gameObject.SetActive(false);
        }
        if(uiManager?.perfectoTexto.gameObject.activeSelf == true)
        {
            uiManager.perfectoTexto.gameObject.SetActive(false);
        }
        if(uiManager?.falloTexto.gameObject.activeSelf == true)
        {
            uiManager.falloTexto.gameObject.SetActive(false);
        }
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
        if (audioManager != null) audioManager.StartAllSynced(0.5);
        if (noteSpawner != null) noteSpawner.ResetAndStart();

        // Mantienes tus resets de score/vida/UI si corresponde
        //scoreManager?.ResetScore();
        healthManager?.ResetHealth();
        powerUpManager?.ResetPower();
        UpdateAllUI();
        isGameOver = false;
    }

    public void OnNoteHit(int laneID, bool type)
    {
        audioManager.HitNote(laneID);
        scoreManager.AddScore(type);
        healthManager.OnNoteHit();
        powerUpManager.OnNoteHit(scoreManager.currentCombo);
        /*animatorBtn1.SetBool("ispresionado", true);
        animatorBtn2.SetBool("ispresionada2", true);
        animatorBtn3.SetBool("ispresionada3", true);
        animatorBtn4.SetBool("ispresionada4", true);*/

        UpdateAllUI();
    }

    public void OnNoteMiss(int laneID)
    {

        audioManager.MissNote(laneID);
        audioManager.PlayMissSound();
        scoreManager.BreakCombo();
        healthManager.OnNoteMiss();
        powerUpManager.OnNoteMiss();
        uiManager.showMissHit();


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
        uiManager?.restartStarts();

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

    void OnLevelFinish()
    {

        uiManager?.ShowFinishPanel(true);
        audioManager?.StopAllAudio();
        noteSpawner?.StopSpawning();
        int stars = EvaluateStars(scoreManager.currentScore, star1Threshold, star2Threshold, star3Threshold);
        GameProgress.Instance.ReportResult(levelID, scoreManager.currentScore, stars, true);
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
        uiManager.UpdateStarsUI();

    }


    public void Menu()
    {
        if ((currentScene == "TheBeatles_LetItBe") || (currentScene == "TheDoors") || (currentScene == "TheRollingStones"))
        {
            SceneManager.LoadScene("Niveles60");
        }
        else if ((currentScene == "BlackSabbath") || (currentScene == "DeepPurple") || (currentScene == "LedZeppelin") || (currentScene == "PinkFloyd") || (currentScene == "Queen"))
        {
            SceneManager.LoadScene("Niveles70");
        }
        else if ((currentScene == "GunsNRoses") || (currentScene == "IronMaiden") || (currentScene == "Metallica"))
        {
            SceneManager.LoadScene("Niveles80");
        }
        else if ((currentScene == "Korn") || (currentScene == "Nirvana") || (currentScene == "Radiohead"))
        {
            SceneManager.LoadScene("Niveles90");
        }
        else if ((currentScene == "ArcticMonkeys") || (currentScene == "LinkinPark") || (currentScene == "SystemOfaDown"))
        {
            SceneManager.LoadScene("Niveles00");
        }
    }


    public int EvaluateStars(int score, int t1, int t2, int t3)
    {
        if (score >= t3) return 3;
        if (score >= t2) return 2;
        if (score >= t1) return 1;
        return 0;
    }

    public void Update()
    {
        if (!isPaused && !isGameOver && !levelFinishedRaised)
        {
            // Sólo por final de canción:
            if (audioManager != null && audioManager.IsSongEnded(0.10))
            {
                levelFinishedRaised = true;
                OnLevelFinish();
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pausa();
        }
            
    }
}