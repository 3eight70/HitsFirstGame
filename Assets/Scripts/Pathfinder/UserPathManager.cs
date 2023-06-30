using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserPathManager
{
    private GridManager Grid;
    private List<Tile> UserPath;
    private Tile StartTile = new Tile();
    private Tile DestinationTile = new Tile();
    private Tile LastTile;
    public List<Tile> AvailableTiles { get; private set; }
    private bool IsOver = false;

    public UserPathManager(GridManager gridManager)
    {
        Grid = gridManager;
        UserPath = new List<Tile>();
        AvailableTiles = new List<Tile>();
    }

    public void Init(Tile startTile, Tile destinationTile)
    {
        DestinationTile = destinationTile;
        InitStartTile(startTile);
    }

    private void InitStartTile(Tile startTile)
    {
        StartTile = startTile;

        startTile.SetStartPoint();
        UserPath.Add(startTile);
        LastTile = startTile;

        UpdateAvailableTiles();
    }

    public void OnTileClick(Tile tile)
    {
        if (LastTile.Position == tile.Position && LastTile.Position != StartTile.Position)
        {
            RemoveLastPathTile();
            return;
        }

        if (!IsValidTile(tile)) return;

        SetNextTile(tile);
    }

    private bool IsValidTile(Tile tile)
    {
        var index = AvailableTiles.FindIndex(availableTile => availableTile.Position == tile.Position);
        return !(index < 0 || AvailableTiles[index].IsUserErrorAvailable());
    }

    private void SetNextTile(Tile tile)
    {
        UserPath.Add(tile);

        LastTile = tile;
        UpdateAvailableTiles();
        tile.SetVisited();
        tile.UserPathLogic.SetShowAccScore();

        if (LastTile.Position == DestinationTile.Position)
        {
            IsOver = true;
            Grid.UserWin(LastTile.UserAccScore());
        }
    }

    private void UpdateAvailableTiles()
    {
        ResetAvailableTiles();
        SetAvailableTiles();
    }

    private void SetAvailableTiles()
    {
        List<Tile> neighbors = Grid.GetNeighbors(LastTile.Position);
        AvailableTiles = new List<Tile>();

        foreach (var tile in neighbors)
        {
            if (tile.IsUserVisited())
            {
                tile.UserPathLogic.SetShowAccScore();
                continue;
            }

            var newAccScore = (LastTile.UserAccScore() + tile.Score);

            if (newAccScore >= 0)
            {
                tile.SetUserAvailable(newAccScore);
                tile.UserPathLogic.SetShowAccScore();
            }
            else
            {
                tile.SetUserErrorAvailable(newAccScore);
            }

            AvailableTiles.Add(tile);
        }
    }

    private void ResetAvailableTiles()
    {
        foreach (var tile in AvailableTiles)
        {
            tile.ResetAvailableUserLogic();
        }
    }

    private void RemoveLastPathTile()
    {
        LastTile.ResetVisitUserLogic();

        UserPath.RemoveAt(UserPath.Count - 1);
        LastTile = UserPath[UserPath.Count - 1];

        UpdateAvailableTiles();
    }
}
