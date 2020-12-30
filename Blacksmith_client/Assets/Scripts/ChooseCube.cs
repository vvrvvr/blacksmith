using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseCube : MonoBehaviour
{
    [SerializeField] LayerMask clickableLayer;
    [SerializeField] GameObject PlayerController;
    private Controls controlScript;
    private GameObject currentCube;

    private void Start()
    {
        controlScript = PlayerController.GetComponent<Controls>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit rayHit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out rayHit, Mathf.Infinity, clickableLayer))
            {
                if (currentCube != rayHit.collider.gameObject)
                {
                    if (currentCube != null)
                        currentCube.GetComponent<Cube>().TurnRed();
                    currentCube = rayHit.collider.gameObject;
                    currentCube.GetComponent<Cube>().TurnGreen();
                    controlScript.ObjectToControl = currentCube;
                    if (currentCube.transform.position.y > controlScript.MinYCoord)
                        controlScript.ClickCounter++; //double tap functionality
                }
                else
                {
                    if (currentCube != null && currentCube.transform.position.y > controlScript.MinYCoord)
                        controlScript.ClickCounter++; //double tap functionality
                }
            }
        }
    }
}
