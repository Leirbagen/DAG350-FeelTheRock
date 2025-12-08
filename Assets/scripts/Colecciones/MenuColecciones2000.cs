using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuColecciones2000 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public void ArcticMonkeys()
    {
        SceneManager.LoadScene("InfoArcticMonkeys");
    }

    public void LinkinPark()
    {
        SceneManager.LoadScene("InfoLinkinPark");

    }
    public void SystemOfaDown()
    {
        SceneManager.LoadScene("InfoSystemOfaDown");

    }
    public void Atras()
    {
        SceneManager.LoadScene("MenuColecciones");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
