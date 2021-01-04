using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private const string LEVEL_SAVE_KEY = "save_level";

    public static PlayerStats Singleton;
    private bool isSaveActive = false;

    #region Stats
    [HideInInspector] public string MenuToLoad;
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
        PlayerPrefs.SetInt(LEVEL_SAVE_KEY, Progress);
        PlayerPrefs.Save();
    }
}
