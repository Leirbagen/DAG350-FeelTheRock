using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicial : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void iniciar()
    {
        SceneManager.LoadScene("MenuDecadas");
    }

    public void colecciones()
    {
        SceneManager.LoadScene("MenuColecciones");
    }

    public void Salir()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {

    }
}

