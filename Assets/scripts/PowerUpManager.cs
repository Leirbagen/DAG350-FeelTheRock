using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    [Header("Configuración")]
    public int comboToFill = 20; // Combo necesario para llenar la barra
    public float powerUpDuration = 10f; // Duración del multiplicador

    public int currentPowerUpValue = 0;
    private bool isPowerUpActive = false;
    private float powerUpTimer = 0f;

    void Update()
    {
        // Lógica del temporizador del poder
        if (isPowerUpActive)
        {
            powerUpTimer -= Time.deltaTime;
            if (powerUpTimer <= 0)
            {
                DeactivatePowerUp();
            }
        }
    }

    // Se llama al acertar una nota
    public void OnNoteHit(int currentCombo)
    {
        // Solo llenamos la barra si el combo es continuo
        if (currentCombo > 0 && currentPowerUpValue < comboToFill)
        {
            currentPowerUpValue = currentCombo;
            if (currentPowerUpValue >= comboToFill)
            {
                currentPowerUpValue = comboToFill;
                // ¡Barra llena! Aquí podrías añadir un efecto de sonido o visual
            }
        }
    }

    // Se llama al fallar una nota
    public void OnNoteMiss()
    {
        // Si el poder no está activo, la barra se vacía al fallar
        if (!isPowerUpActive)
        {
            currentPowerUpValue = 0;
        }
    }
    
    // Método para ser llamado desde el InputManager cuando se activa el poder
    public void TryActivatePowerUp()
    {
        if(currentPowerUpValue >= comboToFill && !isPowerUpActive)
        {
            ActivatePowerUp();
        }
    }

    private void ActivatePowerUp()
    {
        isPowerUpActive = true;
        powerUpTimer = powerUpDuration;
        GameManager.instance.OnMultiplierChanged(2);
        // Aquí podrías añadir la lógica para que las notas cambien de color
    }

    private void DeactivatePowerUp()
    {
        isPowerUpActive = false;
        currentPowerUpValue = 0; // La barra se vacía
        GameManager.instance.OnMultiplierChanged(1);
        // Aquí volverías a poner las notas a sus colores originales
    }
}