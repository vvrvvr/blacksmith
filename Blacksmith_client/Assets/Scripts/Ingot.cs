using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Ingot : MonoBehaviour
{
    [Header("Ingot Size")]
    [SerializeField] private int width;
    [SerializeField] private int length;
    [SerializeField] private int height;

    [Header("Prefabs")]
    [SerializeField] private Cube cubePrefab;

    [Header("Settings")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform colliderPlane;
    [SerializeField] private LayerMask colliderPlaneLayer;
    [SerializeField] private LayerMask ingotLayer;

    private List<Cube> cubes = new List<Cube>();
    private BoxCollider boxCollider;

    #region DoubleClick
    private const float DoubleClickSpeed = 0.25f;
    private float doubleClickTime = 0f;
    #endregion

    #region Dragging
    private Vector3 lastDragPos = Vector3.zero;
    private Vector3 currentPoint;
    private float dragDistance = 0f;
    #endregion


    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.size = new Vector3(width, height, length);
        SpawnCubes();
        boxCollider.center = new Vector3((width - 1) * 0.5f, (height - 1) * 0.5f, (length - 1) * 0.5f);
    }

    private void Update()
    {
        if (doubleClickTime < 1f)
            doubleClickTime += Time.deltaTime;
        if(Input.GetMouseButtonDown(0))
        {
            if (doubleClickTime <= DoubleClickSpeed)
            {
                InitCubes();
                enabled = false;
                boxCollider.enabled = false;
            }
            doubleClickTime = 0f;
        }
    }

    private void OnMouseDown()
    {
        if (!enabled) return;
        
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, ingotLayer))
        {
            lastDragPos = hit.point;
            colliderPlane.position = hit.point;
            colliderPlane.gameObject.SetActive(true);
        }
    }

    private void OnMouseDrag()
    {
        if (!enabled) return;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, colliderPlaneLayer))
        {
            currentPoint = hit.point;
            Vector3 direction = currentPoint - lastDragPos;
            dragDistance += direction.magnitude;
            if (dragDistance >= 1f)
            {
                dragDistance = 0f;
                Vector3 projection1 = Vector3.Project(direction, Vector3.forward);
                Vector3 projection2 = Vector3.Project(direction, Vector3.right);
                if(projection1.magnitude > projection2.magnitude)
                    transform.position += projection1.normalized;
                else
                    transform.position += projection2.normalized;
            }
            lastDragPos = currentPoint;
        }
    }

    private void OnMouseUp()
    {
        if (!enabled) return;
        colliderPlane.gameObject.SetActive(false);
    }

    private void SpawnCubes()
    {
        for (int h = 0; h < height; h++)
        {
            for (int w = 0; w < width; w++)
            {
                for (int l = 0; l < length; l++)
                {
                    Cube cube = Instantiate(cubePrefab, transform.position + new Vector3(w, h, l), Quaternion.identity, transform);
                    cubes.Add(cube);
                }
            }
        }
    }

    private void InitCubes()
    {
        foreach(Cube cube in cubes)
        {
            cube.Init();
        }
    }
}
