using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [SerializeField] private List<Level> levelPrefabs;
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
    }

    private void Start()
    {
        foreach(Level l in levelPrefabs)
        {
            levelStats.Add(0);
        }
        //levelStats[0] = 2;
        //levelStats[1] = 1;
        //levelStats[2] = 3;
        //levelStats[3] = 1;
        //levelStats[5] = 2;
        StarsTotal = 0;
        foreach(int stars in levelStats)
        {
            StarsTotal += stars;
        }
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
        public List<int> levelStats;
        public int StarsTotal;
    }
}


