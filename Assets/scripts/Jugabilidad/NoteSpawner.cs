using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    [Header("Configuración del Chart")]
    public SongChart currentSong;
    public float fallTime = 2f; // Puedes ajustar esto para cambiar la velocidad de las notas
    [Header("Referencias de Carriles")]
    public GameObject[] notePrefabs; // Un prefab por carril
    public Transform[] laneStartPositions;
    public Transform[] laneEndPositions;
    [Header("Delay global del chart")]

    public int nextNoteIndex;
    private bool isSpawning = false;
    public AudioManager audioManager;
    private float spawnerAnchorTime = 0;
    List<GameObject> alive= new();
    IReadOnlyList<SongChart.NoteData> notes;
    public GameManager gameManager;


    private void Start()
    {
        nextNoteIndex = 0;
        audioManager = AudioManager.instance;

    }
    private void Awake()
    {
    }


    void Update()
    {
        if (!isSpawning || currentSong == null || currentSong.notes == null || currentSong.notes.Count == 0)
            return;

        float songTimeAbsolute = audioManager != null ? audioManager.GetSongDspTime() : Time.time;
        if (songTimeAbsolute < 0f) return;

        float delay = (currentSong != null) ? currentSong.chartDelay : 0f;
        if (songTimeAbsolute < delay) return; // aún no empieza el chart

        float sontTimeRelative = (songTimeAbsolute - delay) - spawnerAnchorTime;

        while (nextNoteIndex < currentSong.notes.Count)
        {
            var n = currentSong.notes[nextNoteIndex];
            // [MOD] Lanzar con antelación fallTime para llegar justo en spawnTime al detector
            if (sontTimeRelative >= (n.spawnTime - fallTime))
            {
                SpawnNote(n);
                nextNoteIndex++;
            }
            else
            {
                break;
            }
        }
    }

    void SpawnNote(SongChart.NoteData noteData)
    {
        int lane = Mathf.Clamp(noteData.laneID, 0, laneStartPositions.Length - 1);
        var start = laneStartPositions[lane].position;
        var end = laneEndPositions[lane].position;
        var prefab = notePrefabs[lane];

        var go = Instantiate(prefab, start, Quaternion.identity);
        

        alive.Add(go);
        var movimiento = go.GetComponent<Nota_Movimiento>();
        movimiento.Initialize(start, end, fallTime, noteData.laneID);
    }

    //resetea el chart
    public void ResetChartOnly(bool despawnNotes)
    {
        print ("reseteando chart");
        //  Reinicia índice
        nextNoteIndex = 0;

        //(float)audioManager.GetSongDspTime()
        if (audioManager != null)
            spawnerAnchorTime = (float)audioManager.GetSongDspTime();
        else
            spawnerAnchorTime = Time.time;

        // Limpia notas vivas para que no se acumulen
        if (despawnNotes)
        {
            for (int i = alive.Count - 1; i >= 0; i--)
            {
                if (alive[i] != null) Destroy(alive[i]);
            }
        }
        alive.Clear();
    }

    //   Detiene el spawn de notas
    public void StopSpawning()
    {
        print("aaaa parame");
        isSpawning = false;
    }

    //  Reinicia los valores y vuelve a spawnear desde el inicio
    public void ResetAndStart()
    {
        ResetChartOnly(true);
        isSpawning = true;
    }

    //  Inicia normalmente
    public void StartSpawning()
    {
        isSpawning = true;
    }

}
