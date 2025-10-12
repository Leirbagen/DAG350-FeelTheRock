using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [Header("Configuración de Vida")]
    public int maxHealth = 7;
    public int currentHealth;

    [Header("Configuración de Curación")]
    public int healComboThreshold = 3; // Cada cuántas notas seguidas se cura
    private int comboCounterForHealing = 0;

    void Start()
    {
        currentHealth = maxHealth;
    }

    // Se llama cuando se acierta una nota
    public void OnNoteHit()
    {
        comboCounterForHealing++;
        if (comboCounterForHealing >= healComboThreshold)
        {
            Heal(1);
            comboCounterForHealing = 0; // Reseteamos el contador
        }
    }

    // Se llama cuando se falla una nota
    public void OnNoteMiss()
    {
        currentHealth--;
        comboCounterForHealing = 0; // Se rompe el combo de curación

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            GameManager.instance.OnGameOver();
        }
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        comboCounterForHealing = 0;
    }

    // Método para curar vida
    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
    public void Damage(int amount)
{
    currentHealth -= amount;

    if (currentHealth <= 0)
    {
        currentHealth = 0;
        GameManager.instance.OnGameOver(); // 🔥 llama al GameOver
    }
}
}
