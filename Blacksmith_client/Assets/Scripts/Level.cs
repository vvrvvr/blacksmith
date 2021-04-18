using UnityEngine;

public class Level : MonoBehaviour
{
    [Header("Settings")]
    public int TimerSec;
    public Transform RotationCenter = null;
    [TextArea(2, 15)] public string TutorialText;

    [Header("Rating stats")]
    public int ThreeStarsCubeAmount;
    public int TwoStarsCubeAmount;

    [Header("References")]
    public Ingot Ingot;
    public IngotDragAndDrop IngotDrag;
    public WireframeBlank WireframeBlank;

    [Header("Final Product")]
	public FinalProduct finalProduct;
	public GameObject ProductHandle;
}
