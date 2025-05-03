using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour {

    [SerializeField] TextMeshProUGUI _timerText;

    private float _timeLeft;
    private bool _isRunning = false;
    private LevelController _levelController;

    private void Awake() {
        _levelController = GetComponent<LevelController>();
    }

    public void SetTimer(int seconds) {
        _timeLeft = seconds;
    }

    public void StartTimer() {
        _isRunning = true;
        UpdateTimer();
    }

    public void StopTimer() {
        _isRunning = false;
    }

    private void Update() {
        if (_isRunning) {
            _timeLeft -= Time.deltaTime;
            if (_timeLeft > 0) {
                UpdateTimer();
            }
            else {
                _timeLeft = 0;
                _isRunning = false;
                UpdateTimer();
                _levelController.TimeRanOut();
            }
        }
    }

    private void UpdateTimer() {
        string time = GetTime();
        _timerText.text = time;
    }

    public string GetTime() {
        int minutes = Mathf.FloorToInt(_timeLeft / 60);
        int seconds = Mathf.FloorToInt(_timeLeft % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

}
