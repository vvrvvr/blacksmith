using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [Header("Settings")]
    public int TimerSec;

    [Header("Rating stats")]
    public int ThreeStarsCubeAmount;
    public int TwoStarsCubeAmount;

    [Header("References")]
    public Ingot Ingot;
    public IngotDragAndDrop IngotDrag;
    public WireframeBlank WireframeBlank;

    [Header("Final Product")]
    public FinalProduct finalProduct;
}
