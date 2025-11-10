using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DecadeSelectItem : MonoBehaviour
{
    [Header("Datos de la década")]
    public string decadeId;                // "60s", "70s", ...
    public string sceneName;               // escena del selector de niveles
    public SongChart[] chartsInDecade;     // los 3 charts de la década

    [Header("UI (solo sprite)")]
    public Image targetImage;              // la Image a cambiar
    public Sprite lockedSprite;
    public Sprite unlockedSprite;
    public Button openButton;              // el botón de la tarjeta
    public TextMeshProUGUI progressText;   // opcional

    public void Setup(bool unlocked, System.Action<DecadeSelectItem> onOpen)
    {
        // sprite según estado
        if (targetImage)
            targetImage.sprite = unlocked ? unlockedSprite : lockedSprite;

        // botón
        if (openButton)
        {
            openButton.interactable = unlocked;
            openButton.onClick.RemoveAllListeners();
            openButton.onClick.AddListener(() => onOpen?.Invoke(this));
        }

        // progreso opcional
        if (progressText)
        {
            int done = 0;
            foreach (var ch in chartsInDecade)
                if (ch && GameProgress.Instance.IsCompleted(ch.levelID)) done++;
            progressText.text = $"Completados: {done}/{chartsInDecade.Length}";
        }
    }

    public bool AreAllLevelsCompleted()
    {
        if (chartsInDecade == null || chartsInDecade.Length == 0) return false;
        foreach (var ch in chartsInDecade)
            if (!ch || !GameProgress.Instance.IsCompleted(ch.levelID)) return false;
        return true;
    }
}
