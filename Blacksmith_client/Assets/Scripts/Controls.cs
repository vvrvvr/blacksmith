using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controls : MonoBehaviour
{
    [SerializeField] private LayerMask cubesLayer;
    [SerializeField] private LayerMask wireframeLayer;
    [HideInInspector] public Cube ObjectToControl;
    [HideInInspector] public int ClickCounter = 0;
    private float firstClickTime = 0f;
    public float TimeBetweenClicks;
    private bool coroutineAllowed = true;
    private GameManager gamemanager;

    private void Start()
    {
        gamemanager = GameManager.Instance;
    }

    private void OnEnable()
    {
        Cube.OnMouseMoving += CheckAndMove;
    }

    private void OnDisable()
    {
        Cube.OnMouseMoving -= CheckAndMove;
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
        }
    }

    public void CheckAndMove(Vector3 direction)
    {
        if(ObjectToControl.CanMove && !gamemanager.isAllFramesFilled)
        {
            Vector3 placeToCheck = ObjectToControl.transform.position + direction;

            //if place to move not occupied by another cube
            if (!ObjectToControl.CheckCubeAt(placeToCheck)) 
            {
                Vector3 underplaceToCheck = placeToCheck + new Vector3(0, -1, 0);

                // Check anvil borders
                float distance = (ObjectToControl.transform.position - placeToCheck).magnitude;
                if(Physics.CheckBox(placeToCheck, new Vector3(0.2f, 0.2f, 0.2f), Quaternion.identity))
                {
                    Debug.DrawLine(ObjectToControl.transform.position, placeToCheck, Color.green, 5f);
                    ObjectToControl.CanMove = true;
                    return;
                }

                // find  lowest cube, or floor (MinYCoord) 
                while (!ObjectToControl.CheckCubeAt(underplaceToCheck) && underplaceToCheck.y >= ObjectToControl.MinYCoord) 
                {
                    underplaceToCheck += new Vector3(0, -1, 0);
                }
                underplaceToCheck += new Vector3(0, 1, 0);

                // can be false if sword hand on cube's way
                if (ObjectToControl.CanMove) 
                    ObjectToControl.MoveTo(underplaceToCheck);
                else
                    ObjectToControl.CanMove = true;
            }
            //if place occupied by another cube - try to place ObjectToControl above
            else 
            {
                Vector3 aboveplaceToCheck = placeToCheck + new Vector3(0, 1, 0);
                if (!ObjectToControl.CheckCubeAt(aboveplaceToCheck))
                {
                    // can be false if sword hand on cube's way
                    if (ObjectToControl.CanMove) 
                        ObjectToControl.MoveTo(aboveplaceToCheck);
                    else
                        ObjectToControl.CanMove = true;
                }
            }
        }
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
