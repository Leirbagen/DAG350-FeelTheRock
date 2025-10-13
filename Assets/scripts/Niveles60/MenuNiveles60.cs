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

    }
    public void TheDoors()
    {

    } 

    public void Atras()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
