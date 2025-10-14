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

    private int nextNoteIndex = 0;
    private bool isSpawning = false;
    public AudioManager audioManager;

    private void Awake()
    {
        audioManager = AudioManager.instance;
    }


    void Update()
    {
        if (!isSpawning || currentSong == null || currentSong.notes == null || currentSong.notes.Count == 0)
            return;

        float songTime = audioManager != null ? audioManager.GetSongDspTime() : Time.time;

        while (nextNoteIndex < currentSong.notes.Count)
        {
            var n = currentSong.notes[nextNoteIndex];

            // [MOD] Lanzar con antelación fallTime para llegar justo en spawnTime al detector
            if (songTime >= (n.spawnTime - fallTime))
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
        var movimiento = go.GetComponent<Nota_Movimiento>();
        movimiento.Initialize(start, end, fallTime, noteData.laneID);
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
        nextNoteIndex = 0;
        isSpawning = true;
    }

    //  Inicia normalmente
    public void StartSpawning()
    {
        isSpawning = true;
    }

}
