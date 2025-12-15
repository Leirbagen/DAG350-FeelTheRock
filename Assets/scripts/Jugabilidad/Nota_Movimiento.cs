using UnityEngine;


public class Nota_Movimiento : MonoBehaviour
{
    private Vector3 startPos;
    private Vector3 endPos;
    private float fallTime;
    public int LaneID;
    //variable para evitar doble conteo entre el hit y el miss
    public bool judged = false;

    private float timeElapsed = 0f;

    public void Initialize(Vector3 start, Vector3 end, float time, int laneID)
    {
        startPos = start;
        endPos = end;
        fallTime = time;
        judged = false;
        LaneID = laneID;
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
            GameManager.instance?.OnNoteMiss(LaneID);
            Destroy(gameObject);
        }
    }
}
