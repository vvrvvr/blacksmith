using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowRating : MonoBehaviour
{
    [SerializeField] private GameObject oneStar;
    [SerializeField] private GameObject twoStars;
    [SerializeField] private GameObject threeStars;
    private int rating;
    private int previousRating;
    private PlayerStats playerStats = PlayerStats.Singleton;
    private SaveManager saveManager = SaveManager.Singleton;

    private void Awake()
    {
        previousRating = playerStats.levelStats[playerStats.LoadedLevel];
        rating = GameManager.Instance.CalculateRating();
        if(rating > previousRating)
        {
            playerStats.levelStats[playerStats.LoadedLevel] = rating;
            playerStats.RecalculateStars();
            saveManager.SaveGame();
        }
        switch (rating)
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
