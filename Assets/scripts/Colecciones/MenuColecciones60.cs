using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuColecciones60 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void TheBeatles()
    {
        SceneManager.LoadScene("InfoTheBeatles");
    }

    public void TheRollingStones()
    {
        SceneManager.LoadScene("InfoTheRollingStones");

    }
    public void TheDoors()
    {
        SceneManager.LoadScene("InfoTheDoors");

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
