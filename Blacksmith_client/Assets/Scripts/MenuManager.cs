using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> panels;

    public static MenuManager Singleton;
    private void Awake() => Singleton = this;

    public void ExitGame() => Application.Quit();
    public void LoadScene(string name) => SceneManager.LoadScene(name);
    public void RestartScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    private void Start()
    {
        if (PlayerStats.Singleton.MenuToLoad != "")
            EnablePanel(PlayerStats.Singleton.MenuToLoad);
    }

    public void SetMenuToLoad(string menuName)
    {
        PlayerStats.Singleton.MenuToLoad = menuName;
    }

    public void EnablePanel(GameObject panel)
    {
        DisableAllPanel();
        panel.SetActive(true);
    }

    public void EnablePanel(string name)
    {
        DisableAllPanel();
        foreach(var p in panels)
        {
            if(p.name == name)
            {
                p.SetActive(true);
                return;
            }
        }
        Debug.LogError($"Panel [{name}] not registered!");
    }

    private void DisableAllPanel()
    {
        foreach (var p in panels)
            p.SetActive(false);
    }
}
