using UnityEngine;

public class InputManager : MonoBehaviour
{
    public KeyCode[] inputKeys;
    public KeyCode powerUpKey; // Tecla para activar el poder
    public Detector[] detectors;
    
    void Update()
    {
        // Recorremos las teclas de las notas
        for (int i = 0; i < inputKeys.Length; i++)
        {
            if (Input.GetKeyDown(inputKeys[i]))
            {
                if (detectors[i].TryHitNote())
                {
                    GameManager.instance.OnNoteHit(i);
                }
                else
                {
                    GameManager.instance.OnNoteMiss(i);
                }
            }
        }
        
        // Comprobamos la tecla del poder
        if (Input.GetKeyDown(powerUpKey))
        {
            GameManager.instance.powerUpManager.TryActivatePowerUp();
        }
    }
}