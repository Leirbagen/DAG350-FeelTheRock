using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSelectItem : MonoBehaviour
{
    [Header("Datos del nivel")]
    public SongChart chart;           // el chart de ESTE nivel
    public string sceneName;          // nombre exacto de la escena de este nivel

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public StarUI starUI;
    public Button playButton;
    public GameObject lockGroup;

    // la llama el controller
    public void Setup(bool unlocked, System.Action<LevelSelectItem> onPlay)
    {
        // pinta desde GameProgress usando el levelId del chart
        string levelId = chart.levelID;
        int bestScore = GameProgress.Instance.GetBestScore(levelId);
        int bestStars = GameProgress.Instance.GetBestStars(levelId);

        if (scoreText) scoreText.text = $"Puntaje: {bestScore}";
        if (starUI) starUI.SetStars(bestStars);

        if (lockGroup) lockGroup.SetActive(!unlocked);
        if (playButton)
        {
            playButton.interactable = unlocked;
            playButton.onClick.RemoveAllListeners();
            playButton.onClick.AddListener(() => onPlay?.Invoke(this));
        }
    }
}
