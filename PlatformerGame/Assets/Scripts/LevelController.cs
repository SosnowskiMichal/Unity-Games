using TMPro;
using UnityEngine;

public class LevelController : MonoBehaviour {

    public int CurrentBalloonsToPop { get; private set; }

    [SerializeField] private int _levelNumber;
    [SerializeField] private int _balloonsToPop;
    [SerializeField] private int _levelTimeLimit;

    [SerializeField] private GameObject _levelCompletedPanel;
    [SerializeField] private GameObject _levelFailedPanel;
    [SerializeField] private TextMeshProUGUI _levelFailedMessage;
    [SerializeField] private TextMeshProUGUI _levelInfo;
    [SerializeField] private TextMeshProUGUI _balloonCounter;
    [SerializeField] private TextMeshProUGUI _livesCounter;

    private Timer _timer;

    private void Awake() {
        _timer = GetComponent<Timer>();
    }

    private void Start() {
        _levelFailedPanel.SetActive(false);
        _levelCompletedPanel.SetActive(false);
        Time.timeScale = 1;
        _timer.SetTimer(_levelTimeLimit);
        _timer.StartTimer();
        CurrentBalloonsToPop = _balloonsToPop;
        _levelInfo.text = $"Level {_levelNumber}";
        _balloonCounter.text = $"Balloons: {CurrentBalloonsToPop}";
        _livesCounter.text = $"Lives: 3";
    }

    public void BalloonPopped() {
        CurrentBalloonsToPop--;
        _balloonCounter.text = $"Balloons: {CurrentBalloonsToPop}";
        if (CurrentBalloonsToPop <= 0) {
            LevelCompleted();
        }
    }

    public void LifeLost(int lives) {
        _livesCounter.text = $"Lives: {lives}";
    }

    public void AllLivesLost() {
        LevelFailed(true, false);
    }

    public void TimeRanOut() {
        LevelFailed(false, true);
    }

    private void LevelFailed(bool lives = false, bool time = false) {
        Time.timeScale = 0;
        _timer.StopTimer();

        if (lives) {
            _levelFailedMessage.text = "All lives lost!";
        }
        else if (time) {
            _levelFailedMessage.text = "Time ran out!";
        }

        _levelFailedPanel.SetActive(true);
    }

    public void RestartLevel() {
        SceneController.Instance.ReloadScene();
    }

    public void ReturnToMenu() {
        SceneController.Instance.LoadScene("Main Menu");
    }

    private void LevelCompleted() {
        _levelCompletedPanel.SetActive(true);
        _timer.StopTimer();
        Time.timeScale = 0;
    }

    public void LoadNextLevel() {
        SceneController.Instance.LoadNextScene();
    }

}
