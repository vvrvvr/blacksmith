using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int CubesAmount;
    [SerializeField] private int FramesAmount;
    [SerializeField] private int CubesCanMove;
    [SerializeField] private int FilledFrames;

    [Header("References")]
    [SerializeField] private Ingot ingot;
    [SerializeField] private WireframeBlank wireframeBlank;

    private void OnEnable()
    {
        Wireframe.OnStateChange += OnWireframeStateChange;
        Cube.OnDestroyEvent += OnCubeDestroy;
        Cube.OnStateChange += OnCubeStateChange;
    }

    private void OnDisable()
    {
        Wireframe.OnStateChange -= OnWireframeStateChange;
        Cube.OnDestroyEvent -= OnCubeDestroy;
        Cube.OnStateChange -= OnCubeStateChange;
    }

    private void Start()
    {
        CubesAmount = ingot.Cubes.Count;
        CubesCanMove = CubesAmount;
        FramesAmount = wireframeBlank.Wireframes.Count;
    }

    private void OnCubeDestroy()
    {
        CubesAmount--;
    }

    private void OnCubeStateChange(bool canMove)
    {
        CubesCanMove = canMove ? CubesCanMove + 1 : CubesCanMove - 1;
    }

    private void OnWireframeStateChange(Wireframe frame)
    {
        FilledFrames = frame.IsFilled ? FilledFrames + 1 : FilledFrames - 1;
    }

    private void CheckWinLoseConditions()
    {

    }
}
