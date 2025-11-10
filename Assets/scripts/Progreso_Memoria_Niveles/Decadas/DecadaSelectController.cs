using UnityEngine;
using UnityEngine.SceneManagement;

public class DecadeSelectController : MonoBehaviour
{
    [Header("Tarjetas en orden (60s, 70s, 80s, ...)")]
    public DecadeSelectItem[] items;

    void OnEnable() => Refresh();

    public void Refresh()
    {
        if (items == null || items.Length == 0)
        {
            Debug.LogWarning("[DecadeSelect] items[] vacío.");
            return;
        }

        for (int i = 0; i < items.Length; i++)
        {
            var item = items[i];
            if (!item)
            {
                Debug.LogWarning($"[DecadeSelect] items[{i}] es null.");
                continue;
            }

            bool unlocked = (i == 0);
            string reason = "primera década desbloqueada por defecto";

            if (i > 0)
            {
                var prev = items[i - 1];
                if (prev)
                {
                    bool prevCompleted = prev.AreAllLevelsCompleted();
                    unlocked = prevCompleted;
                    reason = $"prev='{prev.decadeId}' complete={prevCompleted}";
                }
                else
                {
                    unlocked = false;
                    reason = "prev NULO";
                }
            }

            Debug.Log($"[DecadeSelect] i={i} '{item.decadeId}' unlocked={unlocked} ({reason})");
            item.Setup(unlocked, OnOpenDecade);
        }
    }

    private void OnOpenDecade(DecadeSelectItem item)
    {
        if (!string.IsNullOrEmpty(item.sceneName))
            SceneManager.LoadScene(item.sceneName);
        else
            Debug.Log($"DecadeSelect: sceneName vacío para '{item.decadeId}'.");
    }
}
