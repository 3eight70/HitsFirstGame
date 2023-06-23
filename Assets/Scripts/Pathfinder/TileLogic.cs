using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileLogic
{
    public Vector2 Position;
    public int F;
    public int G;
    public int H;
    public List<Tile> Neighbors;
    public bool IsVisited { get; set; }
    public TileLogic PrevTileLogic;
    public int Score { get; private set; }
    public int AccScore { get; set; }

    public TileLogic(Vector2 position, int score)
    {
        Position = position;
        Score = score;
        AccScore = score;

        F = 0;
        G = 0;
        H = 0;
    }

    public void ChangeScore(int newScore)
    {
        Score = newScore;
    }
}
