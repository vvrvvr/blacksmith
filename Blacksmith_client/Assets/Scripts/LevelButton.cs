using UnityEngine;

public class LevelButton : MonoBehaviour
{
    [Header("Settings")]
    public int StageForOpen = 0;

    [Header("References")]
    [SerializeField] private GameObject textObject;
    [SerializeField] private GameObject closedImage;

    private void Start()
    {
        if(PlayerStats.Singleton.SavedLevel >= StageForOpen)
        {
            //textObject.SetActive(true);
            closedImage.SetActive(false);
        }
        else
        {
            //textObject.SetActive(false);
            closedImage.SetActive(true);
        }
    }
}
