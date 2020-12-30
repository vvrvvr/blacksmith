using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controls : MonoBehaviour
{
    [SerializeField] private float minYCoord;
    [SerializeField] private LayerMask cubesLayer;
    public GameObject ObjectToControl;
    private Vector3 halfCubeDimensions = new Vector3(0.49f, 0.49f, 0.49f);
    private Ray[] rays = new Ray[6];
    private Vector3 vectorW = new Vector3(0, 0, 1);
    private Vector3 vectorS = new Vector3(0, 0, -1);
    private Vector3 vectorA = new Vector3(-1, 0, 0);
    private Vector3 vectorD = new Vector3(1, 0, 0);

    void Update()
    {
        if (ObjectToControl != null && ObjectToControl.transform.position.y > minYCoord)
        {
            #region input
            if (Input.GetKeyDown(KeyCode.W))
            {
                CheckAndMove(vectorW);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                CheckAndMove(vectorS);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                CheckAndMove(vectorD);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                CheckAndMove(vectorA);
            }
            #endregion
        }

        void CheckAndMove(Vector3 direction)
        {

            if (!Physics.CheckBox((ObjectToControl.transform.position + new Vector3(0, 1, 0)), halfCubeDimensions, Quaternion.identity, cubesLayer)) //execute only if there are no other cubes above ObjectToControl
            {
                Vector3 placeToCheck = ObjectToControl.transform.position + direction;
                if (!Physics.CheckBox(placeToCheck, halfCubeDimensions, Quaternion.identity, cubesLayer)) //if place to move not occupied by another cube
                {
                    Vector3 underplaceToCheck = placeToCheck + new Vector3(0, -1, 0);
                    while (!Physics.CheckBox(underplaceToCheck, halfCubeDimensions, Quaternion.identity, cubesLayer) && underplaceToCheck.y >= minYCoord) // find  lowest cube, or floor (minYcoord) 
                    {
                        underplaceToCheck += new Vector3(0, -1, 0);
                    }
                    underplaceToCheck += new Vector3(0, 1, 0);
                    if (ConnectedWithOnterCubes(underplaceToCheck))
                        ObjectToControl.transform.position = underplaceToCheck; // change ObjectToControl positon
                }
                else //if place occupied by another cube - try to place ObjectToControl above
                {
                    Vector3 aboveplaceToCheck = placeToCheck + new Vector3(0, 1, 0);
                    if (!Physics.CheckBox(aboveplaceToCheck, halfCubeDimensions, Quaternion.identity, cubesLayer))
                    {
                        if (ConnectedWithOnterCubes(aboveplaceToCheck))
                            ObjectToControl.transform.position = aboveplaceToCheck; // change ObjectToControl positon 
                    }
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
            for (int i = 0; i < rays.Length; i++)
            {
                if (Physics.Raycast(rays[i], out rayHit, 1.0f, cubesLayer))
                {
                    if (rayHit.collider.gameObject.CompareTag("cube") && rayHit.collider.gameObject != ObjectToControl)
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
