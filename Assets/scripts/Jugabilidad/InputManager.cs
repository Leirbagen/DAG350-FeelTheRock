using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [Header("Configuración de Teclas")]
    public KeyCode[] inputKeys;
    public KeyCode powerUpKey = KeyCode.Space; // Tecla para activar el poder
    [SerializeField] Animator animatorBtn1;
    [SerializeField] Animator animatorBtn2;
    [SerializeField] Animator animatorBtn3;
    [SerializeField] Animator animatorBtn4;
  


    [Header("Referencias a Detectores")]
    public List<Detector> detectors;
    public List<Detector> detectorsPerfect;


    public void ControlarAnimaciones()
    {
        if (Input.GetKeyDown(inputKeys[0])) animatorBtn1.SetTrigger("press");
        if (Input.GetKeyDown(inputKeys[1])) animatorBtn2.SetTrigger("press2");
        if (Input.GetKeyDown(inputKeys[2])) animatorBtn3.SetTrigger("press3");
        if (Input.GetKeyDown(inputKeys[3])) animatorBtn4.SetTrigger("press4");
    }

    void Update()
    {
        // Lógica para las notas
        for (int i = 0; i < inputKeys.Length; i++)
        {
            if (Input.GetKeyDown(inputKeys[i]))
            {
                bool hitPerfect = detectorsPerfect[i].HasNote() && detectorsPerfect[i].TryHitNote(true);
                if (hitPerfect)
                {
                    GameManager.instance.OnNoteHit(i, true);   // Perfecto
                }
                else
                {
                    bool hitGood = detectors[i].HasNote() && detectors[i].TryHitNote(false);
                    if (hitGood)
                        GameManager.instance.OnNoteHit(i, false);  // Bien
                    else
                        GameManager.instance.OnNoteMiss(i);
                }
            }
        }
        
        ControlarAnimaciones();

        // --- ¡NUEVA LÓGICA PARA ACTIVAR EL PODER! ---
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Le pedimos al PowerUpManager que intente activarse.
            GameManager.instance.powerUpManager.TryActivatePowerUp();
        }

       
    }
}