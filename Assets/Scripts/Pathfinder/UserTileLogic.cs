using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserTileLogic
{
    public Vector2 Position;
    public List<Tile> Neighbors;
    public BestTileLogic PrevUserTileLogic;
    public bool IsVisited { get; private set; }
    public bool IsErrorAvailable { get; private set; }
    public bool IsAvailable { get; private set; }
    public Tile ParentTile { get; private set; }
    public int AccScore { get; set; }

    public UserTileLogic(Tile parentTile, Vector2 position)
    {
        ParentTile = parentTile;
        Position = position;
        AccScore = 0;

        IsVisited = false;
        IsErrorAvailable = false;
        IsAvailable = false;
    }

    public void SetAvailable(int newAccScore)
    {
        AccScore = newAccScore;
        IsAvailable = true;
    }

    public void UnsetAvailable()
    {
        IsErrorAvailable = false;
        IsAvailable = false;
    }

    public void SetErrorAvailable(int newAccScore)
    {
        AccScore = newAccScore;
        IsErrorAvailable = true;
    }

    public void SetVisited()
    {
        IsVisited = true;
    }

    public void ChangeVisited()
    {
        if (!IsAvailable) return;
        IsVisited = !IsVisited;
    }
}
