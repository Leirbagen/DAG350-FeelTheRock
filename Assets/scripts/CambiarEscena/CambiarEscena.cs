using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SpriteClickCambiaEscena : MonoBehaviour
{
    [Header("Opcional: forzar la música por botón")]
    [SerializeField] private string musicKeyOverride = ""; // "80s", "90s", "2000s", "menu"
    [SerializeField] private float preFadeDelay = 0f;      // 0.2–0.5s si quieres oír el crossfade antes

    public void CambiarEscena(string nombre)
    {
        // --- Música ---
        if (AudioManagerGame.Instance != null)
        {
            string key = musicKeyOverride;

            if (string.IsNullOrEmpty(key))
            {
                // Fallback por patrón en el nombre de escena
                string lower = nombre.ToLowerInvariant();
                if (lower.Contains("80")) key = "80s";
                else if (lower.Contains("90")) key = "90s";
                else if (lower.Contains("00")) key = "2000s";
                else key = "menu";
            }

            Debug.Log($"[SceneMusic] Escena destino: {nombre} -> clave '{key}'");
            AudioManagerGame.Instance.FadeTo(key);
        }
        else
        {
            Debug.LogWarning("[SceneMusic] AudioManager.Instance es null");
        }

        // --- Carga de escena ---
        if (preFadeDelay > 0f)
        {
            StartCoroutine(CoLoadAfterDelay(nombre, preFadeDelay));
        }
        else
        {
            SceneManager.LoadScene(nombre);
        }
    }

    private System.Collections.IEnumerator CoLoadAfterDelay(string scene, float s)
    {
        yield return new WaitForSecondsRealtime(s);
        SceneManager.LoadScene(scene);
    }
}