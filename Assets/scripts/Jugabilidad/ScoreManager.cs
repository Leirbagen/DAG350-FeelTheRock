using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // Añadimos el Singleton para que el UIManager pueda accederlo
    public static ScoreManager instance;

    public int currentScore = 0;
    public int currentCombo = 0;
    public int currentMultiplier = 1;

    [Header("Configuración")]
    public int scorePerNote = 100;
    public int comboToShowThreshold = 5; // A partir de qué combo se muestra

    public UIManager uimanager;

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

    // Este método se llama cuando se acierta una nota
    public void AddScore(bool type)
    {
        if(type == true)
        {
            uimanager.ShowHitText("Perfecto");
            currentScore += (2*scorePerNote) * currentMultiplier;
            currentCombo++;
        }
        if(type == false)
        {
            uimanager.ShowHitText("Bien");
            currentScore += scorePerNote * currentMultiplier;
            currentCombo++;
        }

    }

    // Este método se llama cuando se falla una nota
    public void BreakCombo()
    {
        currentCombo = 0;
    }

    // El PowerUpManager nos dirá cuál es el multiplicador actual
    public void SetMultiplier(int multiplier)
    {
        currentMultiplier = multiplier;
    }
}