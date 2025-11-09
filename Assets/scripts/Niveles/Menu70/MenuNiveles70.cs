using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuNiveles70 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void LedZeppelin()
    {
        SceneManager.LoadScene("LedZeppelin");
    }

    public void BlackSabbath()
    {
        SceneManager.LoadScene("BlackSabbath");

    }
    public void DeepPurple()
    {
        SceneManager.LoadScene("DeepPurple");

    }
    public void PinkFloyd()
    {
        SceneManager.LoadScene("PinkFloyd");

    }
    public void Queen()
    {
        SceneManager.LoadScene("Queen");

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
