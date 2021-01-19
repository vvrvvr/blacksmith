using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Singleton;
    private void Awake() => Singleton = this;

    private GameObject activePanel = null;

    public void ExitGame() => Application.Quit();
    public void LoadScene(string name) => SceneManager.LoadScene(name);
    public void RestartScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    private void Start()
    {
        if (PlayerStats.Singleton.MenuToLoad != "")
            EnablePanel(GameObject.Find(PlayerStats.Singleton.MenuToLoad));
    }

    public void SetMenuToLoad(string menuName)
    {
        PlayerStats.Singleton.MenuToLoad = menuName;
    }

    public void EnablePanel(GameObject panel)
    {
        if (panel != null)
        {
            activePanel?.SetActive(false);
            panel.SetActive(true);
            activePanel = panel;
        }
    }
}
