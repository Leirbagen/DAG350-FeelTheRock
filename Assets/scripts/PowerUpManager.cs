using System.Collections;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    [Header("Configuración del Poder")]
    public int comboToFill = 20;
    public int powerUpMultiplier = 2;
    public float powerUpDuration = 10f;

    [Header("Estado Actual (Solo para Debug)")]
    [SerializeField] private int currentPowerUpValue = 0;
    [SerializeField] private bool isReady = false;
    [SerializeField] private bool isActive = false;

    public void OnNoteHit(int currentCombo)
    {
        // Solo llenamos la barra si el poder no está listo y no está activo
        if (!isReady && !isActive)
        {
            currentPowerUpValue = currentCombo;
            GameManager.instance.uiManager.UpdatePowerUp(currentPowerUpValue, comboToFill);

            if (currentPowerUpValue >= comboToFill)
            {
                isReady = true;
                // Opcional: añadir un sonido o efecto para avisar que está listo
            }
        }
    }

    public void OnNoteMiss()
    {
        // Si fallamos mientras llenamos la barra (y no está activo), la vaciamos
        if (!isActive)
        {
            currentPowerUpValue = 0;
            isReady = false;
            GameManager.instance.uiManager.UpdatePowerUp(currentPowerUpValue, comboToFill);
        }
    }

    // --- ¡NUEVA FUNCIÓN PARA ACTIVAR EL PODER! ---
    public void TryActivatePowerUp()
    {
        if (isReady && !isActive)
        {
            isReady = false;
            StartCoroutine(PowerUpSequence());
        }
    }

    private IEnumerator PowerUpSequence()
    {
        isActive = true;

        // Le decimos al GameManager que aplique el multiplicador
        GameManager.instance.OnMultiplierChanged(powerUpMultiplier);
        
        // Aquí puedes añadir la lógica para cambiar el color de las notas
        // Ejemplo: NoteSpawner.instance.ActivatePowerUpMode(true);

        // Vaciamos la barra en la UI
        currentPowerUpValue = 0;
        GameManager.instance.uiManager.UpdatePowerUp(currentPowerUpValue, comboToFill);
        
        // Esperamos la duración del poder
        yield return new WaitForSeconds(powerUpDuration);

        // Le decimos al GameManager que vuelva al multiplicador normal
        GameManager.instance.OnMultiplierChanged(1);
        
        // Aquí desactivaríamos el modo de cambio de color de notas
        // Ejemplo: NoteSpawner.instance.ActivatePowerUpMode(false);
        
        isActive = false;
    }
}