using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardController : MonoBehaviour {

    public Tilemap tilemap;

    public TileBase tileHidden;
    public TileBase tileEmpty;
    public TileBase tileMine;
    public TileBase tileExploded;
    public TileBase tileWrongMine;
    public TileBase tileFlagged;
    public TileBase tileQuestioned;
    public TileBase[] tileNumbers;

    public void ClearGrid() {
        tilemap.ClearAllTiles();
    }

    public void Draw(Tile[,] gameState) {
        int gridWidth = gameState.GetLength(0);
        int gridHeight = gameState.GetLength(1);

        for (int x = 0; x < gridWidth; x++) {
            for (int y = 0; y < gridHeight; y++) {
                Tile tile = gameState[x, y];
                tilemap.SetTile(tile.position, GetTileType(tile));
            }
        }
    }

    private TileBase GetTileType(Tile tile) {
        if (tile.isRevealed) {
            return GetRevealedTileType(tile);
        }
        if (tile.isFlagged) {
            return tile.type == TileType.WrongMine ? tileWrongMine : tileFlagged;
        }
        if (tile.isQuestioned) {
            return tileQuestioned;
        }
        return tileHidden;
    }

    private TileBase GetRevealedTileType(Tile tile) {
        return tile.type switch {
            TileType.Empty => tileEmpty,
            TileType.Mine => tile.isExploded ? tileExploded : tileMine,
            TileType.Number => tileNumbers[tile.number - 1],
            _ => null
        };
    }

}
