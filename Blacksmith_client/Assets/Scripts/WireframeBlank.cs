using System.Collections.Generic;
using UnityEngine;

public class WireframeBlank : MonoBehaviour
{
    public List<Transform> Wireframes { get; private set; } = new List<Transform>();

    private void Awake()
    {
        InitWireframes();
    }

    private void InitWireframes()
    {
        foreach (Transform frame in transform)
        {
            Wireframes.Add(frame);
        }
    }
}
