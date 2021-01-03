using UnityEngine;

public class IngotDragAndDrop : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Ingot ingot;
    [SerializeField] private Transform colliderPlane;
    [SerializeField] private LayerMask colliderPlaneLayer;
    [SerializeField] private LayerMask ingotLayer;

    #region DoubleClick
    private const float DoubleClickSpeed = 0.25f;
    private float doubleClickTime = 0f;
    #endregion

    #region Dragging
    private Vector3 lastDragPos = Vector3.zero;
    private Vector3 currentPoint;
    private float dragDistance = 0f;
    #endregion

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
                float diff = Mathf.Abs(projection1.magnitude - projection2.magnitude);
                if(diff > 0f && diff <= 0.3f)
                    transform.position += (projection1.normalized + projection2.normalized).normalized;
                else
                if (projection1.magnitude > projection2.magnitude)
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


}
