using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Nuevo Mapa de cancion", menuName = "Juego de Ritmo/ SongChart")]
public class SongChart : ScriptableObject
{
    [Header("Archivos de Audiossss")]
    public AudioClip backingTrackClip;
    [FormerlySerializedAs("Instrument Tracks")]
    public AudioClip instrumentalClip;

    // Esta es la "plantilla" de la nota que el NoteSpawner necesita.
    [System.Serializable]
    public class NoteData
    {
        public float spawnTime; //segundos
        public int laneID; // los botones: 0,1,2,3....n
    }

    [Header("Mapa de Notas")]
    public List<NoteData> notes = new List<NoteData>();
}
