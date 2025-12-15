using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetProgressButton : MonoBehaviour
{

    public Canvas pantalla;
    private void RefreshCanvas()
    {
        if (pantalla == null) return;

        GameObject root = pantalla.gameObject;
        root.SetActive(false);
        root.SetActive(true);
    }
    // Puedes llamarlo desde el OnClick() del botón en el Inspector
    public void ResetProgress()
    {
        // Borra todos los datos guardados (estrellas, puntajes, completado)
        GameProgress.Instance.ResetAllProgress();

        // Si estás en el menú de niveles, refresca la vista
        var controller = FindFirstObjectByType<LevelSelectController>();
        if (controller != null)
            controller.Refresh();
        RefreshCanvas();
        print("Progreso reiniciado.");
    }
}
