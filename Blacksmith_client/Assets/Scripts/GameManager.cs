using System.Collections;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int CubesAmount;
    [SerializeField] private int FramesAmount;
    [SerializeField] private int CubesCanMove;
    [SerializeField] private int FilledFrames;
    [SerializeField] private int CubesRated;

    private int threeStarsRating;
    private int twoStarsRating;
    private bool NeedCheckConditions = false;

    public static GameManager Singleton { get; private set; }

    public void SetLevelStats(Level level)
    {
        CubesAmount = level.Ingot.Cubes.Count;
        threeStarsRating = level.ThreeStarsCubeAmount;
        twoStarsRating = level.TwoStarsCubeAmount;
        CubesCanMove = CubesAmount;
        FramesAmount = level.WireframeBlank.Wireframes.Count;
    }

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

    private void Awake()
    {
        Singleton = this;
    }

    private IEnumerator LateCheckConditions()
    {
        yield return new WaitForFixedUpdate();
        NeedCheckConditions = false;
        if (FilledFrames == FramesAmount && CubesAmount == FramesAmount)
        {
            // Victory
            PlayerStats.Singleton.SaveLevelProgress(1);
            MenuManager.Singleton.EnablePanel("Victory Panel");
        }
        else if (CubesCanMove == 0 && FilledFrames != FramesAmount || CubesAmount < FramesAmount)
        {
            // Lose
            MenuManager.Singleton.EnablePanel("Lose Panel");
        }
    }

    private void OnCubeDestroy(bool isRated)
    {
        CubesAmount--;
        if (isRated)
            CubesRated++;
        CheckWinLoseConditions();
    }

    private void OnCubeStateChange(bool canMove)
    {
        CubesCanMove = canMove ? CubesCanMove + 1 : CubesCanMove - 1;
        CheckWinLoseConditions();
    }

    private void OnWireframeStateChange(Wireframe frame)
    {
        FilledFrames = frame.IsFilled ? FilledFrames + 1 : FilledFrames - 1;
        CheckWinLoseConditions();
    }

    private void CheckWinLoseConditions()
    {
        if (NeedCheckConditions)
            return;
        NeedCheckConditions = true;
        StartCoroutine(LateCheckConditions());
    }

    public int CalculateRating()
    {
        int rating;
        if (CubesRated <= threeStarsRating)
            rating = 3;
        else if (CubesRated > threeStarsRating && CubesRated <= twoStarsRating)
            rating = 2;
        else
            rating = 1;
        return rating;
    }

}
