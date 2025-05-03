using System.Threading.Tasks;
using UnityEngine;

public class GameController : MonoBehaviour {

    private readonly int percentageOfMines = 15;
    private int gridWidth;
    private int gridHeight;

    public bool isGameOver { get; private set; }
    private Tile[,] gameState;
    private int minesLeft;

    private BoardController boardController;
    private MenuController menuController;

    private void Awake() {
        boardController = GetComponent<BoardController>();
        menuController = GetComponent<MenuController>();
    }

    public void InitializeNewGame(int gridWidth, int gridHeight) {
        this.gridWidth = gridWidth;
        this.gridHeight = gridHeight;
        StartNewGame();
    }

    public void StartNewGame() {
        boardController.ClearGrid();
        gameState = new Tile[gridWidth, gridHeight];
        isGameOver = false;
        GenerateTiles();
        GenerateMines();
        GenerateNumbers();
        MoveCamera();
        boardController.Draw(gameState);
    }

    private void GenerateTiles() {
        for (int x = 0; x < gridWidth; x++) {
            for (int y = 0; y < gridHeight; y++) {
                Tile tile = new() {
                    position = new Vector3Int(x, y, 0),
                    type = TileType.Empty
                };
                gameState[x, y] = tile;
            }
        }
    }

    private void GenerateMines() {
        int numberOfMines = (int) (gridWidth * gridHeight * percentageOfMines / 100f);
        int placedMines = 0;

        while (placedMines < numberOfMines) {
            int x = Random.Range(0, gridWidth);
            int y = Random.Range(0, gridHeight);
            if (gameState[x, y].type != TileType.Mine) {
                gameState[x, y].type = TileType.Mine;
                placedMines++;
            }
        }

        minesLeft = numberOfMines;
        menuController.UpdateMineCounter(minesLeft);
    }

    private void GenerateNumbers() {
        for (int x = 0; x < gridWidth; x++) {
            for (int y = 0; y < gridHeight; y++) {
                Tile tile = gameState[x, y];
                if (tile.type != TileType.Mine) {
                    int count = CountAdjacentMines(x, y);
                    if (count > 0) {
                        tile.type = TileType.Number;
                        tile.number = count;
                    }
                    gameState[x, y] = tile;
                }
            }
        }
    }

    private int CountAdjacentMines(int x, int y) {
        int count = 0;
        for (int i = -1; i <= 1; i++) {
            for (int j = -1; j <= 1; j++) {
                int tileX = x + i;
                int tileY = y + j;
                if (GetTile(tileX, tileY).type == TileType.Mine) {
                    count++;
                }
            }
        }
        return count;
    }

    private void MoveCamera() {
        Camera.main.transform.position = new Vector3(gridWidth / 2f, gridHeight / 2f, -10f);
    }

    public void RevealTile(Vector3 worldPosition) {
        if (isGameOver) return;
        Vector3Int tilePosition = boardController.tilemap.WorldToCell(worldPosition);
        Tile tile = GetTile(tilePosition.x, tilePosition.y);

        if (tile.type == TileType.Invalid
            || tile.isRevealed
            || tile.isFlagged
            || tile.isQuestioned) {
            return;
        }

        switch (tile.type) {
            case TileType.Empty:
                RevealEmptyTiles(tile);
                CheckWin();
                break;
            case TileType.Mine:
                ExplodeMine(tile);
                break;
            default:
                tile.isRevealed = true;
                gameState[tilePosition.x, tilePosition.y] = tile;
                CheckWin();
                break;
        }

        boardController.Draw(gameState);
    }

    private void RevealEmptyTiles(Tile tile) {
        if (tile.isRevealed) return;
        if (tile.type == TileType.Mine || tile.type == TileType.Invalid) return;

        if (!tile.isFlagged) {
            tile.isRevealed = true;
            gameState[tile.position.x, tile.position.y] = tile;
        }

        if (tile.type == TileType.Empty) {
            for (int i = -1; i <= 1; i++) {
                for (int j = -1; j <= 1; j++) {
                    if (i == 0 && j == 0) continue;
                    Tile adjacentTile = GetTile(tile.position.x + i, tile.position.y + j);
                    RevealEmptyTiles(adjacentTile);
                }
            }
        }
    }

    public void MarkTile(Vector3 worldPosition) {
        if (isGameOver) return;
        Vector3Int tilePosition = boardController.tilemap.WorldToCell(worldPosition);
        Tile tile = GetTile(tilePosition.x, tilePosition.y);

        if (tile.type == TileType.Invalid || tile.isRevealed) {
            return;
        }

        if (!tile.isFlagged && !tile.isQuestioned) {
            tile.isFlagged = true;
            minesLeft--;
        }
        else if (tile.isFlagged) {
            tile.isFlagged = false;
            tile.isQuestioned = true;
            minesLeft++;
        }
        else {
            tile.isQuestioned = false;
        }

        menuController.UpdateMineCounter(minesLeft);
        gameState[tilePosition.x, tilePosition.y] = tile;
        boardController.Draw(gameState);
    }

    private void ExplodeMine(Tile tile) {
        isGameOver = true;
        tile.isRevealed = true;
        tile.isExploded = true;
        gameState[tile.position.x, tile.position.y] = tile;
        GameLost();
    }

    private void RevealAllMines() {
        for (int x = 0; x < gridWidth; x++) {
            for (int y = 0; y < gridHeight; y++) {
                Tile tile = gameState[x, y];
                if (tile.type == TileType.Mine) {
                    if (!tile.isFlagged) {
                        tile.isRevealed = true;
                        gameState[x, y] = tile;
                    }
                } else if (tile.isFlagged && tile.type != TileType.Mine) {
                    tile.type = TileType.WrongMine;
                    gameState[x, y] = tile;
                }
            }
        }
    }

    private void CheckWin() {
        for (int x = 0; x < gridWidth; x++) {
            for (int y = 0; y < gridHeight; y++) {
                Tile tile = gameState[x, y];
                if (tile.type != TileType.Mine && !tile.isRevealed) {
                    return;
                }
            }
        }
        GameWon();
    }

    private async void GameWon() {
        menuController.StopGameTimer();
        isGameOver = true;
        FlagAllMines();
        await Task.Delay(1000);
        menuController.ShowGameOverMenu(true);
    }

    private async void GameLost() {
        menuController.StopGameTimer();
        isGameOver = true;
        RevealAllMines();
        boardController.Draw(gameState);
        await Task.Delay(1000);
        menuController.ShowGameOverMenu(false);
    }

    private void FlagAllMines() {
        for (int x = 0; x < gridWidth; x++) {
            for (int y = 0; y < gridHeight; y++) {
                Tile tile = gameState[x, y];
                if (tile.type == TileType.Mine) {
                    tile.isFlagged = true;
                    gameState[x, y] = tile;
                }
            }
        }
    }

    private Tile GetTile(int x, int y) {
        if (x >= 0 && x < gridWidth && y >= 0 && y < gridHeight) {
            return gameState[x, y];
        }
        return new Tile();
    }

}
