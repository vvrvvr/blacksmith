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
    //public int StageForOpen = 0;
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

        //if (playerStats.Progress >= LevelIndexForOpen)
        //{
        //    button.interactable = true;
        //    closedImage.SetActive(false);
        //}
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
            switch (saveManager.levelStats[LevelIndex])
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
            return;
        }
        if (starsAmountToOpen <= saveManager.StarsTotal && saveManager.levelStats[LevelIndex - 1] > 0)
        {
            button.interactable = true;
            closedImage.SetActive(false);
            switch (saveManager.levelStats[LevelIndex])
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
}

