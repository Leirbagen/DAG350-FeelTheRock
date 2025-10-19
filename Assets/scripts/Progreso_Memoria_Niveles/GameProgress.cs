using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelRecord
{
    public int bestScore;
    public int bestStars;
    public bool completed;
}

[Serializable]
class SaveBlob
{
    public List<string> keys = new();
    public List<LevelRecord> values = new();
}

public class GameProgress : MonoBehaviour
{
    public static GameProgress Instance { get; private set; }
    const string PP_KEY = "GAME_PROGRESS_V1";

    // Mapa levelId -> record
    private Dictionary<string, LevelRecord> records = new();

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Load();
    }

    // --- API pública ---


    public bool IsCompleted(string levelId)
    {
        return Get(levelId).completed;
    }

    public void MarkCompleted(string levelId)
    {
        if (string.IsNullOrEmpty(levelId)) return;
        var rec = Get(levelId);
        if (!rec.completed)
        {
            rec.completed = true;
            Save(); // << guarda aunque no haya mejora de score/estrellas
        }
    }



    public LevelRecord Get(string levelId)
    {
        if (string.IsNullOrEmpty(levelId)) return new LevelRecord();
        if (!records.TryGetValue(levelId, out var rec))
        {
            rec = new LevelRecord { bestScore = 0, bestStars = 0 };
            records[levelId] = rec;
        }
        return rec;
    }

    // Actualiza solo si mejora algo. Devuelve true si hubo mejora.
    public bool ReportResult(string levelId, int score, int stars, bool markCompleted = true)
    {
        if (string.IsNullOrEmpty(levelId)) return false;
        var rec = Get(levelId);
        bool improved = false;

        if (score > rec.bestScore) { rec.bestScore = score; improved = true; }
        if (stars > rec.bestStars) { rec.bestStars = stars; improved = true; }

        if (markCompleted && !rec.completed)
        {
            rec.completed = true;
            improved = true; // para forzar Save()
        }

        if (improved) Save();
        return improved;
    }

    public int GetBestScore(string levelId) => Get(levelId).bestScore;
    public int GetBestStars(string levelId) => Get(levelId).bestStars;

    // --- Persistencia ---
    void Save()
    {
        var blob = new SaveBlob();
        foreach (var kv in records)
        {
            blob.keys.Add(kv.Key);
            blob.values.Add(kv.Value);
        }
        string json = JsonUtility.ToJson(blob);
        PlayerPrefs.SetString(PP_KEY, json);
        PlayerPrefs.Save();
    }

    void Load()
    {
        records.Clear();
        string json = PlayerPrefs.GetString(PP_KEY, "");
        if (string.IsNullOrEmpty(json)) return;

        var blob = JsonUtility.FromJson<SaveBlob>(json);
        if (blob?.keys == null || blob.values == null) return;

        for (int i = 0; i < blob.keys.Count && i < blob.values.Count; i++)
        {
            records[blob.keys[i]] = blob.values[i];
        }
    }

    // Si algún día necesitas borrar progreso:
    public void ResetAllProgress()
    {
        print("datos borrados y guardados");
        records.Clear();
        PlayerPrefs.DeleteKey(PP_KEY);
        PlayerPrefs.Save();
    }

    internal void ReportResult(string levelID, int currentScore, int stars, object value)
    {
        throw new NotImplementedException();
    }
}

