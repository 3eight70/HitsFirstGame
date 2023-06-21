using UnityEngine;
using TMPro;

public class Tile : MonoBehaviour
{
    public Color ProfitTileColor, DamageTileColor;
    public SpriteRenderer Renderer;
    public GameObject Hover;
    public GameObject Visited;
    public GameObject Available;

    public void Init(bool isProfitTile, int score)
    {
        TextMeshProUGUI tileScore = GetComponentInChildren<TextMeshProUGUI>();

        Debug.Log(tileScore);

        if (tileScore != null) {
            tileScore.text = score.ToString();

            // tileScore.color = isProfitTile ? ProfitTileColor : DamageTileColor;
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

    public void OnClick()
    {
        Visited.SetActive(true);
    }

    public void SetAvailable()
    {
        Available.SetActive(true);
    }
}
