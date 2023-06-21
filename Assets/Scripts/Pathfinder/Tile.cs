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

    private bool IsVisited = false;
    private GridManager Grid;

    public void SetGrid(GridManager grid) {
        Grid = grid;
    }

    public void Init(bool isProfitTile, int score)
    {
        TextMeshPro tileScore = GetComponentInChildren<TextMeshPro>();

        if (tileScore != null)
        {
            var sign = score > 0 ? "+" : "-";
            tileScore.text = String.Format("{0}{1}", sign, score.ToString());

            tileScore.color = isProfitTile ? ProfitTileColor : DamageTileColor;
        }
    }

    public void OnMouseEnter()
    {
        Hover.SetActive(true);
    }

    public void OnMouseExit()
    {
        Hover.SetActive(false);
    }

    private void UpdateVisited()
    {
        if (IsVisited)
        {
            Visited.SetActive(false);
            IsVisited = false;
        }
        else
        {
            Visited.SetActive(true);
            IsVisited = true;
        }
    }

    public void OnMouseDown()
    {
        UpdateVisited();
    }

    public void SetAvailable()
    {
        Available.SetActive(true);
    }
}
