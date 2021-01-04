using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private const string LEVEL_SAVE_KEY = "save_level";

    public static PlayerStats Singleton;
    private bool isSaveActive = false;

    #region Stats
    public int SavedLevel;
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
        SavedLevel = PlayerPrefs.GetInt(LEVEL_SAVE_KEY, 0);
    }

    private void OnDestroy()
    {
        if (!isSaveActive)
            return;
        PlayerPrefs.SetInt(LEVEL_SAVE_KEY, SavedLevel);
        PlayerPrefs.Save();
    }
}
