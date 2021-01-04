using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private const string LEVEL_SAVE_KEY = "save_level";

    public static PlayerStats Singleton;
    private bool isSaveActive = false;

    #region Stats
    [HideInInspector] public string MenuToLoad;
    [HideInInspector] public int LoadedLevel;
    public int LevelToLoad;
    public int Progress;
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
        Progress = PlayerPrefs.GetInt(LEVEL_SAVE_KEY, 0);
    }

    private void OnDestroy()
    {
        if (!isSaveActive)
            return;
        SaveProgress(Progress);
    }

    public void SaveProgress(int progress)
    {
        Progress = progress;
        PlayerPrefs.SetInt(LEVEL_SAVE_KEY, progress);
        PlayerPrefs.Save();
    }

    public void SaveLevelProgress(int modifier = 0)
    {
        Progress = LoadedLevel + modifier;
        PlayerPrefs.SetInt(LEVEL_SAVE_KEY, Progress);
        PlayerPrefs.Save();
    }

    public void SetLevelToLoad(int levelIndex)
    {
        LevelToLoad = levelIndex;
        LoadedLevel = levelIndex;
    }
}
