using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManagerGame : MonoBehaviour
{
    // --- Singleton ---
    public static AudioManagerGame Instance { get; private set; }

    [System.Serializable]
    public class MusicEntry
    {
        public string key;      // Ej: "menu", "80s", "90s"
        public AudioClip clip;
    }

    [Header("Biblioteca de música")]
    [SerializeField] private List<MusicEntry> musicLibrary = new();

    [Header("Fade (segundos)")]
    [SerializeField] private float fadeSeconds = 1.2f;

    [SerializeField] private AudioMixerGroup musicMixerGroup;

    private Dictionary<string, AudioClip> _clips;
    private AudioSource _a, _b;
    private AudioSource _active, _idle;

    private void Awake()
    {
        // Singleton: solo uno en toda la ejecución
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Cargar clips en diccionario
        _clips = new Dictionary<string, AudioClip>();
        foreach (var m in musicLibrary)
        {
            if (!string.IsNullOrEmpty(m.key) && m.clip)
                _clips[m.key] = m.clip;
        }

        // Crear dos fuentes para hacer crossfade
        _a = gameObject.AddComponent<AudioSource>();
        _b = gameObject.AddComponent<AudioSource>();
        _a.loop = true;
        _b.loop = true;
        _a.playOnAwake = false;
        _b.playOnAwake = false;

        if (musicMixerGroup != null)
        {
            _a.outputAudioMixerGroup = musicMixerGroup;
            _b.outputAudioMixerGroup = musicMixerGroup;
        }
        _active = _a;
        _idle = _b;
    }

    // Reproduce inmediatamente un tema (sin fade)
    public void PlayInstant(string key, float volume = 1f)
    {
        if (!_clips.TryGetValue(key, out var clip) || clip == null)
            return;

        _active.clip = clip;
        _active.volume = volume;
        if (!_active.isPlaying) _active.Play();

        _idle.Stop();
        _idle.clip = null;
    }

    // Transición suave entre canciones
    public void FadeTo(string key, float targetVol = 1f)
    {
        if (!_clips.TryGetValue(key, out var next) || next == null)
            return;

        if (_active.clip == next && _active.isPlaying)
            return; // ya está sonando

        StopAllCoroutines();
        StartCoroutine(CoFade(next, targetVol));
    }

    private IEnumerator CoFade(AudioClip nextClip, float targetVol)
    {
        _idle.clip = nextClip;
        _idle.volume = 0f;
        _idle.Play();

        float t = 0f;
        float startVolActive = _active.volume;

        while (t < fadeSeconds)
        {
            t += Time.unscaledDeltaTime;
            float k = Mathf.Clamp01(t / fadeSeconds);
            _active.volume = Mathf.Lerp(startVolActive, 0f, k);
            _idle.volume = Mathf.Lerp(0f, targetVol, k);
            yield return null;
        }

        _active.Stop();
        var temp = _active;
        _active = _idle;
        _idle = temp;
        _idle.clip = null;
    }

    // Saber si una canción está sonando
    public bool IsPlaying(string key)
    {
        return _active != null && _active.clip != null &&
               _clips.TryGetValue(key, out var c) && _active.clip == c;
    }

    // En AudioManager
    public void StopInstant()
    {
        StopAllCoroutines();
        if (_active != null) { _active.Stop(); _active.clip = null; _active.volume = 0f; }
        if (_idle != null) { _idle.Stop(); _idle.clip = null; _idle.volume = 0f; }
    }

    public void FadeOutAndStop(float seconds = 0.5f)
    {
        StopAllCoroutines();
        StartCoroutine(CoFadeOutAndStop(seconds));
    }

    private System.Collections.IEnumerator CoFadeOutAndStop(float seconds)
    {
        float va = _active != null ? _active.volume : 0f;
        float vb = _idle != null ? _idle.volume : 0f;
        float t = 0f;
        while (t < seconds)
        {
            t += Time.unscaledDeltaTime;
            float k = Mathf.Clamp01(t / seconds);
            if (_active) _active.volume = Mathf.Lerp(va, 0f, k);
            if (_idle) _idle.volume = Mathf.Lerp(vb, 0f, k);
            yield return null;
        }
        StopInstant();
    }
}
