using System.Collections.Generic;
using UnityEngine;

public class Ingot : MonoBehaviour
{
    [Header("Ingot Size")]
    [SerializeField] private int width;
    [SerializeField] private int length;
    [SerializeField] private int height;

    [Header("Prefabs")]
    [SerializeField] private Cube cubePrefab;
    private BoxCollider boxCollider;
    public List<Cube> Cubes { get; private set; } = new List<Cube>();

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.size = new Vector3(width, height, length);
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

    public void InitCubes()
    {
        boxCollider.enabled = false;
        foreach (Cube cube in Cubes)
            cube.Init();
        foreach (Cube cube in Cubes)
            cube.UpdateMoveState();
    }
}
