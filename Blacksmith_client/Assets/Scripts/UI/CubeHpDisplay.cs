using UnityEngine.UI;
using UnityEngine;

public class CubeHpDisplay : MonoBehaviour
{
    [SerializeField] private CubeSelector _selector;
    [SerializeField] private Controls _controls;
    [SerializeField] private Text _text;
    [SerializeField] private GameObject _hpPanel;

    private void OnEnable()
    {
        _selector.OnSelect += OnSelect;
        _selector.OnDeselect += OnDeselect;
        _controls.OnCubeMove += OnCubeMove;
    }

    private void OnDisable()
    {
        _selector.OnSelect -= OnSelect;
        _selector.OnDeselect -= OnDeselect;
        _controls.OnCubeMove -= OnCubeMove;
    }

    private void OnCubeMove(Cube cube)
    {
        UpdateText();
    }

    private void Awake() => _hpPanel.SetActive(false);

    private void OnSelect()
    {
        if (_selector.selectedCube.durability > 0)
        {
            _hpPanel.SetActive(true);
            UpdateText();
        }
    }

    private void OnDeselect()
    {
        _hpPanel.SetActive(false);
    }

    private void UpdateText()
    {
        if(_selector.selectedCube != null)
        {
            _text.text = "" + _selector.selectedCube.durability;
        }
    }
}
