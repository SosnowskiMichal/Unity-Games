using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

    [SerializeField] private GameObject startMenuPanel;
    [SerializeField] private GameObject gameGrid;
    [SerializeField] private GameObject gameOverlay;
    [SerializeField] private GameObject gameOverPanel;

    [SerializeField] private Slider[] sliders;
    [SerializeField] private TextMeshProUGUI[] sliderValueTexts;

    private readonly string mineCounterText = "Mines left: ";
    private GameController gameController;
    private int width;
    private int height;

    private void Awake() {
        gameController = GetComponent<GameController>();
    }

    private void Start() {
        width = (int) sliders[0].value;
        height = (int) sliders[1].value;
        sliders[0].onValueChanged.AddListener(value => UpdateWidthValue(value));
        sliders[1].onValueChanged.AddListener(value => UpdateHeightValue(value));
        ShowStartMenu();
    }

    public void UpdateWidthValue(float value) {
        width = (int) value;
        sliderValueTexts[0].text = width.ToString();
    }

    public void UpdateHeightValue(float value) {
        height = (int) value;
        sliderValueTexts[1].text = height.ToString();
    }

    public void StartGame() {
        gameController.InitializeNewGame(width, height);
        startMenuPanel.SetActive(false);
        gameGrid.SetActive(true);
        gameOverlay.SetActive(true);
        gameOverPanel.SetActive(false);
        StartGameTimer();
    }

    public void ShowStartMenu() {
        startMenuPanel.SetActive(true);
        gameGrid.SetActive(false);
        gameOverlay.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    public void ShowGameOverMenu(bool gameWon) {
        gameGrid.SetActive(false);
        gameOverlay.SetActive(false);
        AdjustGameOverText(gameWon);
        gameOverPanel.SetActive(true);
    }

    private void AdjustGameOverText(bool gameWon) {
        GameTimer gameTimer = gameOverlay.GetComponentInChildren<GameTimer>();
        TextMeshProUGUI gameOverText = gameOverPanel.transform.Find("GameOverInfo")
            .GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI gameTimerText = gameOverPanel.transform.Find("GameTimeInfo")
            .GetComponent<TextMeshProUGUI>();

        gameOverText.text = gameWon ? "You won!" : "Game over!";
        gameTimerText.text = "Time: " + gameTimer.GetGameTime();
    }

    public void UpdateMineCounter(int counter) {
        TextMeshProUGUI mineCounterText = gameOverlay.GetComponentInChildren<TextMeshProUGUI>();
        mineCounterText.text = this.mineCounterText + counter;
    }

    public void StartGameTimer() {
        GameTimer gameTimer = gameOverlay.GetComponentInChildren<GameTimer>();
        gameTimer.StartTimer();
    }

    public void StopGameTimer() {
        GameTimer gameTimer = gameOverlay.GetComponentInChildren<GameTimer>();
        gameTimer.StopTimer();
    }

}
