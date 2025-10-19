using System.Collections;
using UnityEngine;

public class Detector : MonoBehaviour
{
    public int laneID;
    private GameObject noteInTrigger = null;
    private AudioManager audioManager;
    public bool HasNote() => !(noteInTrigger == null || noteInTrigger.Equals(null));

    // NOMBRES configurables (ajusta a tus nombres reales del Animator)
    [Header("Animación de hit")]
    [SerializeField] private string hitTriggerName = "Pressed";   // si usas Trigger
    [SerializeField] private string hitBoolName = "ispresionado"; // si usas bool
    [SerializeField] private int animatorLayerIndex = 0;          // capa del Animator donde está el estado de hit
    [SerializeField] private float failSafeDestroy = 0.25f;       // fallback si no se puede leer duración (en segundos)
    private void Start()
    {
        // Usamos FindFirstObjectByType que es más moderno.
        audioManager = FindFirstObjectByType<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Solo reaccionamos a objetos con el Tag "Nota".
        if (other.CompareTag("Nota"))
        {
            noteInTrigger = other.gameObject;

            
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Si la nota que sale es la que teníamos registrada, es un fallo.
        if (other.gameObject == noteInTrigger)
        {
            var mov = other.GetComponent<Nota_Movimiento>();
            // Si ya fue golpeada, NO es Miss
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
        // Unity "null" especial: objeto destruido pero referencia no es null puro
        if (noteInTrigger == null || noteInTrigger.Equals(null))
        {
            noteInTrigger = null;
            return false;
        }

        // Golpe válido
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

        // Desactiva colisiones para que no se vuelva a contar
        var col = noteGO.GetComponent<Collider2D>();
        if (col) col.enabled = false;

        // Busca Animator en la nota o en sus hijos
        var anim = noteGO.GetComponent<Animator>();
        if (anim == null) anim = noteGO.GetComponentInChildren<Animator>();

        float waitTime = failSafeDestroy;

        if (anim != null)
        {
            // ——— Activa la condición en el Animator ———
            // OPCIÓN TRIGGER (recomendada):
            if (!string.IsNullOrEmpty(hitTriggerName))
            {
                anim.ResetTrigger(hitTriggerName);
                anim.SetTrigger(hitTriggerName);
            }

            // OPCIÓN BOOL (si prefieres tu bool ispresionado):
            // if (!string.IsNullOrEmpty(hitBoolName))
            //     anim.SetBool(hitBoolName, true);

            // Espera un frame para que el Animator entre al estado de hit
            yield return null;

            // Intenta leer la duración real del estado actual
            var st = anim.GetCurrentAnimatorStateInfo(animatorLayerIndex);
            if (st.length > 0.01f) waitTime = st.length;

            // Si usaste bool, puedes apagarlo al final (opcional)
            // yield return new WaitForSeconds(waitTime);
            // if (!string.IsNullOrEmpty(hitBoolName))
            //     anim.SetBool(hitBoolName, false);
        }

        // Espera el tiempo calculado (o fallback)
        yield return new WaitForSeconds(waitTime);

        // Destruye la nota
        if (noteGO != null) Destroy(noteGO);
    }
}