using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [HideInInspector] public List<int> levelStats = new List<int>();
    [HideInInspector] public int StarsTotal;

    public static SaveManager Singleton;

    private void Awake()
    {
        if (Singleton != null)
        {
            Destroy(gameObject);
            return;
        }
        Singleton = this;
        DontDestroyOnLoad(gameObject);
        LoadGame();
    }

    public void RecalculateStars()
    {
        StarsTotal = 0;
        foreach (int stars in levelStats)
        {
            StarsTotal += stars;
        }
    }

    private class SaveObject
    {
        public int StarsTotal;
        public List<int> levelStats;
    }

    public void SaveGame()
    {
        SaveObject saveObject = new SaveObject
        {
            StarsTotal = StarsTotal,
            levelStats = levelStats,
        };
        string json = JsonUtility.ToJson(saveObject);
        File.WriteAllText(Application.dataPath + "/save.txt", json);
    }

    public void LoadGame()
    {
        if (File.Exists(Application.dataPath + "/save.txt"))
        {
            string json = File.ReadAllText(Application.dataPath + "/save.txt");
            SaveObject saveObject = JsonUtility.FromJson<SaveObject>(json);
            levelStats = saveObject.levelStats;
            StarsTotal = saveObject.StarsTotal;
        }
    }

    public void RefreshLevelStats(List<Level> levelPrefabs)
    {
        if (levelPrefabs.Count > levelStats.Count)
        {
            while (levelStats.Count < levelPrefabs.Count)
            {
                levelStats.Add(0);
            }
        }
    }
}


