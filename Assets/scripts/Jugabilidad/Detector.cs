using System.Collections;
using UnityEngine;

public class Detector : MonoBehaviour
{
    public int laneID;
    private GameObject noteInTrigger = null;
    private AudioManager audioManager;
    public bool HasNote() => !(noteInTrigger == null || noteInTrigger.Equals(null));

    [Header("Animación de hit")]
    [SerializeField] private string hitTriggerName = "Pressed";   // si usas Trigger
    [SerializeField] private string hitBoolName = "ispresionado"; // si usas bool
    [SerializeField] private int animatorLayerIndex = 0;          // capa del Animator donde está el estado de hit
    [SerializeField] private float failSafeDestroy = 0.25f;       // fallback si no se puede leer duración (en segundos)
    private void Start()
    {
        audioManager = FindFirstObjectByType<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Nota"))
        {
            noteInTrigger = other.gameObject;

            
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == noteInTrigger)
        {
            var mov = other.GetComponent<Nota_Movimiento>();
            if (mov == null || !mov.judged)
            {
                audioManager?.MissNote(laneID);
                audioManager?.PlayMissSound();
            }
            noteInTrigger = null;
        }
    }

    public bool TryHitNote(bool type)
    {
        if (noteInTrigger == null || noteInTrigger.Equals(null))
        {
            noteInTrigger = null;
            return false;
        }

        var noteToDestroy = noteInTrigger;
        noteInTrigger = null;

        var mov = noteToDestroy.GetComponent<Nota_Movimiento>();
        if (mov) mov.MarkAsHit();

        StartCoroutine(PlayHitAnimAndDestroy(mov.gameObject));

        return true;
    }
    private IEnumerator PlayHitAnimAndDestroy(GameObject noteGO)
    {
        if (noteGO == null) yield break;

        var col = noteGO.GetComponent<Collider2D>();
        if (col) col.enabled = false;

        var anim = noteGO.GetComponent<Animator>();
        if (anim == null) anim = noteGO.GetComponentInChildren<Animator>();

        float waitTime = failSafeDestroy;

        if (anim != null)
        {
            if (!string.IsNullOrEmpty(hitTriggerName))
            {
                anim.ResetTrigger(hitTriggerName);
                anim.SetTrigger(hitTriggerName);
            }


            yield return null;

            var st = anim.GetCurrentAnimatorStateInfo(animatorLayerIndex);
            if (st.length > 0.01f) waitTime = st.length;
        }

        yield return new WaitForSeconds(waitTime);

        if (noteGO != null) Destroy(noteGO);
    }
}