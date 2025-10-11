using UnityEngine;

public class Detector : MonoBehaviour
{
    public int laneID;
    private GameObject noteInTrigger = null;
    private AudioManager audioManager;

    private void Start()
    {
        // Usamos FindFirstObjectByType que es más moderno.
        audioManager = FindFirstObjectByType<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Solo reaccionamos a objetos con el Tag "Nota".
        if (other.CompareTag("Nota"))
        {
            noteInTrigger = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Si la nota que sale es la que teníamos registrada, es un fallo.
        if (other.gameObject == noteInTrigger)
        {
            if (audioManager != null)
            {
                audioManager.MissNote(laneID);
                audioManager.PlayMissSound();
            }
            // Olvidamos la nota para no registrar múltiples fallos.
            noteInTrigger = null;
        }
    }

    public bool TryHitNote()
    {
        if (noteInTrigger != null)
        {
            // Guardamos la referencia, la anulamos para evitar fallos falsos y luego destruimos.
            GameObject noteToDestroy = noteInTrigger;
            noteInTrigger = null;
            
            Destroy(noteToDestroy);
            return true;
        }
        return false;
    }
}