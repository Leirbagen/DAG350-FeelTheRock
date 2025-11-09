using UnityEngine;

public class Botones : MonoBehaviour
{
    [SerializeField] private float fadeOutSeconds = 0.4f;

    // Llama esto ANTES de invocar tu CambiarEscena(nombre)
    public void OnClickStopMusic()
    {
        AudioManagerGame.Instance?.FadeOutAndStop(fadeOutSeconds);
    }
}
