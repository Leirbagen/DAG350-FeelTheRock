using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectController : MonoBehaviour
{
    [Header("Tarjetas en orden de progreso (L1, L2, L3...)")]
    public LevelSelectItem[] items;

    void OnEnable() => Refresh();

    public void Refresh()
    {
        if (items == null || items.Length == 0)
        {
            Debug.LogWarning("[LevelSelect] items[] vacío o nulo.");
            return;
        }

        for (int i = 0; i < items.Length; i++)
        {
            var item = items[i];
            if (!item)
            {
                Debug.LogWarning($"[LevelSelect] items[{i}] es null.");
                continue;
            }
            if (!item.chart)
            {
                Debug.LogWarning($"[LevelSelect] '{item.name}' no tiene SongChart asignado.");
                continue;
            }

            string lid = item.chart.levelID; // usa tu campo tal cual
            int bestScore = GameProgress.Instance.GetBestScore(lid);
            int bestStars = GameProgress.Instance.GetBestStars(lid);

            bool unlocked = (i == 0);
            string reason = "primer nivel (desbloqueado por defecto)";

            if (i > 0)
            {
                var prev = items[i - 1].chart;
                if (prev)
                {
                    bool prevCompleted = GameProgress.Instance.IsCompleted(prev.levelID);
                    unlocked = prevCompleted;
                    reason = $"prev='{prev.levelID}' completed={prevCompleted}";
                }
                else
                {
                    unlocked = false;
                    reason = "prev chart NULO";
                }
            }

            Debug.Log($"[LevelSelect] i={i} card='{item.name}' lid='{lid}' " +
                      $"score={bestScore} stars={bestStars} unlocked={unlocked} ({reason})");

            item.Setup(unlocked, OnPlayLevel);
        }
    }


    private void OnPlayLevel(LevelSelectItem item)
    {
        // cargamos directamente la escena propia de ese nivel
        if (!string.IsNullOrEmpty(item.sceneName))
        {
            SceneManager.LoadScene(item.sceneName);
        }
        else
        {
            print($"LevelSelect: sceneName vacío para {item.chart?.levelID}");
        }
    }
    // desbloquea todos los niveles como cheat 
    public void Cheat_UnlockAllLevels()
    {
        if (items == null || items.Length == 0)
        {
            Debug.LogWarning("[LevelSelect] Cheat_UnlockAllLevels: no hay items.");
            return;
        }

        foreach (var item in items)
        {
            if (item == null || item.chart == null) continue;

            string lid = item.chart.levelID;

            // Le damos un score alto y 3 estrellas, y lo marcamos como completado.
            GameProgress.Instance.ReportResult(
                lid,
                score: 999999,   // o lo que quieras
                stars: 3,
                markCompleted: true
            );

            Refresh();  // refrescamos para que se vean todos desbloqueados
        }
    }
}
