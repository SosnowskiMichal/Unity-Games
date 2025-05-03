using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour {

    private float timeElapsed = 0f;
    private bool isRunning = false;

    private TextMeshProUGUI timerText;

    private void Awake() {
        timerText = GetComponent<TextMeshProUGUI>();
    }

    public void StartTimer() {
        timeElapsed = 0f;
        isRunning = true;
        UpdateGameTimer();
    }

    public void StopTimer() {
        isRunning = false;
    }

    private void Update() {
        if (isRunning) {
            timeElapsed += Time.deltaTime;
            UpdateGameTimer();
        }
    }

    private void UpdateGameTimer() {
        string time = GetGameTime();
        timerText.text = time;
    }

    public string GetGameTime() {
        int minutes = Mathf.FloorToInt(timeElapsed / 60);
        int seconds = Mathf.FloorToInt(timeElapsed % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

}
