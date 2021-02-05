using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
   

    public static SaveManager Singleton;
    private PlayerStats playerStats;

    private void Awake()
    {
        if (Singleton != null)
        {
            Destroy(gameObject);
            return;
        }
        Singleton = this;
        playerStats = PlayerStats.Singleton;
        DontDestroyOnLoad(gameObject);
        LoadGame();
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
            StarsTotal = playerStats.StarsTotal,
            levelStats = playerStats.levelStats,
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
            playerStats.levelStats = saveObject.levelStats;
            playerStats.StarsTotal = saveObject.StarsTotal;
        }
    }
}


