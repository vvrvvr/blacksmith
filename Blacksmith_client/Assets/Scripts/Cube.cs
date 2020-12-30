using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] private Material red;
    [SerializeField] private Material green;
    private MeshRenderer myRend;

    void Start()
    {
        myRend = GetComponent<MeshRenderer>();
    }

    /// <summary>
    /// change cube color when selected
    /// </summary>
    public void TurnGreen()
    {
        myRend.material = green;
    }

    /// <summary>
    /// return default cube color
    /// </summary>
    public void TurnRed()
    {
        myRend.material = red;
    }
}
