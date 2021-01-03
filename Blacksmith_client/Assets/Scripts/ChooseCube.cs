using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseCube : MonoBehaviour
{
    [SerializeField] LayerMask clickableLayer;
    [SerializeField] GameObject PlayerController;
    [SerializeField] private Transform colliderPlane;
    [SerializeField] private LayerMask colliderPlaneLayer;
    private Controls controlScript;
    private GameObject currentCube;

    #region vectors
    private Vector3 firstDragPos = Vector3.zero;
    private Vector3 currentPoint;
    private Vector3 baseIngotPos;
    private Vector3 direction;
    #endregion

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
                    {
                        currentCube.GetComponent<Cube>().TurnRed();
                        controlScript.ClickCounter = 0;
                    }
                    currentCube = rayHit.collider.gameObject;
                    currentCube.GetComponent<Cube>().TurnGreen();
                    colliderPlane.transform.position = currentCube.transform.position;
                    controlScript.ObjectToControl = currentCube.GetComponent<Cube>();
                    controlScript.ClickCounter++; //double tap functionality
                }
                else
                {
                    if (currentCube != null)
                    {
                        controlScript.ClickCounter++; //double tap functionality
                        colliderPlane.transform.position = currentCube.transform.position;
                    }
                }
            }
        }
    }

    //private void OnMouseDown()
    //{
    //    RaycastHit rayHit;
    //    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out rayHit, Mathf.Infinity, clickableLayer))
    //    {
    //        firstDragPos = rayHit.point;
    //        colliderPlane.position = rayHit.point;
    //        colliderPlane.gameObject.SetActive(true);
    //    }
    //}

    //private void OnMouseDrag()
    //{
    //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //    if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, colliderPlaneLayer))
    //    {
    //        currentPoint = hit.point;
    //        direction = currentPoint - firstDragPos;
    //        direction.x = Mathf.Round(direction.x);
    //        direction.y = Mathf.Round(direction.y);
    //        direction.z = Mathf.Round(direction.z);
    //        if (direction.magnitude > 1.0f)
    //            return;
    //    }
    //}
    //private void OnMouseUp()
    //{
    //    if (direction.magnitude > 0)
    //        controlScript.CheckAndMove(direction);
    //    firstDragPos = Vector3.zero;
    //    direction = Vector3.zero;
    //    colliderPlane.gameObject.SetActive(false);

    //}
}
