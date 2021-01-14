using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public Ingot Ingot;
    public IngotDragAndDrop IngotDrag;
    public WireframeBlank WireframeBlank;

    [Header("Rating stats")]
    public int ThreeStarsCubeAmount;
    public int TwoStarsCubeAmount;
}
