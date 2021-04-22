using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controls : MonoBehaviour
{
    //[SerializeField] private LayerMask cubesLayer;
    //[SerializeField] private LayerMask wireframeLayer;
    [SerializeField] private LayerMask anvilBorder;
    [HideInInspector] public Cube ObjectToControl;
    [HideInInspector] public int ClickCounter = 0;
    private float firstClickTime = 0f;
    public float TimeBetweenClicks;
    private bool coroutineAllowed = true;
    private GameManager gamemanager;
    private bool isCanMoveNextStep;
    private Coroutine swipeCoroutine;
    private bool crRunning;
    private bool isMoveBlocked;
    private AudioSource audioS;

    public event System.Action<Cube> OnCubeMove;

    private void Start()
    {
        gamemanager = GameManager.Instance;
        audioS = GetComponent<AudioSource>();
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
                StartCoroutine(DoubleClickDetection());
            }
            #endregion
            #region input
            if (Input.GetKeyDown(KeyCode.D))
            {
                CheckAndMove(Vector3.forward);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                CheckAndMove(Vector3.back);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                CheckAndMove(Vector3.right);
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                CheckAndMove(Vector3.left);
            }
            #endregion
            if (!ObjectToControl.CanMove && crRunning) //kill coroutine if cube cant move
            {
                KillCoroutine();
                //Debug.Log("killed");
            }
        }
    }

    public void CheckAndMove(Vector3 direction)
    {
	    if(ObjectToControl != null && ObjectToControl.CanMove && !gamemanager.isAllFramesFilled)
        {
            Vector3 placeToCheck = ObjectToControl.transform.position + direction;

            //if place to move not occupied by another cube
            if (!ObjectToControl.CheckCubeAt(placeToCheck)) 
            {
                Vector3 underplaceToCheck = placeToCheck + new Vector3(0, -1, 0);

                // Check anvil borders
                float distance = (ObjectToControl.transform.position - placeToCheck).magnitude;
                if(Physics.CheckBox(placeToCheck, new Vector3(0.2f, 0.2f, 0.2f), Quaternion.identity, anvilBorder))
                {
                    Debug.DrawLine(ObjectToControl.transform.position, placeToCheck, Color.green, 5f);
                    ObjectToControl.CanMove = true;
                    return; /////////////////
                }

                // find  lowest cube, or floor (MinYCoord) 
                while (!ObjectToControl.CheckCubeAt(underplaceToCheck) && underplaceToCheck.y >= ObjectToControl.MinYCoord)
                {
                    underplaceToCheck += new Vector3(0, -1, 0);
                }
                underplaceToCheck += new Vector3(0, 1, 0);
                if (ObjectToControl.CanMove) // can be false if sword hand on cube's way
                    MoveTo(underplaceToCheck);
                else //sword hand on the way
                {
                    ObjectToControl.CanMove = true;
                    KillCoroutine();
                    // Debug.Log("cant move sword hand");
                }
            }
            //if place occupied by another cube - try to place ObjectToControl above
            else 
            {
                Vector3 aboveplaceToCheck = placeToCheck + new Vector3(0, 1, 0);
                if (!ObjectToControl.CheckCubeAt(aboveplaceToCheck))
                {
                    if (ObjectToControl.CanMove) // can be false if sword hand on cube's way
                        MoveTo(aboveplaceToCheck);
                    else
                    {
                        ObjectToControl.CanMove = true;
                        KillCoroutine();
                    }
                }
                else //cube cant move above 
                {
                    KillCoroutine();
                    // Debug.Log("cant move above");
                }
            }
        }
    }
    public void MoveTo(Vector3 position)
    {
        if (ObjectToControl.IsMovingAllowed)
        {
            Vector3 prevPos = ObjectToControl.transform.position;
            Vector3 direction = position - ObjectToControl.transform.position;
            ObjectToControl.BoxCollider.enabled = false;
            StartCoroutine(MovingWithSpeed(direction, position, prevPos));
        }
        else
        {
            isMoveBlocked = true;
        }
    }

    /// <summary>
    /// Compare two vectors
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public bool V3Equal(Vector3 a, Vector3 b)
    {
        return Vector3.SqrMagnitude(a - b) < 0.0001;
    }

    private IEnumerator MovingWithSpeed(Vector3 dir, Vector3 pos, Vector3 prevP)
    {
        ObjectToControl.IsMovingAllowed = false;
        while (!V3Equal(pos, ObjectToControl.transform.position))
        {
            ObjectToControl.transform.Translate(dir * ObjectToControl.movementSpeed);
            yield return new WaitForEndOfFrame();
        }
        ObjectToControl.transform.position = pos;
        ObjectToControl.UpdateMoveState();
        ObjectToControl.UpdateCubeAt(prevP + Vector3.down);
        ObjectToControl.UpdateCubeAt(ObjectToControl.transform.position + Vector3.down);
        ObjectToControl.BoxCollider.enabled = true;
        ObjectToControl.IsMovingAllowed = true;
        if (ObjectToControl.canBreak)
        {
            ObjectToControl.durability--;
            OnCubeMove?.Invoke(ObjectToControl);
            if (ObjectToControl.durability == 0)
            {
                gamemanager.AddCubeRated(1);
                ObjectToControl.Kill();
                gamemanager.CanChooseCube = true;
            }
            ObjectToControl.UpdateMaterialByDurability();
        }
        isCanMoveNextStep = true;
    }
    private void KillCoroutine()
    {
        StopCoroutine(swipeCoroutine);
        crRunning = false;
        gamemanager.CanChooseCube = true;
    }

    private void SwipeMove(Vector3 pointerDir)
    {
        swipeCoroutine = StartCoroutine(SwipeMovement(pointerDir));
        if (audioS.isPlaying)
        {
            audioS.pitch = Random.Range(0.7f, 1.2f);
            audioS.Stop();
            audioS.Play();
        }
        else
        {
            audioS.pitch = Random.Range(0.7f, 1.2f);
            audioS.Play();
        }
    }
    private IEnumerator SwipeMovement(Vector3 pointerDirection)
    {
        crRunning = true;
        gamemanager.CanChooseCube = false;
        int steps = (int)pointerDirection.magnitude;
        Vector3 dir = pointerDirection.normalized;
        while (steps > 0)
        {
            //Debug.Log(steps);
            isCanMoveNextStep = false;
            isMoveBlocked = false;
            CheckAndMove(dir);
            yield return new WaitUntil(() => isCanMoveNextStep || isMoveBlocked);
            steps--;
        }
        //Debug.Log("done");
        gamemanager.CanChooseCube = true;
        crRunning = false;
    }

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
                    ObjectToControl.Kill();
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        ClickCounter = 0;
        firstClickTime = 0;
        coroutineAllowed = true;
    }
}
