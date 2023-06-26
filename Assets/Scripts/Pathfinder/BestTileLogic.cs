using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BestTileLogic
{
    public Vector2 Position;
    public Tile ParentTile { get; private set; }
    public int F;
    public int G;
    public int H;
    public bool IsVisited { get; set; }
    public BestTileLogic PrevBestTileLogic;
    public int AccScore { get; set; }

    public BestTileLogic(Tile tile, Vector2 position, int score)
    {
        ParentTile = tile;
        Position = position;
        AccScore = score;

        IsVisited = false;

        F = 0;
        G = 0;
        H = 0;
    }
}