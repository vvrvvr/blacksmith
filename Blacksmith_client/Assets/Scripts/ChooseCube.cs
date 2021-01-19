using UnityEngine.EventSystems;
using UnityEngine;

public class ChooseCube : MonoBehaviour
{
    [SerializeField] private LayerMask clickableLayer;
    [SerializeField] private CubeSelector cubeSelector;
    private Controls controlScript;
    private Cube currentCube;

    private void Start()
    {
        controlScript = GetComponent<Controls>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            RaycastHit rayHit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out rayHit, Mathf.Infinity, clickableLayer))
            {
                if(currentCube == null || currentCube != null && currentCube.gameObject != rayHit.collider.gameObject)
                {
                    if (currentCube != null)
                    {
                        controlScript.ClickCounter = 0;
                    }
                    currentCube = rayHit.collider.gameObject.GetComponent<Cube>();
                    if(currentCube.CanMove)
                        cubeSelector.SelectCube(currentCube);
                    controlScript.ObjectToControl = currentCube;
                    controlScript.ClickCounter++; //double tap functionality
                }
                else
                {
                    if (currentCube.gameObject != null)
                    {
                        controlScript.ClickCounter++; //double tap functionality
                    }
                }
            }
        }
    }

    
}
