using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public List<AudioSource> instrumentSources;
    public AudioSource backingTrackSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip missSoundClip;

    [Header("Configuración de Volumen")]
    public float volumeFadeSpeed = 10f;
    private float[] targetInstrumentVolumes;

    public static AudioManager instance;

    // 🔹 Estado de reproducción sincronizada
    private bool started = false;
    private double startDspTime; // marca de inicio en DSP

    private void Awake()
    {
        if (instance == null) instance = this;
        else { Destroy(gameObject); return; }

        // Asegura tamaños correctos aunque cambie la cantidad de instrumentos
        int count = instrumentSources != null ? instrumentSources.Count : 0;
        targetInstrumentVolumes = new float[count];
        for (int i = 0; i < count; i++) targetInstrumentVolumes[i] = 0f;
    }

    private void Update()
    {
        // Suaviza el cambio de volumen de los instrumentos
        if (instrumentSources == null) return;
        for (int i = 0; i < instrumentSources.Count; i++)
        {
            var src = instrumentSources[i];
            if (src == null) continue;
            src.volume = Mathf.Lerp(src.volume, targetInstrumentVolumes[i], Time.deltaTime * volumeFadeSpeed);
        }
    }

    // ⚠️ Deja de reproducir aquí; solo asigna clips.
    public void SetupSong(SongChart song)
    {
        if (song == null) return;

        if (backingTrackSource != null)
            backingTrackSource.clip = song.backingTrackClip;

        int tracks = Mathf.Min(instrumentSources.Count, song.instrumentTracks.Count);
        for (int i = 0; i < tracks; i++)
        {
            if (instrumentSources[i] == null) continue;
            instrumentSources[i].clip = song.instrumentTracks[i];
            instrumentSources[i].loop = false; // ajusta a tu gusto
            targetInstrumentVolumes[i] = 0f;   // empieza silenciado
        }
    }

    // 🔸 Arrancar TODO sincronizado en una misma marca de tiempo DSP
    public void StartAllSynced(double leadTime = 0.1)
    {
        if (started) return;
        startDspTime = AudioSettings.dspTime + leadTime;

        if (backingTrackSource != null && backingTrackSource.clip != null)
            backingTrackSource.PlayScheduled(startDspTime);

        if (instrumentSources != null)
        {
            for (int i = 0; i < instrumentSources.Count; i++)
            {
                var src = instrumentSources[i];
                if (src != null && src.clip != null)
                    src.PlayScheduled(startDspTime);
            }
        }

        started = true;
    }

    // 🔸 Detener TODO (para reiniciar limpio)
    public void StopAllAudio()
    {
        if (backingTrackSource) backingTrackSource.Stop();
        if (instrumentSources != null)
        {
            foreach (var s in instrumentSources) if (s) s.Stop();
        }
        started = false;
    }

    // 🔸 Pausar / Reanudar (para GameOver o Pausa)
    public void PauseAll()
    {
        backingTrackSource?.Pause();
        if (instrumentSources != null)
            foreach (var s in instrumentSources) s?.Pause();
    }

    public void UnpauseAll()
    {
        backingTrackSource?.UnPause();
        if (instrumentSources != null)
            foreach (var s in instrumentSources) s?.UnPause();
    }

    // Interacción desde el gameplay
    public void HitNote(int laneID)
    {
        if (laneID >= 0 && laneID < targetInstrumentVolumes.Length)
            targetInstrumentVolumes[laneID] = 1f;
    }

    public void MissNote(int laneID)
    {
        if (laneID >= 0 && laneID < targetInstrumentVolumes.Length)
            targetInstrumentVolumes[laneID] = 0f;
    }

    public void PlayMissSound()
    {
        if (sfxSource != null && missSoundClip != null)
            sfxSource.PlayOneShot(missSoundClip);
    }
}
