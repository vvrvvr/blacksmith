using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    private PlayerStats playerStats;

    private void Awake()
    {
        playerStats = PlayerStats.Singleton;
    }

    public void NextL()
    {
        playerStats.SetLevelToLoad(playerStats.LoadedLevel + 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
