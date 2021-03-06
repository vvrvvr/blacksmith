using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private AudioClip _clockSound;

    private void OnEnable() 
    {
        LevelManager.OnLevelLoad += OnLevelLoad;
        IngotDragAndDrop.OnIngotPlaced += OnIngotPlaced;
    }
    private void OnDisable()
    {
        LevelManager.OnLevelLoad -= OnLevelLoad;
        IngotDragAndDrop.OnIngotPlaced -= OnIngotPlaced;
    }

    [SerializeField] private GameObject _timer;
    [SerializeField] private Text _text;
    [SerializeField] private Volume volume;
    private Bloom bloom;

    private float _baseBloomIntensity;
    private float _baseRemainingTime;
    private float _remainingTime;
    private bool _ingotPlaced;

    private void Start() {
        volume.profile.TryGet<Bloom>(out bloom);
        if(bloom)
        {
            _baseBloomIntensity = bloom.intensity.value;
        }
        _ingotPlaced = false;
    }

    private void OnLevelLoad(Level level)
    {
        _baseRemainingTime = level.TimerSec;
        _remainingTime = level.TimerSec;
        if (_remainingTime > 15 || _remainingTime <= 0)
        {
            _timer.SetActive(false);
        }
        else
        {
            MusicManager.Instance.PlayMusic(_clockSound);
        }
        UpdateText();
    }

    private void OnIngotPlaced(Ingot ingot)
    {
        _ingotPlaced = true;
    }

    private void UpdateText()
    {
        string minutes = Mathf.Floor(_remainingTime / 60).ToString("00");
        string seconds = (_remainingTime % 60).ToString("00");
        _text.text = $"{minutes}:{seconds}";
    }

    private void Update() {
        if (_ingotPlaced && _remainingTime > 0f)
        {
            _remainingTime -= Time.deltaTime;

            if (_timer.activeSelf == false && _remainingTime <= 15)
            {
                MusicManager.Instance.PlayMusic(_clockSound);
                _timer.SetActive(true);
            }

            if(_remainingTime <= 0f)
            {
                MusicManager.Instance.PlayPrevMusic();
                _remainingTime = 0;
                GameManager.Instance.Defeat();
            }

            UpdateText();

            if(bloom)
            {
                float bloomIntensity = _baseBloomIntensity * _remainingTime / _baseRemainingTime;
                bloom.intensity.value = bloomIntensity;
            }
        }
    }
}
