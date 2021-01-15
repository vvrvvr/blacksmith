using System;
using UnityEngine;
using System.Collections;

public class Cube : MonoBehaviour
{
    [Header("Settings")]
    public float MinYCoord = 0f;

    [Header("Drag")]
    [SerializeField] private float deadZone = 0.6f;
    [SerializeField] private float movementSpeed;
    private Transform colliderPlane;
    private Vector3 firstDragPos = Vector3.zero;
    private Vector3 currentPoint;
    private Vector3 direction;

    [Header("References")]
    [SerializeField] private Material red;
    [SerializeField] private Material green;
    [SerializeField] private LayerMask colliderPlaneLayer;
    [SerializeField] private LayerMask cubesLayer;

    private MeshRenderer myRend;
    private BoxCollider boxCollider;

    public static Action<bool> OnDestroyEvent;
    public static Action<bool> OnStateChange;
    public static Action<Vector3> OnMouseMoving;

    public bool isInitialized { get; private set; } = false;
    public bool CanMove { get; private set; } = true;
    private bool isMovingAllowed = true;
    private bool isCubeRated = false;

    private Vector3 halfCubeDimensions = new Vector3(0.2f, 0.2f, 0.2f); // fix this

    private void Start()
    {
        myRend = GetComponent<MeshRenderer>();
        boxCollider = GetComponent<BoxCollider>();
    }

    /// <summary>
    /// Use it instead of Destroy()
    /// </summary>
    public void Kill()
    {
        if (transform.position.y <= MinYCoord)
            isCubeRated = true;
        UpdateCubeAt(transform.position + Vector3.down);
        transform.position = new Vector3(-10000f, -10000f, -10000f);
        StartCoroutine(LateDestroy());
    }

    private IEnumerator LateDestroy()
    {
        yield return new WaitForFixedUpdate();
        colliderPlane.gameObject.SetActive(false);
        StopAllCoroutines();
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (gameObject.scene.isLoaded)
        {
            SetState(false);
            UpdateCubeAt(transform.position + Vector3.down);
            OnDestroyEvent?.Invoke(isCubeRated);
        }
    }

    public void Init(Transform cPlane)
    {
        isInitialized = true;
        gameObject.layer = LayerMaskToLayer(cubesLayer);
        boxCollider.enabled = true;
        TurnRed();
        colliderPlane = cPlane;
    }

    public void SetState(bool canMove)
    {
        if (CanMove == canMove)
            return;
        CanMove = canMove;
        OnStateChange?.Invoke(CanMove);
    }

    private int LayerMaskToLayer(LayerMask layerMask)
    {
        int layerNumber = 0;
        int layer = layerMask.value;
        while (layer > 0)
        {
            layer = layer >> 1;
            layerNumber++;
        }
        return layerNumber - 1;
    }

    public void MoveTo(Vector3 position)
    {
        if (isMovingAllowed)
        {
            Vector3 prevPos = transform.position;
            Vector3 direction = position - transform.position;
            boxCollider.enabled = false;
            StartCoroutine(MovingWithSpeed(direction, position, prevPos));
        }
    }

    /// <summary>
    /// compare two vectors
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public bool V3Equal(Vector3 a, Vector3 b)
    {
        return Vector3.SqrMagnitude(a - b) < 0.0001;
    }

    private IEnumerator MovingWithSpeed(Vector3 dir, Vector3 pos, Vector3 prevP)
    {

        isMovingAllowed = false;
        while (!V3Equal(pos, transform.position))
        {
            transform.Translate(dir * movementSpeed);
            yield return new WaitForEndOfFrame();
        }
        transform.position = pos;
        UpdateMoveState();
        UpdateCubeAt(prevP + Vector3.down);
        UpdateCubeAt(transform.position + Vector3.down);
        boxCollider.enabled = true;
        isMovingAllowed = true;
    }

    private void UpdateCubeAt(Vector3 position)
    {
        if (Physics.Raycast(position + Vector3.up, Vector3.down, out RaycastHit hit, 1f, cubesLayer))
        {
            if (hit.transform.TryGetComponent(out Cube cube))
            {
                cube.UpdateMoveState(true);
            }
        }
    }

    public void UpdateMoveState(bool lateUpdate = false)
    {
        if (lateUpdate)
            StartCoroutine(LateCubeUpdate());
        else
            UpdateState();
    }

    // Update Move State after current frame
    private IEnumerator LateCubeUpdate()
    {
        yield return new WaitForFixedUpdate();
        UpdateState();
    }

    // Update Move State at current frame
    private void UpdateState()
    {
        if (transform.position.y <= MinYCoord || CheckCubeAt(transform.position + Vector3.up))
            SetState(false);
        else
            SetState(true);
    }

    public bool CheckCubeAt(Vector3 position)
    {
        if (Physics.CheckBox(position, halfCubeDimensions, Quaternion.identity, cubesLayer))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// change cube color when selected
    /// </summary>
    public void TurnGreen()
    {
        if (isInitialized)
            myRend.material = green;
    }

    /// <summary>
    /// return default cube color
    /// </summary>
    public void TurnRed()
    {
        if (isInitialized)
            myRend.material = red;
    }

    private void OnMouseDown()
    {
        RaycastHit rayHit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out rayHit, Mathf.Infinity, cubesLayer))
        {
            colliderPlane.position = transform.position;
            firstDragPos = transform.position;
            colliderPlane.position = firstDragPos;
            colliderPlane.gameObject.SetActive(true);
        }
    }

    private void OnMouseDrag()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, colliderPlaneLayer))
        {
            currentPoint = hit.point;
            direction = currentPoint - firstDragPos;
            direction.y = 0f;
            if (direction.magnitude > deadZone)
            {
                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
                {
                    direction = new Vector3(Mathf.Sign(direction.x), 0, 0);
                }
                else
                    direction = new Vector3(0, 0, Mathf.Sign(direction.z));
            }
            else
            {
                direction = Vector3.zero;
            }
        }
    }

    private void OnMouseUp()
    {
        if (direction != Vector3.zero)
            OnMouseMoving?.Invoke(direction);
        firstDragPos = Vector3.zero;
        direction = Vector3.zero;
        colliderPlane.gameObject.SetActive(false);
    }
}
