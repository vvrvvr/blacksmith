using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowRating : MonoBehaviour
{
    [SerializeField] private GameObject oneStar;
    [SerializeField] private GameObject twoStars;
    [SerializeField] private GameObject threeStars;
    private int rating;

    private void Awake()
    {
        rating = GameManager.Singleton.CalculateRating();
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
