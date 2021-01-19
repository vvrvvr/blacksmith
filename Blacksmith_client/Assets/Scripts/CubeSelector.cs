using UnityEngine;

public class CubeSelector : MonoBehaviour
{
    private Cube selectedCube;

    public void SelectCube(Cube cube)
    {
        gameObject.SetActive(true);
        selectedCube = cube;
        selectedCube.OnDestroyEventLocal += DeselectCube;
        transform.position = cube.transform.position;
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
    }
}
