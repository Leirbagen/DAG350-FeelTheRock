using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuColecciones90 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public void Korn()
    {
        SceneManager.LoadScene("InfoKorn");
    }

    public void Nirvana()
    {
        SceneManager.LoadScene("InfoNirvana");

    }
    public void Radiohead()
    {
        SceneManager.LoadScene("InfoRadiohead");

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
