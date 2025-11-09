using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuNiveles80 : MonoBehaviour
{
    void Start()
    {

    }

    public void Metallica()
    {
        SceneManager.LoadScene("Metallica");
    }

    public void IronMaiden()
    {
        SceneManager.LoadScene("IronMaiden");

    }
    public void GunsNRoses()
    {
        SceneManager.LoadScene("GunsNRoses");

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
