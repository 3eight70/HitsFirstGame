using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserPathManager
{
    private GridManager Grid;
    private List<Tile> UserPath;
    private Tile LastTile;
    public List<Tile> AvailableTiles { get; private set; }
    public int AccScore { get; private set; }

    public UserPathManager(GridManager gridManager)
    {
        Grid = gridManager;
        UserPath = new List<Tile>();
        AvailableTiles = new List<Tile>();
        AccScore = 0;
    }

    public void Init()
    {
        InitStartTile();
    }

    private void InitStartTile()
    {
        Tile startTile = Grid.GetStartTile();
        if (startTile != null)
        {
            AccScore += startTile.Score;
            startTile.SetStartPoint();
            UserPath.Add(startTile);
            LastTile = startTile;
        }

        UpdateAvailableTiles();
    }

    private void UpdateAvailableTiles()
    {
        foreach (var tile in AvailableTiles)
        {
            tile.UserPathLogic.UnsetAvailable();
        }

        List<Tile> neighbors = Grid.GetNeighbors(LastTile.Position);
        AvailableTiles = new List<Tile>();

        foreach (var tile in neighbors)
        {
            if (tile.UserPathLogic.IsVisited) continue;

            var newAccScore = (LastTile.UserPathLogic.AccScore + tile.Score);

            if (newAccScore > 0)
            {
                AccScore += tile.Score;
                AvailableTiles.Add(tile);
                tile.UserPathLogic.SetAvailable(newAccScore);
            }
            else
            {
                tile.UserPathLogic.SetErrorAvailable(newAccScore);
            }
        }
    }

    public void OnTileClick(Tile tile)
    {
        var index = AvailableTiles.FindIndex(availableTile => availableTile.Position == tile.Position);
        if (index < 0) return;

        tile.SetVisited();
        UserPath.Add(tile);
        LastTile = tile;
        AccScore += LastTile.Score;
        UpdateAvailableTiles();
    }
}
