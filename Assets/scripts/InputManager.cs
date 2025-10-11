using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [Header("Configuración de Teclas")]
    public KeyCode[] inputKeys;
    public KeyCode powerUpKey = KeyCode.Space; // Tecla para activar el poder

    [Header("Referencias a Detectores")]
    public List<Detector> detectors;

    void Update()
    {
        // Lógica para las notas
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

        // --- ¡NUEVA LÓGICA PARA ACTIVAR EL PODER! ---
        if (Input.GetKeyDown(powerUpKey))
        {
            // Le pedimos al PowerUpManager que intente activarse.
            GameManager.instance.powerUpManager.TryActivatePowerUp();
        }
    }
}