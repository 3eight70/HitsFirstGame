using System;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private int PxPerTile = 70;
    public int Width;
    public int Height;
    public Tile TilePrefab;
    public Transform Camera;
    private Dictionary<Vector2, Tile> Tiles = new Dictionary<Vector2, Tile>();
    private UserPathManager UserManager;
    private Tile StartTile = new Tile();
    private Tile DestinationTile = new Tile();

    void Start()
    {
        UserManager = new UserPathManager(this);

        SetSize();
        InitGrid();
        MoveCamera();
        GenerateScores();
        UserManager.Init(StartTile, DestinationTile);

        int bestPathScore = ExecuteBestPathfinder();

        Debug.Log(bestPathScore);
    }

    private void SetSize()
    {
        Width = Screen.width / PxPerTile - 1;
        Height = Screen.height / PxPerTile - 2;
    }

    private void InitGrid()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                CreateTile(x, y);
            }
        }

        StartTile = Tiles[new Vector2(0, 0)];
        DestinationTile = Tiles[new Vector2(Width - 1, Height - 1)];
    }

    private void CreateTile(int x, int y)
    {
        var newTile = Instantiate(TilePrefab, new Vector3(x, y), Quaternion.identity);
        newTile.name = String.Format("Tile-{0}{1}", x, y);
        var position = new Vector2(x, y);

        newTile.Init(UserManager, position, -1);

        Tiles.Add(position, newTile);
    }

    private void MoveCamera()
    {
        Camera.transform.position = new Vector3(Width / 2f - 0.5f, Height / 2f - 0.5f, -10);
    }

    private void GenerateScores()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                int randomSign = UnityEngine.Random.Range(1, 3);
                int randomScore = 0;

                randomScore = randomSign == 1 ? UnityEngine.Random.Range(-10, -2) : UnityEngine.Random.Range(-1, 10);
                int randomDirection = UnityEngine.Random.Range(1, 3);

                Tile bottomTile = GetTileAtPosition(new Vector2(x, y + 1));
                Tile rightTile = GetTileAtPosition(new Vector2(x + 1, y));

                // bottom
                if (bottomTile != null && (randomDirection == 1 || rightTile == null))
                {
                    bottomTile.SetScore(randomScore);
                }
                else if (rightTile != null)
                {
                    rightTile.SetScore(randomScore);
                }

                var tile = GetTileAtPosition(new Vector2(x, y));
                tile.SetScore(randomScore);
            }
        }
    }

    public List<Tile> GetNeighbors(Vector2 position)
    {
        List<Tile> neighbors = new List<Tile>();
        var x = position.x;
        var y = position.y;

        var leftTile = GetTileAtPosition(new Vector2(x - 1, y));
        if (leftTile != null) neighbors.Add(leftTile);

        var rightTile = GetTileAtPosition(new Vector2(x + 1, y));
        if (rightTile != null) neighbors.Add(rightTile);

        var topTile = GetTileAtPosition(new Vector2(x, y - 1));
        if (topTile != null) neighbors.Add(topTile);

        var bottomTile = GetTileAtPosition(new Vector2(x, y + 1));
        if (bottomTile != null) neighbors.Add(bottomTile);

        return neighbors;
    }

    public Tile GetTileAtPosition(Vector2 position)
    {
        if (Tiles.TryGetValue(position, out var tile))
        {
            return tile;
        }

        return null;
    }

    private int ExecuteBestPathfinder()
    {
        Vector2 startPointPosition = new Vector2(0, 0);
        Vector2 endPointPosition = new Vector2(Width - 1, Height - 1);

        BestPathfinder finder = new BestPathfinder(this, startPointPosition, endPointPosition);
        var path = finder.GetBestPath();

        var accScore = 0;
        foreach (Tile tile in path)
        {
            accScore += tile.Score;
            tile.SetPartOfBestPath();
        }
        
        return accScore;
    }

    public void UserWin(int accScore)
    {
        Debug.Log(accScore);
        int bestPathScore = ExecuteBestPathfinder();

        Debug.Log(bestPathScore);
    }
}
