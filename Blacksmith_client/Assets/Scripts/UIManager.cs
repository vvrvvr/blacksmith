using UnityEngine.SceneManagement;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject LosePanel;

    public static UIManager Singleton;

    private void Awake()
    {
        if (Singleton != null)
            Destroy(Singleton);
        Singleton = this;
    }

    public void EnableLosePanel()
    {
        LosePanel.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
