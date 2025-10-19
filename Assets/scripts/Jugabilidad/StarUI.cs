using UnityEngine;
using UnityEngine.UI;

public class StarUI : MonoBehaviour
{
    [Header("Sprites (los dos estados)")]
    [SerializeField] private Sprite starOff;  //  estrelaApagada.png
    [SerializeField] private Sprite starOn;   //  estrella.png

    [Header("Tres imágenes visibles en la escena")]
    [SerializeField] private Image[] stars;  

    public void SetStars(int count)
    {
        // Asegura que esté entre 0 y 3
        count = Mathf.Clamp(count, 0, stars.Length);

        // Cambia el sprite según el número de estrellas obtenidas
        for (int i = 0; i < stars.Length; i++)
        {
            if (stars[i] != null)
                stars[i].sprite = (i < count) ? starOn : starOff;
        }
    }
}
