using System;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int Width;
    public int Height;
    public Tile TilePrefab;
    public Transform Camera;

    void Start()
    {
        GenerateGrid();
        MoveCamera();
    }

    private void CreateTile(int x, int y)
    {
        var newTile = Instantiate(TilePrefab, new Vector3(x, y), Quaternion.identity);
        newTile.name = String.Format("Tile-{0}{1}", x, y);

        bool isOdd = (x + y) % 2 == 1;
        newTile.Init(isOdd, x);
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

    private void MoveCamera()
    {
        Camera.transform.position = new Vector3(Width / 2f - 0.5f, Height / 2f - 0.5f, -10);
    }
}
