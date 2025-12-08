using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuNiveles90 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public void Korn()
    {
        SceneManager.LoadScene("Korn");
    }

    public void Nirvana()
    {
        SceneManager.LoadScene("Nirvana");

    }
    public void Radiohead()
    {
        SceneManager.LoadScene("Radiohead");

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
