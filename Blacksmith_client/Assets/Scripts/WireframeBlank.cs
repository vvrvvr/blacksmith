using System.Collections.Generic;
using UnityEngine;

public class WireframeBlank : MonoBehaviour
{
    [SerializeField] public int FilledFrames = 0;
    [SerializeField] public int EmptyFrames = 0;
    private List<Transform> wireframes = new List<Transform>();

    private void OnEnable() => Wireframe.OnStateChange += OnWireframeStateChange;
    private void OnDisable() => Wireframe.OnStateChange -= OnWireframeStateChange;
    
    private void Start()
    {
        InitWireframes();
        EmptyFrames = wireframes.Count;
    }

    private void OnWireframeStateChange(Wireframe frame)
    {
        FilledFrames = frame.IsFilled ? FilledFrames + 1 : FilledFrames - 1;
        EmptyFrames = frame.IsFilled ? EmptyFrames - 1 : EmptyFrames + 1;
    }

    private void InitWireframes()
    {
        foreach (Transform frame in transform)
        {
            wireframes.Add(frame);
        }
    }
}
