using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] private Material red;
    [SerializeField] private Material green;
    [SerializeField] private LayerMask layer;
    private MeshRenderer myRend;
    private BoxCollider boxCollider;

    [HideInInspector]
    public bool isInitialized { get; private set; } = false;

    void Start()
    {
        myRend = GetComponent<MeshRenderer>();
        boxCollider = GetComponent<BoxCollider>();
    }

    public void Init()
    {
        isInitialized = true;
        gameObject.layer = LayerMaskToLayer(layer);
        boxCollider.enabled = true;
        TurnRed();
    }

    public int LayerMaskToLayer(LayerMask layerMask)
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
