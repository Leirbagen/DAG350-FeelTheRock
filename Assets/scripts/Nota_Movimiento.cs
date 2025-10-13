using UnityEngine;

// NO HAY CAMBIOS EN ESTE SCRIPT, PERO LO INCLUYO PARA QUE TENGAS EL PAQUETE COMPLETO
// PUEDES VERIFICAR QUE TU CÓDIGO SEA IGUAL A ESTE.
public class Nota_Movimiento : MonoBehaviour
{
    private Vector3 startPos;
    private Vector3 endPos;
    private float fallTime;
    //variable para evitar doble conteo entre el hit y el miss
    private bool judged = false;

    private float timeElapsed = 0f;

    public void Initialize(Vector3 start, Vector3 end, float time)
    {
        startPos = start;
        endPos = end;
        fallTime = time;
        judged = false;
    }
    public void MarkAsHit()
    {
        judged = true;
    }

    void Update()
    {
        if (fallTime <= 0) return;

        timeElapsed += Time.deltaTime;
        float progress = timeElapsed / fallTime;
        transform.position = Vector3.Lerp(startPos, endPos, progress);

        if (progress >= 1.05f) 
        {
            
            Destroy(gameObject);
        }
    }
}
