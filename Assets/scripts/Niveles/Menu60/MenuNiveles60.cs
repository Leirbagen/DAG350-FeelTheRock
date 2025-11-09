using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuNiveles60 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void TheBeatles()
    {
        SceneManager.LoadScene("TheBeatles_LetItBe");
    }

    public void TheRollingStones()
    {
        SceneManager.LoadScene("TheRollingStones");

    }
    public void TheDoors()
    {
        SceneManager.LoadScene("TheDoors");

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
