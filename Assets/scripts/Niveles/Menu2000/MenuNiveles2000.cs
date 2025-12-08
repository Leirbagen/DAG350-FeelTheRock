using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuNiveles2000 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public void LinkinPark()
    {
        SceneManager.LoadScene("LinkinPark");
    }

    public void SystemOfaDown()
    {
        SceneManager.LoadScene("SystemOfaDown");

    }
    public void ArcticMonkeys()
    {
        SceneManager.LoadScene("ArcticMonkeys");

    }
    public void Atras()
    {
        SceneManager.LoadScene("MenuDecadas");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
