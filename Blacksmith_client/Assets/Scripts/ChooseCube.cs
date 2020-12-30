using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseCube : MonoBehaviour
{
    [SerializeField] LayerMask clickableLayer;
    [SerializeField] GameObject PlayerController;
    private GameObject currentCube;
    

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit rayHit;
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out rayHit, Mathf.Infinity, clickableLayer ))
            {
                if (currentCube != rayHit.collider.gameObject)
                {
                    if(currentCube != null)
                        currentCube.GetComponent<Cube>().TurnRed();
                    currentCube = rayHit.collider.gameObject;
                    currentCube.GetComponent<Cube>().TurnGreen();
                    PlayerController.GetComponent<Controls>().ObjectToControl = currentCube;
                }
            }
        }
    }
}
