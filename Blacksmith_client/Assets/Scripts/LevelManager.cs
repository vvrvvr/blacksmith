using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [HideInInspector] public Ingot currentIngot;
    [HideInInspector] public WireframeBlank currentWireframe;
    [HideInInspector] public Level currentLevel;

    [SerializeField] private Transform colliderPlane;
    [SerializeField] private List<Level> levelPrefabs;

    private void Start()
    {
        LoadLevel(Mathf.Min(PlayerStats.Singleton.LevelToLoad, levelPrefabs.Count - 1));
    }

    public void LoadLevel(int index)
    {
        currentLevel = Instantiate(levelPrefabs[index]);
        currentLevel.IngotDrag.Init(colliderPlane);
        currentIngot = currentLevel.Ingot;
        currentWireframe = currentLevel.WireframeBlank;

        GameManager.Singleton.SetLevelStats(currentLevel);
    }
}
