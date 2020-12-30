using System.Collections.Generic;
using UnityEngine;

public class WireframeBlank : MonoBehaviour
{
    [SerializeField] private List<Transform> wireframes;
    [SerializeField] public int FilledFrames = 0;
    [SerializeField] public int EmptyFrames = 0;

    private void OnEnable() => Wireframe.OnStateChange += WireframeStateChange;
    private void OnDisable() => Wireframe.OnStateChange -= WireframeStateChange;
    
    private void Start()
    {
        // All frames is empty by default
        EmptyFrames = wireframes.Count;
    }

    private void WireframeStateChange(Wireframe frame)
    {
        FilledFrames = frame.IsFilled ? FilledFrames + 1 : FilledFrames - 1;
        EmptyFrames = frame.IsFilled ? EmptyFrames - 1 : EmptyFrames + 1;
    }

    /* autoload wireframes on Start()
    private void Start()
    {
        foreach (Transform frame in transform)
        {
            wireframes.Add(frame);
        }
    }*/
}
