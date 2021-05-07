using UnityEngine.EventSystems;
using UnityEngine;

public class CameraController : MonoBehaviour, IDragHandler
{
    [ReadOnly] public Transform RotationCenter;
    [SerializeField] private Transform _cameraHolder;
    [SerializeField] private float _speed;

    private void OnEnable() => LevelManager.OnLevelLoad += OnLevelLoad;
    private void OnDisable() => LevelManager.OnLevelLoad -= OnLevelLoad;

    private void OnLevelLoad(Level level)
    {
        if (level.RotationCenter != null)
        {
            RotationCenter = level.RotationCenter;
            _cameraHolder.position += RotationCenter.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
	{
        if (RotationCenter != null)
        {
            _cameraHolder.RotateAround(RotationCenter.position, Vector3.up, eventData.delta.x * Time.deltaTime * _speed);
        }
        else
        {
            _cameraHolder.Rotate(Vector3.up * eventData.delta.x * Time.deltaTime * _speed);
        }
    }
}