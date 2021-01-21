using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayCurrentStarsAmount : MonoBehaviour
{
    [SerializeField] private Text text1;
    private void Awake()
    {
        text1.text = $"Stars: {SaveManager.Singleton.StarsTotal}";
    }
}
