using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [SerializeField] private AudioSource _sounds;
    [SerializeField] private AudioSource _music;
    private AudioClip _prevMusic;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySound(AudioClip clip, bool loop = false)
    {
        _sounds.loop = loop;
        _sounds.clip = clip;
        _sounds.Play();
    }

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if(_music.clip != null)
            _prevMusic = _music.clip;
        _music.loop = loop;
        _music.clip = clip;
        _music.Play();
    }

    public void SetMusicVolume(float volume, float duration = 0f)
    {
        volume = Mathf.Clamp01(volume);
        _music.volume = volume;
        if(duration > 0f)
        {
            Invoke(nameof(ResetMusicVolume), duration);
        }
        void ResetMusicVolume() => _music.volume = 1f;
    }

    public void PlayPrevMusic()
    {
        if(_prevMusic != null)
            PlayMusic(_prevMusic);
    }
}
