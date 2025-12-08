using UnityEngine;
using UnityEngine.Audio;

public class MenuOpciones : MonoBehaviour
{

    [SerializeField]
    private AudioMixer audioMixer;

    void Start()
    {
        
    }


    public void pantallaCompleta(bool pantallaCompleta)
    {
                Screen.fullScreen = pantallaCompleta;
    }

    public void cambiarCalidad(int index)
    {
        QualitySettings.SetQualityLevel(index); 
    }


    public void cambiarVolumen(float volumen)
    {
        audioMixer.SetFloat("Volumen", volumen);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
