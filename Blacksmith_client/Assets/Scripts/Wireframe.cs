using UnityEngine;
using System;

public class Wireframe : MonoBehaviour
{
    public static Action<Wireframe> OnStateChange;
    public bool IsFilled { get; private set; }

    public void SetState(bool setFilled)
    {
        if (IsFilled == setFilled)
            return;
        IsFilled = setFilled;
        OnStateChange?.Invoke(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Cube cube))
        {
            SetState(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Cube cube))
        {
            SetState(false);
        }
    }
}
