using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Sources")]
    //nueva jugabilidad
    public AudioSource instrumentalSource; //Instrumental general
    public AudioSource backingTrackSource; //voces

    [Header("SFX")]
    public AudioSource sfxSource;
    public AudioClip missSoundClip;
    //arranque sincronizado
    private bool started = false;
    private double startDspTime;
    double pausedDspTime;
    double pauseSAcumulado;
    bool isPause;



    private void Awake()
    {
        if (instance == null) instance = this;
        else { Destroy(gameObject); return; }
    }

    private void Update()
    {
      
    }

    //  Deja de reproducir aquí; solo asigna clips.
    public void SetupSong(SongChart song)
    {
        if (song == null) return;
        if (backingTrackSource) backingTrackSource.clip = song.backingTrackClip;
        if (instrumentalSource) instrumentalSource.clip = song.instrumentalClip; // [NUEVO]
        if (backingTrackSource) backingTrackSource.loop = false;
        if (instrumentalSource) instrumentalSource.loop = false;
        started = false;
    }

    // 🔸 Arrancar TODO sincronizado en una misma marca de tiempo DSP
    public void StartAllSynced(double leadTime = 0.1)
    {
        if (started) return;
        pauseSAcumulado = 0;
        startDspTime = AudioSettings.dspTime + leadTime;

        if (backingTrackSource && backingTrackSource.clip)
            backingTrackSource.PlayScheduled(startDspTime);

        if (instrumentalSource && instrumentalSource.clip)
            instrumentalSource.PlayScheduled(startDspTime);

        started = true;
    }


    public double GetSongDuration()
    {
        double a = (backingTrackSource && backingTrackSource.clip) ? backingTrackSource.clip.length : 0.0;
        double b = (instrumentalSource && instrumentalSource.clip) ? instrumentalSource.clip.length : 0.0;
        return Mathf.Max((float)a, (float)b); // duración “oficial” = la más larga de las dos
    }

    public bool IsSongEnded(double margin = 0.10) // 100 ms de colchón contra redondeos
    {
        if (!started) return false;
        return GetSongDspTime() >= GetSongDuration() - margin;
    }


    //  Detener TODO (para reiniciar limpio)
    public void StopAllAudio()
    {
        backingTrackSource?.Stop();
        instrumentalSource?.Stop();
        started = false;
    }

    //  Pausar / Reanudar (para GameOver o Pausa)
    public void PauseAll()
    {
        pausedDspTime = AudioSettings.dspTime;
        backingTrackSource?.Pause();
        instrumentalSource?.Pause();
    }

    public void UnpauseAll()
    {
        pauseSAcumulado += AudioSettings.dspTime - pausedDspTime;
        backingTrackSource?.UnPause();
        instrumentalSource?.UnPause();
    }

    public void HitNote(int laneID)
    {
        // Ya no se maneja volumen por lane.
        // Aquí puedes poner un efecto o SFX de acierto.
        // Ejemplo opcional:
        // sfxSource?.PlayOneShot(hitSoundClip);
    }

    public void MissNote(int laneID)
    {
        // Antes bajaba el volumen del instrumento del lane.
        // Ahora podrías aplicar un filtro temporal, o simplemente:
        PlayMissSound(); // mantiene el feedback de error
    }

    public void PlayMissSound()
    {
        if (sfxSource != null && missSoundClip != null)
            sfxSource.PlayOneShot(missSoundClip);
    }

    public float GetSongDspTime()
    {
        if (!started) return 0f;
        double now = AudioSettings.dspTime;
        return (float)((now - startDspTime) - pauseSAcumulado);
    }

}
