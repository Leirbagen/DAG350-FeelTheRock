using UnityEngine;

// Versión simplificada para solo notas cortas.
public class Nota : MonoBehaviour
{
    [Header("Configuración de la Nota")]
    public int laneID;
    public PowerUpManager powerUpManager;

    // Ya no necesitamos 'duration', 'isCompleted', 'head', 'tail', etc.

    // La función Hit ahora es muy simple: solo destruye la nota.
    public void Hit()
    {
        // Inmediatamente destruimos el objeto de la nota al ser tocada.
        Destroy(gameObject);
    }
    public void poderActivado(Sprite poder)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = poder;
    }
}
