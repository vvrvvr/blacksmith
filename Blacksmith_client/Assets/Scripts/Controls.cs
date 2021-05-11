using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Controls : MonoBehaviour
{
    public event System.Action<Cube> OnCubeMove;

    [HideInInspector] public Cube ObjectToControl;
    [HideInInspector] public int ClickCounter = 0;
    [HideInInspector] public List<Vector3> PathPointsList;
    [SerializeField] private AudioClip[] _dragStopSounds;
    [SerializeField] private AudioClip[] _dragSounds;
    private GameManager gamemanager;
    //private AudioSource audioS;
    private float firstClickTime = 0f;
    public float TimeBetweenClicks;
    private float timeBetweenSteps = 0.1f;
    private bool coroutineAllowed = true;
    private Vector3 startPos;
    private Vector3 prevPosition;
    private Vector3[] path;

    private void Start()
    {
        gamemanager = GameManager.Instance;
    }

    private void OnEnable()
    {
        Cube.OnMouseMoving += SwipeMove;
    }

    private void OnDisable()
    {
        Cube.OnMouseMoving -= SwipeMove;
    }

    void Update()
    {
        if (ObjectToControl != null)
        {
            #region double click
            if (ClickCounter == 1 && coroutineAllowed)
            {
                firstClickTime = Time.time;
                //destroy cube if possible
                StartCoroutine(DoubleClickDetection());
            }
            #endregion //можно не трогать
        }
    }
    private void SwipeMove()
    {
        if (!gamemanager.isAllFramesFilled && PathPointsList.Count > 0 && ObjectToControl != null)
        {
            path = PathPointsList.ToArray();
            startPos = ObjectToControl.transform.position;
            gamemanager.CanChooseCube = false;
            ObjectToControl.BoxCollider.enabled = false;
            var time = timeBetweenSteps * path.Length;

            MusicManager.Instance.PlaySound(_dragSounds[Random.Range(0, _dragSounds.Length)], true);

            ObjectToControl.transform.DOPath(path, time)
                .OnWaypointChange(CheckStep)
                .OnComplete(UpdatePositions)
                .SetId(ObjectToControl.GetInstanceID());
        }
    }
    private void CheckStep(int waypointIndex)
    {
        //if (audioS.isPlaying)
        //{
        //    audioS.pitch = Random.Range(0.7f, 1.2f);
        //    audioS.Stop();
        //    audioS.Play();
        //}
        //else
        //{
        //    audioS.pitch = Random.Range(0.7f, 1.2f);
        //    audioS.Play();
        //}

        //проверить, передвигался ли куб по x и z, чтобы вычитать прочность (передвижение по y не считается) 
        if (waypointIndex == 0)
        {
            if (path[waypointIndex].x != startPos.x || path[waypointIndex].z != startPos.z)
            {
                HadleDurability();
            }
            prevPosition = path[0];
        }
        else if (waypointIndex < path.Length)
        {
            Vector3 currentPosition = path[waypointIndex];
            if (currentPosition.x != prevPosition.x || currentPosition.z != prevPosition.z)
            {
                HadleDurability();
            }
            prevPosition = currentPosition;
        }  
    }

    private void HadleDurability()
    {
        if (ObjectToControl.canBreak)
        {
            ObjectToControl.durability--;
            OnCubeMove?.Invoke(ObjectToControl);
            if (ObjectToControl.durability == 0)
            {
                gamemanager.AddCubeRated(1);
                DOTween.Kill(ObjectToControl.GetInstanceID());
                ObjectToControl.Kill();
                gamemanager.CanChooseCube = true;
            }
            ObjectToControl.UpdateMaterialByDurability();
        }
    }
    private void UpdatePositions()
    {
        gamemanager.CanChooseCube = true;
        ObjectToControl.BoxCollider.enabled = true;
        //иногда куб самую малость не доезжает до нулевой отметки и из-за этого не засчитывается, поэтому вот костылик
        if (ObjectToControl.transform.position.y < 1f)
        {
            ObjectToControl.transform.position = new Vector3(ObjectToControl.transform.position.x, 0, ObjectToControl.transform.position.z);
        }
        ObjectToControl.UpdateMoveState();
        ObjectToControl.UpdateCubeAt(startPos + Vector3.down);
        ObjectToControl.UpdateCubeAt(ObjectToControl.transform.position + Vector3.down);
        MusicManager.Instance.PlaySound(_dragStopSounds[Random.Range(0, _dragStopSounds.Length)]);
    }
   
    //двойной клик и удаление кубов. можно не трогать
    private IEnumerator DoubleClickDetection()
    {
        coroutineAllowed = false;
        while (Time.time < firstClickTime + TimeBetweenClicks)
        {
            if (ClickCounter == 2)
            {
                int framesAmount = gamemanager.FramesAmount;
                int framesFilled = gamemanager.FilledFrames;
                bool isCubeAbove = ObjectToControl.CheckCubeAt(ObjectToControl.transform.position + Vector3.up);
                if (!isCubeAbove && (framesAmount == framesFilled))
                {
                    DOTween.KillAll();
                    ObjectToControl.Kill();
                }    
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        ClickCounter = 0;
        firstClickTime = 0;
        coroutineAllowed = true;
    }
}
