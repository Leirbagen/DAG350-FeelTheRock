using UnityEngine;
using UnityEngine.SceneManagement;
public class ColeccionAtras : MonoBehaviour
{
    string currentScene;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;
    }


    public void Atras()
    {
        if(currentScene== "InfoTheBeatles" || currentScene == "InfoTheDoors" || currentScene == "InfoTheRollingStones")
        {
            SceneManager.LoadScene("Coleccion60");
        }
        if(currentScene == "InfoBlackSabbath" || currentScene == "InfoDeepPurple" || currentScene == "InfoLedZeppelin" || currentScene == "InfoPinkFloyd" || currentScene == "InfoQueen")
        {
            SceneManager.LoadScene("Coleccion70");
        }
        if (currentScene == "InfoGunsNRoses" || currentScene == "InfoIronMaiden" || currentScene == "InfoMetallica")
        {
            SceneManager.LoadScene("Coleccion80");
        }
        if (currentScene == "InfoKorn" || currentScene == "InfoNirvana" || currentScene == "InfoRadiohead")
        {
            SceneManager.LoadScene("Coleccion90");
        }
        if (currentScene == "InfoArcticMonkeys" || currentScene == "InfoLinkinPark" || currentScene == "InfoSystemOfaDown")
        {
            SceneManager.LoadScene("Coleccion2000");
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
