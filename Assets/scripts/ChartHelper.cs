using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor; // Necesario para modificar assets
#endif

public class ChartHelper : MonoBehaviour
{
    [System.Serializable]
    public class NoteData
    {
        public int laneID;
        public float spawnTime;
    }

    [Header("Configuración")]
    public AudioSource backingTrackSource;
    public KeyCode[] inputKeys;

    [Header("¡Nuevo! Arrastra tu SongChart aquí")]
    public SongChart targetSongChart; // La referencia al asset que vamos a modificar

    private bool isPlaying = false;
    private List<SongChart.NoteData> recordedNotes = new List<SongChart.NoteData>();
    
    void Update()
    {
        // Tecla P para iniciar/detener la música
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePlayback();
        }

        // Tecla G para guardar el chart en el asset
        if (Input.GetKeyDown(KeyCode.G))
        {
            SaveChartToAsset();
        }

        if (!isPlaying) return;

        // Escuchamos las teclas de los carriles para grabar notas
        for (int i = 0; i < inputKeys.Length; i++)
        {
            if (Input.GetKeyDown(inputKeys[i]))
            {
                RecordNote(i);
            }
        }
    }

    void TogglePlayback()
    {
        if (isPlaying)
        {
            backingTrackSource.Stop();
            isPlaying = false;
            Debug.Log("Playback detenido. Presiona 'G' para guardar el chart o 'P' para grabar de nuevo.");
        }
        else
        {
            recordedNotes.Clear();
            backingTrackSource.Play();
            isPlaying = true;
            Debug.Log("Iniciando grabación... Toca las notas al ritmo. Presiona 'P' para detener.");
        }
    }

    void RecordNote(int laneIndex)
    {
        float hitTime = backingTrackSource.time;
        
        SongChart.NoteData newNote = new SongChart.NoteData { laneID = laneIndex, spawnTime = hitTime };
        recordedNotes.Add(newNote);

        Debug.Log($"Nota grabada: laneID: {laneIndex}, spawnTime: {hitTime:F3}f");
    }

    void SaveChartToAsset()
    {
        if (targetSongChart == null)
        {
            Debug.LogError("¡ERROR! No has asignado un 'Target Song Chart' en el Inspector del ChartHelper.");
            return;
        }

        if (recordedNotes.Count == 0)
        {
            Debug.LogWarning("No hay notas grabadas para guardar. ¡Toca algunas primero!");
            return;
        }

        // Asignamos la lista de notas grabadas directamente a la lista del ScriptableObject
        targetSongChart.notes = new List<SongChart.NoteData>(recordedNotes);

#if UNITY_EDITOR
        // Marcamos el asset como "sucio" para que Unity sepa que necesita guardar los cambios
        EditorUtility.SetDirty(targetSongChart);
        // Guardamos todos los cambios en los assets
        AssetDatabase.SaveAssets();
#endif

        Debug.Log($"<color=lime>¡ÉXITO!</color> Se han guardado {recordedNotes.Count} notas en el SongChart '{targetSongChart.name}'.");
    }
}