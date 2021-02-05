using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Button))]
public class LevelButton : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject textObject;
    [SerializeField] private GameObject closedImage;
    [SerializeField] private Button button;
    [SerializeField] private GameObject oneStar;
    [SerializeField] private GameObject twoStars;
    [SerializeField] private GameObject threeStars;

    [Header("Settings")]
    [Tooltip("Level index from LevelManager's Level Prefabs")]
    public int LevelIndex = 0;
    [HideInInspector]
    public bool isFirstLevelInLine;
    [HideInInspector]
    public int starsAmountToOpen;
    [HideInInspector]
    public bool isEpic;
    [HideInInspector]
    public int indexOfFirstLevelInLine;
    [HideInInspector]
    public int indexOfLastLevelInLine;


    private PlayerStats playerStats;

    public void LoadScene(string name) => SceneManager.LoadScene(name);

    private void Start()
    {
        playerStats = PlayerStats.Singleton;
        SetLevelStats();
    }

    public void SetLevelToLoad()
    {
        playerStats.SetLevelToLoad(LevelIndex);
        playerStats.MenuToLoad = "";
    }

    private void SetLevelStats()
    {
        if (isEpic)
        {
            if (playerStats.levelStats.Count >= indexOfLastLevelInLine + 1)
            {
                bool condition = true;
                for (int i = indexOfFirstLevelInLine; i <= indexOfLastLevelInLine; i++)
                {
                    if (playerStats.levelStats[i] != 3)
                        condition = false;
                }
                if (condition)
                {
                    button.interactable = true;
                    closedImage.SetActive(false);
                    SetStars(playerStats.levelStats[LevelIndex]);
                }
            }
        }
        else
        {
            if (isFirstLevelInLine && starsAmountToOpen <= playerStats.StarsTotal)
            {
                button.interactable = true;
                closedImage.SetActive(false);
                if (playerStats.levelStats.Count > 0)
                {
                    SetStars(playerStats.levelStats[LevelIndex]);
                }
                return;
            }
            if (playerStats.levelStats.Count > 0)
            {
                if (starsAmountToOpen <= playerStats.StarsTotal && LevelIndex > 0 && playerStats.levelStats[LevelIndex - 1] > 0)
                {
                    button.interactable = true;
                    closedImage.SetActive(false);
                    SetStars(playerStats.levelStats[LevelIndex]);
                }
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

#if UNITY_EDITOR
    [CustomEditor(typeof(LevelButton))]
    public class RandomScript_Editor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            LevelButton script = (LevelButton)target;
            if (!script.isEpic)
                script.isFirstLevelInLine = EditorGUILayout.Toggle("Is first level in line", script.isFirstLevelInLine);
            if (script.isFirstLevelInLine)
            {
                script.starsAmountToOpen = EditorGUILayout.IntField("Stars amount to open", script.starsAmountToOpen);   
            }
            if (!script.isFirstLevelInLine)
                script.isEpic = EditorGUILayout.Toggle("Is epic", script.isEpic);
            if (script.isEpic)
            {
                script.indexOfFirstLevelInLine = EditorGUILayout.IntField("Index of first level in line", script.indexOfFirstLevelInLine);
                script.indexOfLastLevelInLine = EditorGUILayout.IntField("Index of last level in line", script.indexOfLastLevelInLine);
            } 
        }
    }
#endif
}




