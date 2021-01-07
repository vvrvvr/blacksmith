using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(Button))]
public class LevelButton : MonoBehaviour
{
    [Header("Settings")]
    public int StageForOpen = 0;

    [Header("References")]
    [SerializeField] private GameObject textObject;
    [SerializeField] private GameObject closedImage;
    [SerializeField] private Button button;
    private PlayerStats playerStats;

    public void LoadScene(string name) => SceneManager.LoadScene(name);

    private void Start()
    {
        playerStats = PlayerStats.Singleton;
        if (playerStats.Progress >= StageForOpen)
        {
            button.interactable = true;
            closedImage.SetActive(false);
        }
    }

    public void SetLevelToLoad(int levelIndex)
    {
        playerStats.SetLevelToLoad(levelIndex);
        playerStats.MenuToLoad = "";
    }
}
