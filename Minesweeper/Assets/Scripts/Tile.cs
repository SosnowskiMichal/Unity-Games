using UnityEngine;

public struct Tile {
    public TileType type;
    public Vector3Int position;
    public int number;
    public bool isRevealed;
    public bool isFlagged;
    public bool isQuestioned;
    public bool isExploded;
}

public enum TileType {
    Invalid,
    Empty,
    Mine,
    WrongMine,
    Number
}
