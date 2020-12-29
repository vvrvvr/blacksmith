using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CkickOn : MonoBehaviour
{
    [SerializeField] Material red;
    [SerializeField] Material green;
    // Start is called before the first frame update
    private MeshRenderer myRend;
    void Start()
    {
        myRend = GetComponent<MeshRenderer>();
    }
    
    public void TurnGreen()
    {
        myRend.material = green;
    }
    public void TurnRed()
    {
        myRend.material = red;
    }
}
