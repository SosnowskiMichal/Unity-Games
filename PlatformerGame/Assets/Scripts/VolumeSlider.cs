using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour {

    private Slider _volumeSlider;
    private readonly float _defaultMusicVolume = 0.5f;

    private void Awake() {
        _volumeSlider = GetComponent<Slider>();
    }

    private void Start() {
        if (AudioController.Instance != null) {
            _volumeSlider.value = AudioController.Instance.MusicVolume;
        }
        else {
            _volumeSlider.value = _defaultMusicVolume;
        }
    }

    public void OnVolumeChange() {
        AudioController.Instance.ChangeMusicVolume(_volumeSlider.value);
    }

    public void OnToggleMusic() {
        AudioController.Instance.ToggleMusic();
    }

}
