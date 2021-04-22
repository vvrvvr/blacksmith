using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject initialPanel;
    [SerializeField] private Transform panelsContainer;

    public static MenuManager Singleton;
    private void Awake() => Singleton = this;

    private GameObject activePanel = null;

    public void ExitGame() => Application.Quit();
    public void LoadScene(string name) => SceneManager.LoadScene(name);
    public void RestartScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    private void Start()
    {
        foreach(Transform panel in panelsContainer)
        {
            panel.gameObject.SetActive(false);
        }

        if (PlayerStats.Singleton.MenuToLoad != "")
            EnablePanel(panelsContainer.Find(PlayerStats.Singleton.MenuToLoad).gameObject);
        else
            EnablePanel(initialPanel);
    }

    public void SetMenuToLoad(string menuName)
    {
        PlayerStats.Singleton.MenuToLoad = menuName;
    }

    public void EnablePanel(GameObject panel)
    {
        if (panel != null)
        {
            if(activePanel != null)
                activePanel.SetActive(false);
            panel.SetActive(true);
            activePanel = panel;
        }
    }

    public void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
    }

    public void SwitchActivity(GameObject go)
    {
        go.SetActive(!go.activeSelf);
    }

    public void EnablePanelWithDelay(GameObject panel, float delay) => StartCoroutine(EnablePanelCoroutine(panel, delay));
    private IEnumerator EnablePanelCoroutine(GameObject panel, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        EnablePanel(panel);
    }
}
