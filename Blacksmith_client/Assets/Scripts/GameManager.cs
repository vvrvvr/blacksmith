using System.Collections;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField, ReadOnly] private int CubesAmount;
    [ReadOnly] public int FramesAmount;
    [SerializeField, ReadOnly] private int CubesCanMove;
    [ReadOnly] public int FilledFrames;
    [SerializeField, ReadOnly] private int CubesRated;
    [SerializeField] private float VictoryDelay = 2f;
    [SerializeField] private AudioClip _backgroundMusic;

    [Header("References")]
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private AudioClip _loseMusic;
    [SerializeField] private AudioClip _victoryMusic1;
    [SerializeField] private AudioClip _victoryMusic2;
    [SerializeField] private AudioClip _victoryMusic3;

    public static Action OnVictoryEvent;
    public static Action OnLoseEvent;

    private int threeStarsRating;
    private int twoStarsRating;
    private bool NeedCheckConditions = false;
    public bool CanChooseCube = true;
    private AudioSource audioS;
    public bool isAllFramesFilled { get; private set; } = false;

    public static GameManager Instance { get; private set; }

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
        Cube.OnDestroyEventGlobal += OnCubeDestroy;
        Cube.OnStateChange += OnCubeStateChange;
    }

    private void OnDisable()
    {
        Wireframe.OnStateChange -= OnWireframeStateChange;
        Cube.OnDestroyEventGlobal -= OnCubeDestroy;
        Cube.OnStateChange -= OnCubeStateChange;
    }

    private void Awake()
    {
        Instance = this;
        audioS = GetComponent<AudioSource>();
    }

    private void Start()
    {
        MusicManager.Instance.PlayMusic(_backgroundMusic);
    }

    private IEnumerator LateCheckConditions()
    {
        yield return new WaitForFixedUpdate();
        NeedCheckConditions = false;
        if (FilledFrames == FramesAmount)
            isAllFramesFilled = true;
        else
            isAllFramesFilled = false;
        if (FilledFrames == FramesAmount && CubesAmount == FramesAmount)
        {
            Victory();
        }
        else if (CubesCanMove == 0 && FilledFrames != FramesAmount || CubesAmount < FramesAmount)
        {
            Defeat();
        }
    }

    public void Victory()
    {
        OnVictoryEvent?.Invoke();
        if(levelManager.currentLevel.finalProduct != null)
        {
            MenuManager.Singleton.EnablePanelWithDelay(victoryPanel, VictoryDelay);
        }
        else
        {
            MenuManager.Singleton.EnablePanel(victoryPanel);
        }

        int stars = CalculateRating();
        switch(stars)
        {
            case 1:
                MusicManager.Instance.PlaySound(_victoryMusic1);
                break;
            case 2:
                MusicManager.Instance.PlaySound(_victoryMusic2);
                break;
            case 3:
                MusicManager.Instance.PlaySound(_victoryMusic3);
                break;
            default:
                break;
        }
    }

    public void Defeat()
    {
        OnLoseEvent?.Invoke();
        MenuManager.Singleton.EnablePanel(losePanel);
        MusicManager.Instance.PlaySound(_loseMusic);
    }

    private void OnCubeDestroy(bool isRated)
    {
        if (audioS.isPlaying)
        {
            audioS.pitch = UnityEngine.Random.Range(0.7f, 1.2f);
            audioS.Stop();
            audioS.Play();
        }
        else
        {
            audioS.pitch = UnityEngine.Random.Range(0.7f, 1.2f);
            audioS.Play();
        }
        CubesAmount--;
        if (isRated)
            CubesRated++;
        CheckWinLoseConditions();
    }

    public void AddCubeRated(int count = 1)
    {
        CubesRated += count;
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
        if (CubesRated <= threeStarsRating)
            return 3;
        if (CubesRated > threeStarsRating && CubesRated <= twoStarsRating)
            return 2;
        return 1;
    }

    //for testing. delete if you see it
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            Victory();
        }
    }
}