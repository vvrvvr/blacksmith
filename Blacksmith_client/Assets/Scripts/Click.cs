using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Click : MonoBehaviour
{
    [SerializeField] LayerMask clickableLayer;
    [SerializeField] GameObject PlayerController;
    private GameObject currentCube;
    // Start is called before the first frame update
    void Start()
    {
        
    }

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
                        currentCube.GetComponent<CkickOn>().TurnRed();
                    currentCube = rayHit.collider.gameObject;
                    currentCube.GetComponent<CkickOn>().TurnGreen();
                    PlayerController.GetComponent<Controls>().objectToControl = currentCube;
                }
            }
        }
    }
}
