using UnityEngine;
using UnityEditor;

public class LevelLoader : MonoBehaviour
{
    [MenuItem("GD/Load Last Level")]
    static void LoadLastLevel()
    {
        if (!Application.isPlaying)
            return;
        //LevelManager lm = FindObjectOfType<LevelManager>();
        //lm.LoadLevel(999);
    }
}
