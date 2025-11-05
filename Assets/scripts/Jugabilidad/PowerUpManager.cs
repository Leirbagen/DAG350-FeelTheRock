using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    [Header("Configuración del Poder")]
    [SerializeField] public int comboToFill = 10;
    public event Action<bool> OnPowerChanged; // <<< true=ON, false=OFF
    public int powerUpMultiplier = 2;
    public int incrementadorPorLLenado = 2;
    public int maxMultiplier = 0;
    public ScoreManager scoreManager;
    public UIManager uiManager;
    [SerializeField] private Animator animator;
    [SerializeField] public int currentPowerUpValue = 0;
    [SerializeField] private bool isReady = false;
    [SerializeField] public bool isActivo = false;

    public void Awake()
    {
        // Forzar evaluación inmediata del IdlePose para evitar cualquier frame “sucio”
        animator.Play("Idle", 0, 0f);
        animator.Update(0f);
    }
    public void OnNoteHit(int currentCombo)
    {
        if (!isActivo)
        {
            // Antes de activar: usa el COMBO global para llenar la barra
            currentPowerUpValue = Mathf.Clamp(currentCombo, 0, comboToFill);
            uiManager?.UpdatePowerUp(currentPowerUpValue, comboToFill);

            if (currentPowerUpValue >= comboToFill)
                isReady = true; // listo para activar por primera vez
        }
        else
        {
            // Ya activo: llenamos por hits para permitir stacking
            currentPowerUpValue = Mathf.Min(currentPowerUpValue + 1, comboToFill);
            uiManager?.UpdatePowerUp(currentPowerUpValue, comboToFill);

            if (currentPowerUpValue >= comboToFill)
                stackMultiplier(); // x2 -> x4 -> x6...
        }
    }

    public void ResetPower()
    {
        isActivo = false;
        OnPowerChanged?.Invoke(false); // <<< OFF
        isReady = false;
        currentPowerUpValue = 0;
        
        scoreManager.SetMultiplier(1);
        uiManager?.UpdateMultiplier(1);
        if (uiManager?.multiplierText) uiManager.multiplierText.gameObject.SetActive(false);
        uiManager?.UpdatePowerUp(currentPowerUpValue, comboToFill);
    }

    public void OnNoteMiss()
    {
        if (isActivo)
        {
            isActivo = false;
            OnPowerChanged?.Invoke(false); // <<< OFF
            animator.gameObject.SetActive(false);
            scoreManager.SetMultiplier(1);
            uiManager?.UpdateMultiplier(1);
            if(uiManager?.multiplierText)
                uiManager.multiplierText.gameObject.SetActive(false);
        }

        currentPowerUpValue = 0;
        isReady = false;
        uiManager?.UpdatePowerUp(currentPowerUpValue, comboToFill);
    }

    public void TryActivatePowerUp()
    {
        if (isReady && !isActivo)
        {
            isReady = false;
            activarPowerUp();
        }
    }


    public void activarPowerUp()
    {
        isActivo = true;
        animator.gameObject.SetActive(true);
        animator.SetBool("isActive", true);
        OnPowerChanged?.Invoke(true); // <<< ON
        //primer multiplicador minimo es 2
        int first = Mathf.Max(powerUpMultiplier,2);
        scoreManager.SetMultiplier(first);
        uiManager?.UpdateMultiplier(first);
        if(uiManager?.multiplierText)
            uiManager.multiplierText.gameObject.SetActive(true);
        //se reinicia la barra para poder volver a llenarla  y activar el otro multiplicador
        currentPowerUpValue = 0;
        uiManager?.UpdatePowerUp(currentPowerUpValue, comboToFill);

    }

    public void stackMultiplier()
    {
        int next = scoreManager.currentMultiplier + Mathf.Max(1, incrementadorPorLLenado);
        if (maxMultiplier > 0) next = Mathf.Min(next, maxMultiplier);

        scoreManager.SetMultiplier(next);
        uiManager?.UpdateMultiplier(next);

        // Reinicia barra para seguir apilando
        currentPowerUpValue = 0;
        uiManager?.UpdatePowerUp(currentPowerUpValue, comboToFill);
        // (Opcional) feedback/animación aquí
    }
}