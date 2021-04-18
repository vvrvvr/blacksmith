using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayCurrentStarsAmount : MonoBehaviour
{
    [SerializeField] private Text text1;
    private void Start()
    {
        int starsTotal = 0;
        if (PlayerStats.Singleton != null)
            starsTotal = PlayerStats.Singleton.StarsTotal;
        text1.text = $"Stars: {starsTotal}";
    }
}
