﻿using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [HideInInspector] public Ingot currentIngot;
    [HideInInspector] public WireframeBlank currentWireframe;
    [HideInInspector] public Level currentLevel;

    [SerializeField] private Transform colliderPlane;
    [SerializeField] private List<Level> levelPrefabs;

    public static System.Action<Level> OnLevelLoad;

    private void OnEnable() => GameManager.OnVictoryEvent += OnVictoryEvent;
    private void OnDisable() => GameManager.OnVictoryEvent -= OnVictoryEvent;

    private void Start()
    {
        LoadLevel(Mathf.Min(PlayerStats.Singleton.LevelToLoad, levelPrefabs.Count - 1));
        PlayerStats.Singleton.RefreshLevelStats(levelPrefabs);
    }

    public void LoadLevel(int index)
    {
        currentLevel = Instantiate(levelPrefabs[Mathf.Min(index, levelPrefabs.Count - 1)]);
        currentLevel.IngotDrag.Init(colliderPlane);
        currentIngot = currentLevel.Ingot;
        currentWireframe = currentLevel.WireframeBlank;

        PlayerStats.Singleton.LoadedLevel = Mathf.Min(index, levelPrefabs.Count - 1);
        GameManager.Instance.SetLevelStats(currentLevel);
        OnLevelLoad?.Invoke(currentLevel);
    }

    private void OnVictoryEvent()
    {
        if(currentLevel.finalProduct != null)
        {
	        currentLevel.finalProduct.Activate();
	        if(currentLevel.ProductHandle != null)
	        	currentLevel.ProductHandle.SetActive(false);
            currentLevel.Ingot.gameObject.SetActive(false);
            currentLevel.WireframeBlank.gameObject.SetActive(false);
        }
    }
}
