using UnityEngine;

// Versión simplificada para solo notas cortas.
public class Nota : MonoBehaviour
{
    [Header("Configuración de la Nota")]
    public int laneID;
    public PowerUpManager powerUpManager;
    [Header("Animación / Skin")]
    [SerializeField] public Animator anim;
    public RuntimeAnimatorController normalController;
    public RuntimeAnimatorController powerController;

    [Header("Opcional - Cambiar color/material")]
    [SerializeField] private SpriteRenderer sr;
    public Color normalTint = Color.white;
    public Color powerTint = Color.white;
    void Reset() => anim = GetComponent<Animator>();

    void Awake()
    {
        if (!anim) anim = GetComponent<Animator>();
        if (!powerUpManager)
            powerUpManager = FindObjectOfType<PowerUpManager>();
    }

    void OnEnable()
    {
        if (powerUpManager != null)
        {
            powerUpManager.OnPowerChanged += ApplySkin;
            ApplySkin(powerUpManager.isActivo);
        }
    }

    void OnDisable()
    {
        if (powerUpManager != null)
            powerUpManager.OnPowerChanged -= ApplySkin;
    }

    private void ApplySkin(bool on)
    {
        if (anim)
            anim.runtimeAnimatorController = on ? powerController : normalController;
            
        if (sr)
            sr.color = on ? powerTint : normalTint;
    }



    // La función Hit ahora es muy simple: solo destruye la nota.
    public void Hit()
    {
        // Inmediatamente destruimos el objeto de la nota al ser tocada.
        Destroy(gameObject);
    }
 
}
