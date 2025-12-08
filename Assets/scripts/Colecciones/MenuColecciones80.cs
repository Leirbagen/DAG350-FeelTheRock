using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuColecciones80 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public void GunsNRoses()
    {
        SceneManager.LoadScene("InfoGunsNRoses");
    }

    public void IronMaiden()
    {
        SceneManager.LoadScene("InfoIronMaiden");

    }
    public void Metallica()
    {
        SceneManager.LoadScene("InfoMetallica");

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
