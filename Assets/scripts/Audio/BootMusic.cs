using UnityEngine;

public class BootMusic : MonoBehaviour
{
    [SerializeField] private string startKey = "menu"; // misma para Menú, Álbum, etc.

    void Start()
    {
        // Asegura que suene el tema del menú si no está ya sonando
        if (AudioManagerGame.Instance != null && !AudioManagerGame.Instance.IsPlaying(startKey))
        {
            AudioManagerGame.Instance.FadeTo(startKey);
        }
    }
}
