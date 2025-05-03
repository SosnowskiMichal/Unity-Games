using UnityEngine;

public class UIController : MonoBehaviour {

    private void Start() {
        Time.timeScale = 1;
    }

    public void LoadMenu() {
        SceneController.Instance.LoadScene("Main Menu");
    }

    public void StartGame() {
        SceneController.Instance.LoadNextScene();
    }

    public void ChangeMusicVolume(float volume) {
        AudioController.Instance.ChangeMusicVolume(volume);
    }

    public void ToggleMusic() {
        AudioController.Instance.ToggleMusic();
    }

    public void QuitGame() {
        Application.Quit();
    }

}
