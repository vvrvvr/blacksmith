using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    
    [SerializeField] private GameObject[] dotArray = new GameObject[40];
    [SerializeField] private Controls controls;
    [SerializeField] private LayerMask cubesLayer;
    [SerializeField] private LayerMask handLayer;
    private List<Vector3> pathPointsList = new List<Vector3>();
    private Vector3 currentPos = Vector3.zero;
    private Vector3 dotPosToAdd = Vector3.zero;
    private Vector3 currentDir = Vector3.zero;
    private int currentSteps = 0;
    private bool isSwordHandOnTheWay;
    private Vector3 halfCubeDimensions = new Vector3(0.2f, 0.2f, 0.2f); // fix this

    public static Trajectory Instance { get; private set; }

    //private int pathLenghtTemp = 0;
    //private bool isPathEnd;

    void Awake()
    {
        Instance = this;
    }

    public void HandleTrajectory(Vector3 pointerDir, Vector3 startPos)
    {
        //if (currentDir == pointerDir.normalized)
        //{
        //    if (pathLenghtTemp != (int)pointerDir.magnitude && !isPathEnd)
        //    {
        //        Hide();
        //        ConstructPath(pointerDir, startPos);
        //    }
        //    else if (pathLenghtTemp > (int)pointerDir.magnitude && isPathEnd)
        //    {
        //        Hide();
        //        ConstructPath(pointerDir, startPos);
        //    }
        //}
        //else
        //{
        Hide();
        ConstructPath(pointerDir, startPos);
        //}
    }

    private void ConstructPath(Vector3 pointerDir, Vector3 startPos)
    {
        //Debug.Log("construct path");
        isSwordHandOnTheWay = false;
        currentDir = pointerDir.normalized;
        currentPos = startPos;
        currentSteps = (int)pointerDir.magnitude;
        //pathLenghtTemp = (int)pointerDir.magnitude;
        // isPathEnd = false;

        while (currentSteps > 0)
        {
            if (CheckAndMovePath(currentDir))
                break;
            currentSteps--;
        }
        controls.PathPointsList.Clear();
        controls.PathPointsList = pathPointsList;
        pathPointsList = pathPointsList.Distinct().ToList();
        //здесь имеем лист с координатами ключевых точек и начинаем строить
        for (int i = 0; i < pathPointsList.Count; i++)
        {
            if (i >= dotArray.Length) //на случай, если координат точек будет больше, чем префабов точек 
                break;
            dotArray[i].SetActive(true);
            dotArray[i].transform.position = pathPointsList[i];
        }
    }

    public void Hide()
    {
        pathPointsList.Clear();
        foreach (GameObject dot in dotArray)
        {
            dot.SetActive(false);
        }
    }

    public bool CheckAndMovePath(Vector3 direction)
    {
        if (currentPos.y == 0f)
        {
            //isPathEnd = true;
            return true;
        }
        Vector3 placeToCheck = currentPos + direction;
        if (!CheckCubeAt(placeToCheck)) //if place to move not occupied by another cube
        {
            dotPosToAdd = placeToCheck;
            Vector3 underplaceToCheck = placeToCheck + new Vector3(0, -1, 0);
            while (!CheckCubeAt(underplaceToCheck) && underplaceToCheck.y >= 0f) // find  lowest cube, or floor (MinYCoord) 
            {
                underplaceToCheck += new Vector3(0, -1, 0);
            }
            underplaceToCheck += new Vector3(0, 1, 0);

            if (!isSwordHandOnTheWay)// can be false if sword hand on cube's way
            {
                if (V3Equal(dotPosToAdd, underplaceToCheck))
                    pathPointsList.Add(dotPosToAdd); //спуска не было, добавляем
                else
                {
                    pathPointsList.Add(dotPosToAdd); //добавляем точку начала спуска
                    var dotToAddCurrentY = dotPosToAdd.y - 1;
                    while (dotToAddCurrentY > underplaceToCheck.y)
                    {
                        var middlePointPos = new Vector3(dotPosToAdd.x, dotToAddCurrentY, dotPosToAdd.z);
                        pathPointsList.Add(middlePointPos);
                        dotToAddCurrentY--;
                    }
                    pathPointsList.Add(underplaceToCheck); //добавляем точку конца спуска
                }
                currentPos = underplaceToCheck;
            }
            else //sword hand on the way
            {
                isSwordHandOnTheWay = false;
                // isPathEnd = true;
                return true;
                // Debug.Log("cant move sword hand");
            }
        }
        else //if place occupied by another cube - try to place ObjectToControl above
        {
            Vector3 aboveplaceToCheck = placeToCheck + new Vector3(0, 1, 0);
            if (!CheckCubeAt(aboveplaceToCheck))
            {
                if (!isSwordHandOnTheWay) // can be false if sword hand on cube's way
                {
                    var dotBefore = new Vector3(currentPos.x, aboveplaceToCheck.y, currentPos.z); //для построения маршрута под прямыми углами
                    pathPointsList.Add(dotBefore);
                    pathPointsList.Add(aboveplaceToCheck);
                }
                else
                {
                    isSwordHandOnTheWay = false;
                    // isPathEnd = true;
                    return true;
                }
                currentPos = aboveplaceToCheck;
            }
            else //cube cant move above 
            {
                isSwordHandOnTheWay = false;
                // isPathEnd = true;
                return true;
                // Debug.Log("cant move above");
            }
        }
        return false;
    }

    public bool V3Equal(Vector3 a, Vector3 b)
    {
        return Vector3.SqrMagnitude(a - b) < 0.0001;
    }

    public bool CheckCubeAt(Vector3 position)
    {
        if (Physics.CheckBox(position, halfCubeDimensions, Quaternion.identity, cubesLayer))
        {
            return true;
        }
        if (Physics.CheckBox(position, halfCubeDimensions, Quaternion.identity, handLayer)) //check if there is a sword hand on  desired position
        {
            isSwordHandOnTheWay = true;
        }
        return false;
    }
}
