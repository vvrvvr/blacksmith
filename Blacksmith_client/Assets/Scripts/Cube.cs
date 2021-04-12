using System;
using UnityEngine;
using System.Collections;

public class Cube : MonoBehaviour
{
    [Header("Settings")]
    public int durability = 0;
    public float MinYCoord = 0f;

    [Header("Drag")]
   // [SerializeField] private float deadZone = 0.6f;
    public float movementSpeed;
    private Transform colliderPlane;
    private Vector3 firstDragPos = Vector3.zero;
    private Vector3 currentPoint;
    //private Vector3 direction;

    [Header("References")]
    [SerializeField] private Material[] durabilityMaterials;
    [SerializeField] private LayerMask colliderPlaneLayer;
    [SerializeField] private LayerMask cubesLayer;
    [SerializeField] private LayerMask handLayer;

    private MeshRenderer myRend;
    public BoxCollider BoxCollider;

    public Action<bool> OnDestroyEventLocal;
    public static Action<bool> OnDestroyEventGlobal;
    public static Action<bool> OnStateChange;
    public static Action<Vector3> OnMouseMoving;

    public bool isInitialized { get; private set; } = false;
    public bool CanMove { get; set; } = true;
    public bool IsMovingAllowed = true;
    private bool isCubeRated = false;
    public bool canBreak = false;
    private Material currentMaterial;

    private Vector3 halfCubeDimensions = new Vector3(0.2f, 0.2f, 0.2f); // fix this

    //cube movement
    private Vector3 pointerDirection;
    private Vector3 pathDir = Vector3.zero;
    private int steps = 0;
    private bool isTrajectoryShown;

    private void Start()
    {
        myRend = GetComponent<MeshRenderer>();
        BoxCollider = GetComponent<BoxCollider>();
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
            OnDestroyEventGlobal?.Invoke(isCubeRated);
            OnDestroyEventLocal?.Invoke(isCubeRated);
        }
    }

    public void Init(Transform cPlane)
    {
        if(durability > 0)
        {
            canBreak = true;
        }
        GetComponent<MeshRenderer>().enabled = true;
        gameObject.layer = LayerMaskToLayer(cubesLayer);
        BoxCollider.enabled = true;
        colliderPlane = cPlane;
        UpdateMaterialByDurability();
        isInitialized = true;
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

    //public void MoveTo(Vector3 position)
    //{
    //    if (isMovingAllowed)
    //    {
    //        Vector3 prevPos = transform.position;
    //        Vector3 direction = position - transform.position;
    //        boxCollider.enabled = false;
    //        StartCoroutine(MovingWithSpeed(direction, position, prevPos));
    //    }
    //}

   

    //private IEnumerator MovingWithSpeed(Vector3 dir, Vector3 pos, Vector3 prevP)
    //{
    //    isMovingAllowed = false;
    //    while (!V3Equal(pos, transform.position))
    //    {
    //        transform.Translate(dir * movementSpeed);
    //        yield return new WaitForEndOfFrame();
    //    }
    //    transform.position = pos;
    //    UpdateMoveState();
    //    UpdateCubeAt(prevP + Vector3.down);
    //    UpdateCubeAt(transform.position + Vector3.down);
    //    boxCollider.enabled = true;
    //    isMovingAllowed = true;
    //    if (canBreak)
    //    {
    //        durability--;
    //        if (durability == 0)
    //        {
    //            GameManager.Instance.AddCubeRated(1);
    //            Kill();
    //        }
    //        UpdateMaterialByDurability();
    //    }
    //}

    public void UpdateMaterialByDurability()
    {
        int index = durability - 1;
        if (index < 0) index = 0;
        if (index > durabilityMaterials.Length - 1) index = durabilityMaterials.Length - 1;

        currentMaterial = durabilityMaterials[index];
        myRend.material = currentMaterial;
    }

    public void UpdateCubeAt(Vector3 position)
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
        if(Physics.CheckBox(position, halfCubeDimensions, Quaternion.identity, handLayer)) //check if there is a sword hand on position
        {
            CanMove = false;
        }
        return false;
    }

    private void OnMouseDown()
    {
        steps = 0;
        RaycastHit rayHit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out rayHit, Mathf.Infinity, cubesLayer) && GameManager.Instance.CanChooseCube)
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
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, colliderPlaneLayer) && GameManager.Instance.CanChooseCube)
        {
            currentPoint = hit.point;
            pointerDirection = currentPoint - firstDragPos;
            pointerDirection.y = 0f;

            if (pointerDirection.magnitude > 0.1f)
            {
                if (Mathf.Abs(pointerDirection.x) > Mathf.Abs(pointerDirection.z))
                {
                    if (pointerDirection.x < 0)
                        pointerDirection = new Vector3(Mathf.Ceil(pointerDirection.x), 0, 0);
                    else
                        pointerDirection = new Vector3(Mathf.Floor(pointerDirection.x), 0, 0);
                }
                else
                {
                    if (pointerDirection.z < 0)
                        pointerDirection = new Vector3(0, 0, Mathf.Ceil(pointerDirection.z));
                    else
                        pointerDirection = new Vector3(0, 0, Mathf.Floor(pointerDirection.z));
                }
            }
            else
            {
                pointerDirection = Vector3.zero;
            }
            var currentSteps = (int)pointerDirection.magnitude;
            var currentPathDir = pointerDirection.normalized;


            if (currentSteps >= 1)
            {
                isTrajectoryShown = true;
                if ((currentSteps != steps) || !V3Equal(pathDir, currentPathDir))
                {
                    steps = currentSteps;
                    Trajectory.Instance.HandleTrajectory(pointerDirection, transform.position);
                }
            }
            else
            {
                if (isTrajectoryShown)
                {
                    isTrajectoryShown = false;
                    Trajectory.Instance.Hide();
                }
            }
            pathDir = currentPathDir;
        }
    }

    private void OnMouseUp()
    {
        if (pointerDirection != Vector3.zero && GameManager.Instance.CanChooseCube)
        {
            OnMouseMoving?.Invoke(pointerDirection);
            Trajectory.Instance.Hide();
        }
        pointerDirection = Vector3.zero;
        colliderPlane.gameObject.SetActive(false);
    }

    /// <summary>
    /// Compare two vectors
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public bool V3Equal(Vector3 a, Vector3 b)
    {
        return Vector3.SqrMagnitude(a - b) < 0.0001;
    }
}
