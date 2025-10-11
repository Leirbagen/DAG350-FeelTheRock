using UnityEngine;

// NO HAY CAMBIOS EN ESTE SCRIPT, PERO LO INCLUYO PARA QUE TENGAS EL PAQUETE COMPLETO
// PUEDES VERIFICAR QUE TU CÓDIGO SEA IGUAL A ESTE.
public class Nota_Movimiento : MonoBehaviour
{
    private Vector3 startPos;
    private Vector3 endPos;
    private float fallTime;

    private float timeElapsed = 0f;

    public void Initialize(Vector3 start, Vector3 end, float time)
    {
        startPos = start;
        endPos = end;
        fallTime = time;
    }

    void Update()
    {
        if (fallTime <= 0) return;

        timeElapsed += Time.deltaTime;
        float progress = timeElapsed / fallTime;
        transform.position = Vector3.Lerp(startPos, endPos, progress);

        if (progress >= 1.1f) 
        {
            Destroy(gameObject);
        }
    }
}
