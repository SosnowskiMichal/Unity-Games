using UnityEngine;

public class AudioController : MonoBehaviour {

    public static AudioController Instance { get; private set; }
    public float MusicVolume { get; private set; } = 0.5f;

    [SerializeField] private AudioSource _musicSource;

    [SerializeField] private AudioClip _backgroundMusic;

    private bool _isMusicOn = true;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        _musicSource.clip = _backgroundMusic;
        _musicSource.loop = true;
        _musicSource.volume = MusicVolume;
        _musicSource.Play();
    }

    public void ChangeMusicVolume(float volume) {
        MusicVolume = volume;
        _musicSource.volume = volume;
    }

    public void ToggleMusic() {
        _isMusicOn = !_isMusicOn;
        _musicSource.volume = _isMusicOn ? MusicVolume : 0;
    }

}
