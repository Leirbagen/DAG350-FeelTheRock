using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuColecciones70 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public void BlackSabbath()
    {
        SceneManager.LoadScene("InfoBlackSabbath");
    }

    public void DeepPurple()
    {
        SceneManager.LoadScene("InfoDeepPurple");

    }
    public void LedZeppelin()
    {
        SceneManager.LoadScene("InfoLedZeppelin");

    }
    public void PinkFloyd()
    {
        SceneManager.LoadScene("InfoPinkFloyd");

    }
    public void Queen()
    {
        SceneManager.LoadScene("InfoQueen");

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
