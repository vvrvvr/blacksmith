using UnityEngine;

public class CubeSelector : MonoBehaviour
{
    [HideInInspector] public Cube selectedCube;
    public event System.Action OnSelect;
    public event System.Action OnDeselect;

    public void SelectCube(Cube cube)
    {
        gameObject.SetActive(true);
        selectedCube = cube;
        selectedCube.OnDestroyEventLocal += DeselectCube;
        transform.position = cube.transform.position;
        OnSelect?.Invoke();
    }

    private void Update()
    {
        if(selectedCube != null)
        {
            transform.position = selectedCube.transform.position;
        }
    }

    private void OnDisable()
    {
        selectedCube.OnDestroyEventLocal -= DeselectCube;
    }

    private void DeselectCube(bool r)
    {
        gameObject.SetActive(false);
        selectedCube = null;
        OnDeselect?.Invoke();
    }
}
