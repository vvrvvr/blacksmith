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
            if (ClickCounter == 1 && coroutineAllowed)
            {
                firstClickTime = Time.time;
                StartCoroutine(DoubleClickDetection());
            }
            #region input
            if (Input.GetKeyDown(KeyCode.W))
            {
                CheckAndMove(Vector3.forward);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                CheckAndMove(Vector3.back);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                CheckAndMove(Vector3.right);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                CheckAndMove(Vector3.left);
            }
            #endregion
        }
    }

    /// <summary>
    /// check if ObjectToControl can move to choosen direction
    /// </summary>
    /// <param name="direction"></param>
    public void CheckAndMove(Vector3 direction)
    {
        if(ObjectToControl.CanMove)
        {
            Vector3 placeToCheck = ObjectToControl.transform.position + direction;
            if (!ObjectToControl.CheckCubeAt(placeToCheck)) //if place to move not occupied by another cube
            {
                Vector3 underplaceToCheck = placeToCheck + new Vector3(0, -1, 0);
                while (!ObjectToControl.CheckCubeAt(underplaceToCheck) && underplaceToCheck.y >= ObjectToControl.MinYCoord) // find  lowest cube, or floor (MinYCoord) 
                {
                    underplaceToCheck += new Vector3(0, -1, 0);
                }
                underplaceToCheck += new Vector3(0, 1, 0);
                ObjectToControl.MoveTo(underplaceToCheck);
            }
            else //if place occupied by another cube - try to place ObjectToControl above
            {
                Vector3 aboveplaceToCheck = placeToCheck + new Vector3(0, 1, 0);
                if (!ObjectToControl.CheckCubeAt(aboveplaceToCheck))
                {
                    ObjectToControl.MoveTo(aboveplaceToCheck);
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
                if (!ObjectToControl.CheckCubeAt(ObjectToControl.transform.position + Vector3.up))
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
