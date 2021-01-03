using System.Collections.Generic;
using UnityEngine;

public class Ingot : MonoBehaviour
{
    [Header("Ingot Size")]
    [SerializeField] private int width;
    [SerializeField] private int length;
    [SerializeField] private int height;

    [Header("Settings")]
    [SerializeField] private Cube cubePrefab;
    [SerializeField] private LayerMask cubeLayer;
    private BoxCollider boxCollider;
    private int OverlappedCubes = 0;
    public bool IsInit { get; private set; }
    public List<Cube> Cubes { get; private set; } = new List<Cube>();

    private void Awake()
    {
        IsInit = false;
    }

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.size = new Vector3(width - 0.1f, height - 0.1f, length - 0.1f);
        SpawnCubes();
        boxCollider.center = new Vector3((width - 1) * 0.5f, (height - 1) * 0.5f, (length - 1) * 0.5f);
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
                    Cubes.Add(cube);
                }
            }
        }
    }

    public bool InitCubes()
    {
        if (OverlappedCubes == 0)
            return false;
        boxCollider.enabled = false;
        foreach (Cube cube in Cubes)
            cube.Init(GetComponent<IngotDragAndDrop>().colliderPlane);
        foreach (Cube cube in Cubes)
            cube.UpdateMoveState();
        IsInit = true;
        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMaskToLayer(cubeLayer))
            OverlappedCubes++;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMaskToLayer(cubeLayer))
            OverlappedCubes--;
    }

    // Duplicated function....
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
}
