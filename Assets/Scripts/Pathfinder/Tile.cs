using System;
using UnityEngine;
using TMPro;

public class Tile : MonoBehaviour
{
    public Color ProfitTileColor, DamageTileColor;
    public SpriteRenderer Renderer;
    public GameObject Hover;
    public GameObject Visited;
    public GameObject Available;
    public GameObject ErrorAvailable;
    public GameObject PartOfBestPath;

    private bool IsVisited = false;
    private bool IsErrorAvailable = false;
    private bool IsAvailable = false;
    private GridManager Grid;
    public Vector2 Position { get; private set; }
    public BestTileLogic Logic { get; private set; }

    public void Init(int score, Vector2 position)
    {
        Position = position;
        Logic = new BestTileLogic(Position, score);
    }

    public void Update()
    {
        WatchScoreText();
        WatchIsVisited();
        WatchIsAvailable();
    }

    private void WatchScoreText()
    {
        int score = Logic.Score;
        var isProfitTile = score > 0;
        TextMeshPro tileScoreText = GetComponentInChildren<TextMeshPro>();

        if (tileScoreText != null)
        {
            var sign = isProfitTile ? "+" : "";

            tileScoreText.text = String.Format("{0}{1}", sign, score.ToString());
            tileScoreText.color = isProfitTile ? ProfitTileColor : DamageTileColor;
        }
    }

    private void WatchIsVisited()
    {
        if (IsVisited) Visited.SetActive(true);
        else Visited.SetActive(false);
    }

    private void WatchIsAvailable()
    {
        // if () 
        // if (IsVisited) Visited.SetActive(true);
        // else Visited.SetActive(false);
    }

    public void OnMouseEnter()
    {
        Hover.SetActive(true);
    }

    public void OnMouseExit()
    {
        Hover.SetActive(false);
    }

    public void UpdateVisited()
    {
        IsVisited = IsVisited ? false : true;
    }

    public void SetPartOfBestPath()
    {
        PartOfBestPath.SetActive(true);
    }

    public void OnMouseDown()
    {
        UpdateVisited();
    }

    public void SetAvailable()
    {
        Available.SetActive(true);
    }

    public void SetGrid(GridManager grid)
    {
        Grid = grid;
    }

    public void SetStartPoint()
    {
        SetScore(10);
        UpdateVisited();
    }

    public void SetScore(int newScore)
    {
        Logic.ChangeScore(newScore);
    }

}
