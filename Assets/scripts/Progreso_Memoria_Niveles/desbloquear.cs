using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CheatCodeUnlockAll : MonoBehaviour
{
    [Header("Código secreto (lo que debe escribir el jugador)")]
    public string cheatCode = "dag350feeltherock";

    [Header("Lista de TODOS los levelID del juego")]
    public string[] allLevelIds;
    [Header("el texto de alerta")]
    public GameObject unlockMessageUI;
    public float messageDuration = 2f;
    private string currentInput = "";
    public Canvas pantalla;

    void Update()
    {
        foreach (char c in Input.inputString)
        {
            // Ignorar teclas de control (Enter, Backspace, etc.)
            if (char.IsControl(c))
                continue;

            char lower = char.ToLower(c);

            // Aceptar letras y números
            if (char.IsLetterOrDigit(lower))
            {
                currentInput += lower;

                // Mantener el largo máximo igual al del cheatCode
                if (currentInput.Length > cheatCode.Length)
                {
                    currentInput = currentInput.Substring(currentInput.Length - cheatCode.Length);
                }

                // Comparar siempre en minúsculas
                if (currentInput == cheatCode.ToLower())
                {
                    Debug.Log("CHEAT CODE DETECTADO");
                    ActivateCheat();
                    currentInput = "";
                }
            }
        }
    }

    private void ActivateCheat()
    {
        if (GameProgress.Instance == null)
        {
            print("CheatCodeUnlockAll: No hay GameProgress en escena.");
            return;
        }

        if (allLevelIds == null || allLevelIds.Length == 0)
        {
            print("CheatCodeUnlockAll: no hay levelIds configurados.");
            return;
        }

        foreach (var lid in allLevelIds)
        {
            if (string.IsNullOrEmpty(lid)) continue;

            // Les damos 3 estrellas, puntaje alto, y los marcamos como completados
            GameProgress.Instance.ReportResult(
                lid,
                score: 999999,
                stars: 3,
                markCompleted: true
            );
        }
        RefreshCanvas();
        StartCoroutine(ShowUnlockMessage());
        print("codigo activado: TODOS los niveles desbloqueados.");
    }

    private void RefreshCanvas()
    {
        if (pantalla == null) return;

        GameObject root = pantalla.gameObject;
        root.SetActive(false);
        root.SetActive(true);
    }

    private IEnumerator ShowUnlockMessage()
    {
        if (unlockMessageUI == null) yield break;

        unlockMessageUI.SetActive(true);

        CanvasGroup cg = unlockMessageUI.GetComponent<CanvasGroup>();
        if (cg == null) cg = unlockMessageUI.AddComponent<CanvasGroup>();

        // Reset alpha
        cg.alpha = 1;

        // Mostrar por unos segundos
        yield return new WaitForSeconds(messageDuration);

        // Fade out
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime;
            cg.alpha = 1 - t;
            yield return null;
        }

        unlockMessageUI.SetActive(false);
    }
}

