using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SpriteClickCambiaEscena : MonoBehaviour
{
    public void CambiarEscena(string nombre)
    {
        SceneManager.LoadScene(nombre);
    }
}
