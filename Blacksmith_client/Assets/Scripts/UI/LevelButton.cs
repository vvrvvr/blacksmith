using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Button))]
public class LevelButton : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Level index from LevelManager's Level Prefabs")]
    public int LevelIndex = 0;
    [SerializeField] private bool isFirstLevelInLine;
    [SerializeField] private int starsAmountToOpen;

    [Header("References")]
    [SerializeField] private GameObject textObject;
    [SerializeField] private GameObject closedImage;
    [SerializeField] private Button button;
    [SerializeField] private GameObject oneStar;
    [SerializeField] private GameObject twoStars;
    [SerializeField] private GameObject threeStars;

    private PlayerStats playerStats;
    private SaveManager saveManager;

    public void LoadScene(string name) => SceneManager.LoadScene(name);

    private void Start()
    {
        playerStats = PlayerStats.Singleton;
        saveManager = SaveManager.Singleton;
        SetLevelStats();
    }

    public void SetLevelToLoad()
    {
        playerStats.SetLevelToLoad(LevelIndex);
        playerStats.MenuToLoad = "";
    }

    private void SetLevelStats()
    {
        if (isFirstLevelInLine && starsAmountToOpen <= saveManager.StarsTotal)
        {
            button.interactable = true;
            closedImage.SetActive(false);
            if (saveManager.levelStats.Count > 0)
            {
                SetStars(saveManager.levelStats[LevelIndex]);
            }
            return;
        }
        if (saveManager.levelStats.Count > 0)
        {
            if (starsAmountToOpen <= saveManager.StarsTotal && LevelIndex > 0 && saveManager.levelStats[LevelIndex - 1] > 0)
            {
                button.interactable = true;
                closedImage.SetActive(false);
                SetStars(saveManager.levelStats[LevelIndex]);
            }
        }
    }

    private void SetStars(int i)
    {
        switch (i)
        {
            case 1:
                oneStar.SetActive(true);
                break;
            case 2:
                twoStars.SetActive(true);
                break;
            case 3:
                threeStars.SetActive(true);
                break;
            default:
                break;
        }
    }
}

