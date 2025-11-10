using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorBotones : MonoBehaviour
{
    public List<GameObject> botones;  

    void Start()
    {
        StartCoroutine(ActivarBotonesDespuesDeTiempo());
    }

    IEnumerator ActivarBotonesDespuesDeTiempo()
    {
        yield return new WaitForSeconds(2f);  

        // Opción A: usando foreach
        foreach (GameObject boton in botones)
        {
            boton.SetActive(true);
        }
    }
}
