using UnityEngine.EventSystems;
using UnityEngine;

public class IngotDragAndDrop : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Ingot ingot;
   
    [SerializeField] private LayerMask colliderPlaneLayer;
    [SerializeField] private LayerMask ingotLayer;
    public Transform colliderPlane;
    private Camera mainCamera;
    private bool _isDrag;

    public static System.Action<Ingot> OnIngotPlaced;

    #region DoubleClick
    private const float DoubleClickSpeed = 0.25f;
    private float doubleClickTime = 0f;
    #endregion

    #region Drag & Drop
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

        if(Input.GetMouseButtonDown(0))
        {
            MouseDown();
            _isDrag = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            MouseUp();
            _isDrag = false;
        }

        if (_isDrag)
        {
            MouseDrag();
        }
    }

    private void MouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        //Double click functionality
        if (doubleClickTime <= DoubleClickSpeed)
        {
            if (ingot.InitCubes())
            {
                OnIngotPlaced?.Invoke(ingot);
                enabled = false;
            }
        }
        doubleClickTime = 0f;

        //Drag and drop init start position
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, ingotLayer))
        {
            baseIngotPos = transform.position;
            firstDragPos = hit.point;
            colliderPlane.position = hit.point;
            colliderPlane.gameObject.SetActive(true);
        }
    }

    private void MouseDrag()
    {
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

    private void MouseUp()
    {
        firstDragPos = Vector3.zero;
        colliderPlane.gameObject.SetActive(false);
    }
}
