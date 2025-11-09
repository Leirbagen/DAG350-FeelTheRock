using UnityEngine;

public class SceneMusicSetter : MonoBehaviour
{
    [SerializeField] private string musicKey ;
    [SerializeField] private float delay = 0f; // 0.1–0.3s si quieres que el fade suene tras cargar

    private void Start()
    {
        StartCoroutine(CoSet());
    }

    private System.Collections.IEnumerator CoSet()
    {
        // Espera 1 frame para que cualquier BootMusic de la escena termine de correr
        yield return null;

        if (delay > 0f)
            yield return new WaitForSecondsRealtime(delay);

        if (AudioManagerGame.Instance != null && !string.IsNullOrEmpty(musicKey))
        {
            Debug.Log($"[SceneMusicSetter] Forzando música '{musicKey}' en escena {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");
            AudioManagerGame.Instance.FadeTo(musicKey);
        }
        else
        {
            Debug.LogWarning("[SceneMusicSetter] AudioManager.Instance es null o musicKey vacío");
        }
    }
}