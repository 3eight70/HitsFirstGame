using System;
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

    void Start()
    {
        SetSize();
        GenerateGrid();
        MoveCamera();
    }

    private void SetSize()
    {
        Width = Screen.width / PxPerTile - 1;
        Height = Screen.height / PxPerTile - 2;
    }

    private void GenerateGrid()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                CreateTile(x, y);
            }
        }
    }

    private void CreateTile(int x, int y)
    {
        var newTile = Instantiate(TilePrefab, new Vector3(x, y), Quaternion.identity);
        newTile.name = String.Format("Tile-{0}{1}", x, y);

        bool isOdd = (x + y) % 2 == 1;
        newTile.Init(isOdd, x);

        Tiles.Add(new Vector2(x, y), newTile);
        newTile.SetGrid(this);
    }

    private void MoveCamera()
    {
        Camera.transform.position = new Vector3(Width / 2f - 0.5f, Height / 2f - 0.5f, -10);
    }

}
