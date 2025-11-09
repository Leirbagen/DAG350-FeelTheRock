using UnityEngine;
using UnityEngine.UI;

public class StarUI : MonoBehaviour
{
    public enum DisplayMode { AnimatedHUD, StaticSprites }

    [Header("Common")]
    [SerializeField] private DisplayMode mode = DisplayMode.AnimatedHUD;
    [SerializeField] private Image[] stars;

    [Header("StaticSprites (Pause/End)")]
    [SerializeField] private Sprite offSprite;      // Asigna sprite “apagado”
    [SerializeField] private Sprite onSprite;       // Asigna sprite “encendido”
    [SerializeField] private bool setNativeSize = false; // opcional

    // Anim HUD caches
    private Animator[] anims;
    private RectTransform[] rts;
    private Vector2[] initialAnchoredPos;
    private Vector3[] initialLocalScale;
    private Quaternion[] initialLocalRot;
    private Transform[] initialParents;

    private int currentShown = 0;
    private bool initialized = false;

    private void EnsureInit()
    {
        if (initialized) return;

        int n = stars?.Length ?? 0;

        if (mode == DisplayMode.AnimatedHUD)
        {
            anims = new Animator[n];
            rts = new RectTransform[n];
            initialAnchoredPos = new Vector2[n];
            initialLocalScale = new Vector3[n];
            initialLocalRot = new Quaternion[n];
            initialParents = new Transform[n];

            for (int i = 0; i < n; i++)
            {
                if (stars[i] == null) continue;

                stars[i].TryGetComponent(out anims[i]);
                var rt = stars[i].rectTransform;
                rts[i] = rt;

                initialParents[i] = stars[i].transform.parent;
                initialAnchoredPos[i] = rt.anchoredPosition;
                initialLocalScale[i] = rt.localScale;
                initialLocalRot[i] = rt.localRotation;

                // En HUD las estrellas empiezan ocultas
                stars[i].gameObject.SetActive(false);
            }
        }
        else
        {
            // En modo estático nos aseguramos que existan sprites
            if (onSprite == null || offSprite == null)
                Debug.LogWarning($"StarUI ({name}): Asigna onSprite y offSprite para StaticSprites.");
            // En Pausa/Final queremos que siempre estén visibles con estado "apagado" por defecto
            for (int i = 0; i < n; i++)
            {
                var img = stars[i];
                if (img == null) continue;
                if (offSprite != null) img.sprite = offSprite;
                if (setNativeSize) img.SetNativeSize();
                img.gameObject.SetActive(true);
            }
        }

        initialized = true;
    }

    private void OnEnable()
    {
        EnsureInit();

        // En paneles estáticos, al abrir/reenable, garantizamos “apagado” visual por defecto
        if (mode == DisplayMode.StaticSprites)
            ApplyStatic(0);
    }

    /// <summary>
    /// Actualiza la UI de estrellas al valor 'count'.
    /// - HUD (animado): solo revela de currentShown -> count con animación (no repite lo ya visto).
    /// - Pausa/Final (estático): pinta TODAS según 'count' sin animación.
    /// </summary>
    public void SetStars(int count)
    {
        EnsureInit();

        int n = stars?.Length ?? 0;
        count = Mathf.Clamp(count, 0, n);

        if (mode == DisplayMode.AnimatedHUD)
        {
            if (count <= currentShown) return; // no repetir animación hacia atrás

            for (int i = currentShown; i < count; i++)
            {
                var img = (stars != null && i < n) ? stars[i] : null;
                if (img == null) continue;

                var rt = rts[i];
                if (rt == null) { Debug.LogWarning($"StarUI: RectTransform nulo en índice {i}"); continue; }

                if (rt.parent != initialParents[i])
                    rt.SetParent(initialParents[i], false);

                rt.anchoredPosition = initialAnchoredPos[i];
                rt.localScale = initialLocalScale[i];
                rt.localRotation = initialLocalRot[i];

                if (!img.gameObject.activeSelf)
                    img.gameObject.SetActive(true);

                var anim = anims[i];
                if (anim != null)
                {
                    anim.ResetTrigger("Activate");
                    anim.Rebind();
                    anim.Update(0f);
                    anim.SetTrigger("Activate");
                }
            }

            currentShown = count;
        }
        else
        {
            // Modo estático: siempre refresca TODO para reflejar exactamente 'count'
            ApplyStatic(count);
            currentShown = count; // por consistencia, aunque no lo usamos para evitar early-return
        }
    }

    /// <summary>
    /// Oculta todo (HUD) o apaga todo (Pausa/Final).
    /// </summary>
    public void HideAll()
    {
        EnsureInit();
        int n = stars?.Length ?? 0;

        if (mode == DisplayMode.AnimatedHUD)
        {
            for (int i = 0; i < n; i++)
            {
                var img = stars[i];
                if (img == null) continue;

                var rt = (rts != null && i < rts.Length) ? rts[i] : null;
                if (rt != null && initialParents != null && i < initialParents.Length)
                {
                    rt.SetParent(initialParents[i], false);
                    rt.anchoredPosition = initialAnchoredPos[i];
                    rt.localScale = initialLocalScale[i];
                    rt.localRotation = initialLocalRot[i];
                }

                if (anims != null && i < anims.Length && anims[i] != null)
                {
                    anims[i].ResetTrigger("Activate");
                    anims[i].Rebind();
                    anims[i].Update(0f);
                }

                img.gameObject.SetActive(false);
            }
        }
        else
        {
            // Pausa/Final: no se ocultan, solo vuelven a "apagado"
            for (int i = 0; i < n; i++)
            {
                var img = stars[i];
                if (img == null) continue;
                if (offSprite != null) img.sprite = offSprite;
                if (setNativeSize) img.SetNativeSize();
                img.gameObject.SetActive(true);
            }
        }

        currentShown = 0;
    }

    // ----- Helpers (modo estático) -----
    private void ApplyStatic(int count)
    {
        int n = stars?.Length ?? 0;

        for (int i = 0; i < n; i++)
        {
            var img = stars[i];
            if (img == null) continue;

            // Encendidos primeros 'count', el resto apagados
            var target = (i < count) ? onSprite : offSprite;
            if (target != null) img.sprite = target;
            if (setNativeSize) img.SetNativeSize();
            if (!img.gameObject.activeSelf) img.gameObject.SetActive(true);
        }
    }
}
