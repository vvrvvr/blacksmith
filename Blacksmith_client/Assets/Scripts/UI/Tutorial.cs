using UnityEngine.UI;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private Text _text;

    private void OnEnable() 
    {
        LevelManager.OnLevelLoad += OnLevelLoad;
    }

    private void OnDisable()
    {
        LevelManager.OnLevelLoad -= OnLevelLoad;
    }

    private void OnLevelLoad(Level level)
    {
        if(string.IsNullOrEmpty(level.TutorialText))
        {
            _panel.SetActive(false);
        }
        else
        {
            _text.text = level.TutorialText;
            _panel.SetActive(true);
        }
    }
}
