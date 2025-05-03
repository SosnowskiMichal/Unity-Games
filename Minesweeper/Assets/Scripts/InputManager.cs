using UnityEngine;

public class InputManager : MonoBehaviour {

    private GameController gameController;
    private MenuController menuController;
    private InputSystem_Actions inputActions;

    private void Awake() {
        gameController = GameObject.Find("ScriptContainer").GetComponent<GameController>();
        menuController = GameObject.Find("ScriptContainer").GetComponent<MenuController>();
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable() {
        inputActions.MyActionMap.LeftClick.performed += _ => HandleTileReveal();
        inputActions.MyActionMap.RightClick.performed += _ => HandleTileMark();
        inputActions.MyActionMap.RestartGame.performed += _ => RestartGame();
        inputActions.MyActionMap.BackToMenu.performed += _ => BackToMainMenu();
        inputActions.MyActionMap.Enable();
    }

    private void OnDisable() {
        inputActions.MyActionMap.LeftClick.performed -= _ => HandleTileReveal();
        inputActions.MyActionMap.RightClick.performed -= _ => HandleTileMark();
        inputActions.MyActionMap.RestartGame.performed -= _ => RestartGame();
        inputActions.MyActionMap.BackToMenu.performed -= _ => BackToMainMenu();
        inputActions.MyActionMap.Disable();
    }

    private void RestartGame() {
        if (!gameController.isGameOver) {
            gameController.StartNewGame();
            menuController.StartGameTimer();
        }
    }

    private void BackToMainMenu() {
        if (!gameController.isGameOver) {
            menuController.ShowStartMenu();
            menuController.StopGameTimer();
        }
    }

    private void HandleTileReveal() {
        Vector3 worldPosition = GetWorldPosition();
        gameController.RevealTile(worldPosition);
    }

    private void HandleTileMark() {
        Vector3 worldPosition = GetWorldPosition();
        gameController.MarkTile(worldPosition);
    }

    private Vector3 GetWorldPosition() {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

}
