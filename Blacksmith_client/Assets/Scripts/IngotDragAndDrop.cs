using UnityEngine.EventSystems;
using UnityEngine;

public class IngotDragAndDrop : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Ingot ingot;
    public Transform colliderPlane;
    [SerializeField] private LayerMask colliderPlaneLayer;
    [SerializeField] private LayerMask ingotLayer;
    private Transform colliderPlane;
    private Camera mainCamera;

    #region DoubleClick
    private const float DoubleClickSpeed = 0.25f;
    private float doubleClickTime = 0f;
    #endregion

    #region Dragging
    private Vector3 firstDragPos = Vector3.zero;
    private Vector3 currentPoint;
    private Vector3 baseIngotPos;
    #endregion

    public void Init(Transform _colliderPlane)
    {
        colliderPlane = _colliderPlane;
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (doubleClickTime < 1f)
            doubleClickTime += Time.deltaTime;
        if (Input.GetMouseButtonDown(0))
        {
            if (doubleClickTime <= DoubleClickSpeed)
            {
                if(ingot.InitCubes())
                    enabled = false;
            }
            doubleClickTime = 0f;
        }
    }

    private void OnMouseDown()
    {
        if (!enabled) return;
        if (EventSystem.current.IsPointerOverGameObject()) return;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, ingotLayer))
        {
            baseIngotPos = transform.position;
            firstDragPos = hit.point;
            colliderPlane.position = hit.point;
            colliderPlane.gameObject.SetActive(true);
        }
    }

    private void OnMouseDrag()
    {
        if (!enabled) return;
        if (EventSystem.current.IsPointerOverGameObject()) return;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, colliderPlaneLayer))
        {
            currentPoint = hit.point;
            Vector3 direction = currentPoint - firstDragPos;
            Vector3 newPos = baseIngotPos + direction;

            newPos.x = Mathf.Round(newPos.x);
            newPos.y = Mathf.Round(newPos.y);
            newPos.z = Mathf.Round(newPos.z);

            transform.position = newPos;
        }
    }

    private void OnMouseUp()
    {
        if (!enabled) return;
        if (EventSystem.current.IsPointerOverGameObject()) return;

        firstDragPos = Vector3.zero;
        colliderPlane.gameObject.SetActive(false);
    }


}
