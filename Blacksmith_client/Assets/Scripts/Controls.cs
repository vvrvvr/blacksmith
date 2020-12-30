using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controls : MonoBehaviour
{
    [HideInInspector] public float MinYCoord;
    [SerializeField] private LayerMask cubesLayer;
    [HideInInspector] public GameObject ObjectToControl;
    [HideInInspector] public int ClickCounter = 0;
    private float firstClickTime = 0f;
    public float TimeBetweenClicks;
    private bool coroutineAllowed = true;
    private Vector3 halfCubeDimensions = new Vector3(0.49f, 0.49f, 0.49f);
    private Vector3 vectorW = new Vector3(0, 0, 1);
    private Vector3 vectorS = new Vector3(0, 0, -1);
    private Vector3 vectorA = new Vector3(-1, 0, 0);
    private Vector3 vectorD = new Vector3(1, 0, 0);

    void Update()
    {
        if (ObjectToControl != null && ObjectToControl.transform.position.y > MinYCoord)
        {
            if (ClickCounter == 1 && coroutineAllowed)
            {
                firstClickTime = Time.time;
                StartCoroutine(DoubleClickDetection());
            }
            #region input
            if (Input.GetKeyDown(KeyCode.W))
            {
                CheckAndMove(vectorW);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                CheckAndMove(vectorS);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                CheckAndMove(vectorD);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                CheckAndMove(vectorA);
            }
            #endregion
        }
    }

    /// <summary>
    /// check if ObjectToControl can move to choosen direction
    /// </summary>
    /// <param name="direction"></param>
    void CheckAndMove(Vector3 direction)
    {

        if (CheckCubeAbove()) //execute only if there are no other cubes above ObjectToControl
        {
            Vector3 placeToCheck = ObjectToControl.transform.position + direction;
            if (!Physics.CheckBox(placeToCheck, halfCubeDimensions, Quaternion.identity, cubesLayer)) //if place to move not occupied by another cube
            {
                Vector3 underplaceToCheck = placeToCheck + new Vector3(0, -1, 0);
                while (!Physics.CheckBox(underplaceToCheck, halfCubeDimensions, Quaternion.identity, cubesLayer) && underplaceToCheck.y >= MinYCoord) // find  lowest cube, or floor (MinYCoord) 
                {
                    underplaceToCheck += new Vector3(0, -1, 0);
                }
                underplaceToCheck += new Vector3(0, 1, 0);
                ObjectToControl.transform.position = underplaceToCheck; // change ObjectToControl positon
            }
            else //if place occupied by another cube - try to place ObjectToControl above
            {
                Vector3 aboveplaceToCheck = placeToCheck + new Vector3(0, 1, 0);
                if (!Physics.CheckBox(aboveplaceToCheck, halfCubeDimensions, Quaternion.identity, cubesLayer))
                {
                    ObjectToControl.transform.position = aboveplaceToCheck; // change ObjectToControl positon 
                }
            }
        }
    }

    private bool CheckCubeAbove()
    {
        if (!Physics.CheckBox((ObjectToControl.transform.position + new Vector3(0, 1, 0)), halfCubeDimensions, Quaternion.identity, cubesLayer))
            return true;
        else
            return false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private IEnumerator DoubleClickDetection()
    {
        coroutineAllowed = false;
        while (Time.time < firstClickTime + TimeBetweenClicks)
        {
            if (ClickCounter == 2)
            {
                if (CheckCubeAbove())
                    Destroy(ObjectToControl);
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        ClickCounter = 0;
        firstClickTime = 0;
        coroutineAllowed = true;
    }
}
