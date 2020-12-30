using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controls : MonoBehaviour
{
    [SerializeField] private float minYCoord;
    [SerializeField] private LayerMask cubesLayer;
    public GameObject objectToControl;
    private Vector3 halfCubeDimensions = new Vector3(0.4f, 0.4f, 0.4f);
    private Ray[] rays = new Ray[6]; 

    void Update()
    {
        if (objectToControl != null && objectToControl.transform.position.y > minYCoord)
        {
            #region input
            if (Input.GetKeyDown(KeyCode.W))
            {
               // if (ConnectedWithOnterCubes(objectToControl.transform.position))
                    CheckAndMove(new Vector3(0, 0, 1));
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
               // if (ConnectedWithOnterCubes(objectToControl.transform.position))
                    CheckAndMove(new Vector3(0, 0, -1));
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
               // if (ConnectedWithOnterCubes(objectToControl.transform.position))
                    CheckAndMove(new Vector3(1, 0, 0));
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
               // if (ConnectedWithOnterCubes(objectToControl.transform.position))
                    CheckAndMove(new Vector3(-1, 0, 0));
            }
            #endregion
        }

        void CheckAndMove(Vector3 direction)
        {
            //чекнуть нижний слой
            Vector3 placeToCheck = objectToControl.transform.position + direction;
            if (!Physics.CheckBox(placeToCheck, halfCubeDimensions, Quaternion.identity, cubesLayer))
            {
                Vector3 underCube = placeToCheck + new Vector3(0, -1, 0);
                print(underCube);
                while (!Physics.CheckBox(underCube, halfCubeDimensions, Quaternion.identity, cubesLayer) && underCube.y >= minYCoord)
                {
                    underCube += new Vector3(0, -1, 0);
                }
                underCube += new Vector3(0, 1, 0);
                print(underCube);
                if (ConnectedWithOnterCubes(underCube))
                    objectToControl.transform.position = underCube; // change positon с перемещением вниз
            }
            else
            {
                Vector3 aboveCube = placeToCheck + new Vector3(0, 1, 0);
                if (!Physics.CheckBox(aboveCube, halfCubeDimensions, Quaternion.identity, cubesLayer)
                    && !Physics.CheckBox((objectToControl.transform.position + new Vector3(0, 1, 0)), halfCubeDimensions, Quaternion.identity, cubesLayer)) // шоб над кубом небыло другого куба
                { // дописать правило, чтоб нельзя было двигать куб, если над ним другой куб
                    if (ConnectedWithOnterCubes(aboveCube))
                        objectToControl.transform.position = aboveCube; // change positon 
                }
            }
        }

        bool ConnectedWithOnterCubes(Vector3 position)
        {
            bool isConnected = false;
            rays[0] = new Ray(position, new Vector3(0, 0, 1));
            rays[1] = new Ray(position, new Vector3(0, 0, -1));
            rays[2] = new Ray(position, new Vector3(1, 0, 0));
            rays[3] = new Ray(position, new Vector3(-1, 0, 0));
            rays[4] = new Ray(position, new Vector3(0, 1, 0));
            rays[5] = new Ray(position, new Vector3(0, -1, 0));
            RaycastHit rayHit;
            for(int i = 0; i <rays.Length; i++)
            {
                if (Physics.Raycast(rays[i], out rayHit, 1.0f, cubesLayer))
                {
                    if (rayHit.collider.gameObject.CompareTag("cube") && rayHit.collider.gameObject != objectToControl)
                    {
                        isConnected = true;
                        break;
                    }
                }
            }
            return isConnected;
        }
    }

    /// <summary>
    /// restart game button
    /// </summary>
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// quit game button
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
