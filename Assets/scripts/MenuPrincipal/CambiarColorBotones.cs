using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

[DisallowMultipleComponent]
[RequireComponent(typeof(Button))]
public class CambiarColorBotones : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("References")]
    [SerializeField] private Button button;                 // Button 
    [SerializeField] private TextMeshProUGUI tmpLabel;      // Si usas TMP
    [SerializeField] private Text uiTextLabel;              // Si usas Text (legacy)

    [Header("Colors")]
    [SerializeField] private Color normal = Color.white;
    [SerializeField] private Color highlighted = new Color(1f, 0.95f, 0.2f); // amarillo suave
    [SerializeField] private Color pressed = new Color(1f, 0.8f, 0.1f);
    [SerializeField] private Color disabled = new Color(1f, 1f, 1f, 0.5f);

    private bool isHovering = false;
    private bool lastInteractable = true;

    #region Unity lifecycle
    private void Reset()
    {
        button = GetComponent<Button>();
        // Busca primero TMP, luego Text estándar
        if (!tmpLabel) tmpLabel = GetComponentInChildren<TextMeshProUGUI>(true);
        if (!uiTextLabel) uiTextLabel = GetComponentInChildren<Text>(true);
    }

    private void Awake()
    {
        if (!button) button = GetComponent<Button>();
        if (!tmpLabel && !uiTextLabel)
        {
            // fallback por si el texto está en un nieto
            tmpLabel = GetComponentInChildren<TextMeshProUGUI>(true);
            uiTextLabel = GetComponentInChildren<Text>(true);
        }
        lastInteractable = button ? button.interactable : true;
    }

    private void OnEnable()
    {
        Apply(lastInteractable ? normal : disabled);
    }

    private void Update()
    {
        if (!button) return;

        // Si cambia el estado interactable en runtime, ajusta color
        if (button.interactable != lastInteractable)
        {
            lastInteractable = button.interactable;
            Apply(lastInteractable ? (isHovering ? highlighted : normal) : disabled);
        }
    }
    #endregion

    #region Pointer handlers
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!IsUsable()) return;
        isHovering = true;
        Apply(highlighted);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!IsUsable()) return;
        isHovering = false;
        Apply(normal);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!IsUsable() || eventData.button != PointerEventData.InputButton.Left) return;
        Apply(pressed);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!IsUsable()) return;
        // Vuelve a highlighted si el cursor sigue encima; si no, a normal
        Apply(isHovering ? highlighted : normal);
    }
    #endregion

    #region Helpers
    private bool IsUsable()
    {
        return button != null && button.interactable && (tmpLabel || uiTextLabel);
    }

    private void Apply(Color c)
    {
        if (tmpLabel) tmpLabel.color = c;
        if (uiTextLabel) uiTextLabel.color = c;
    }
    #endregion
}



