using System;
using UnityEngine;
using System.Collections;

public class Cube : MonoBehaviour
{
    [SerializeField] private Material red;
    [SerializeField] private Material green;
    [SerializeField] private LayerMask layer;
    public float MinYCoord = 0f;
    private MeshRenderer myRend;
    private BoxCollider boxCollider;
    public bool CanMove { get; private set; } = true;

    public static Action OnDestroyEvent;
    public static Action<bool> OnStateChange;
    private Vector3 halfCubeDimensions = new Vector3(0.49f, 0.49f, 0.49f);

    [HideInInspector]
    public bool isInitialized { get; private set; } = false;

    private void Start()
    {
        myRend = GetComponent<MeshRenderer>();
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnDestroy()
    {
        SetState(false);
        UpdateCubeAt(transform.position + Vector3.down);
        OnDestroyEvent?.Invoke();
    }

    public void Init()
    {
        isInitialized = true;
        gameObject.layer = LayerMaskToLayer(layer);
        boxCollider.enabled = true;
        TurnRed();
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
        Vector3 prevPos = transform.position;
        transform.position = position;
        UpdateMoveState();
        UpdateCubeAt(prevPos + Vector3.down);
    }

    private void UpdateCubeAt(Vector3 position)
    {
        if (Physics.Raycast(position + Vector3.up, Vector3.down, out RaycastHit hit, 1f, layer))
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
        if (Physics.CheckBox(position, halfCubeDimensions, Quaternion.identity, layer))
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
        if(isInitialized)
            myRend.material = green;
    }

    /// <summary>
    /// return default cube color
    /// </summary>
    public void TurnRed()
    {
        if(isInitialized)
            myRend.material = red;
    }

}
