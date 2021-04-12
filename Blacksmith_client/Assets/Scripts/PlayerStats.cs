using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Singleton;
    private bool isSaveActive = false;

    #region Stats
    [HideInInspector] public string MenuToLoad;
    [HideInInspector] public int LoadedLevel;
   // [HideInInspector] 
    public int LevelToLoad;
    [HideInInspector] public List<int> levelStats = new List<int>();
    [HideInInspector] public int StarsTotal;
    #endregion

    private void Awake()
    {
        if (Singleton != null)
        { 
            Destroy(gameObject);
            return;
        }
        isSaveActive = true;
        Singleton = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        if (!isSaveActive)
            return;
    }

    public void SetLevelToLoad(int levelIndex)
    {
        LevelToLoad = levelIndex;
        LoadedLevel = levelIndex;
    }

    public void RecalculateStars()
    {
        StarsTotal = 0;
        foreach (int stars in levelStats)
        {
            StarsTotal += stars;
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
