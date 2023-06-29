using System;
using UnityEngine;
using TMPro;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color ProfitTileColor, DamageTileColor;
    [SerializeField] private SpriteRenderer Renderer;
    [SerializeField] private GameObject Hover;
    [SerializeField] private GameObject Visited;
    [SerializeField] private GameObject Available;
    [SerializeField] private GameObject ErrorAvailable;
    [SerializeField] private GameObject PartOfBestPath;
    private TextMeshPro ScoreText;
    private TextMeshPro AccScoreText;

    private UserPathManager UserPathManager;
    public Vector2 Position { get; private set; }
    public BestTileLogic BestPathLogic { get; private set; }
    public UserTileLogic UserPathLogic { get; private set; }
    public int Score { get; private set; }


    public void Start()
    {
        SetScoreTexts();
    }

    private void SetScoreTexts()
    {
        TextMeshPro[] texts = GetComponentsInChildren<TextMeshPro>();

        string layerName = "tiles";

        if (texts[0] != null)
        {
            ScoreText = texts[0];
            ScoreText.renderer.sortingLayerName = layerName;
            ScoreText.sortingOrder = 12;
        }
        if (texts[1] != null)
        {
            AccScoreText = texts[1];
            AccScoreText.renderer.sortingLayerName = layerName;
            AccScoreText.sortingOrder = 13;
        }
    }

    public void Init(UserPathManager userPathManager, Vector2 position, int score)
    {
        UserPathManager = userPathManager;
        Position = position;
        Score = score;
        BestPathLogic = new BestTileLogic(this, Position, Score);
        UserPathLogic = new UserTileLogic(this, Position);
    }

    public void Update()
    {
        WatchScoreText();
        WatchVisited();
        WatchAvailable();
        WatchErrorAvailable();
    }

    private void WatchScoreText()
    {
        var isProfitTile = Score > 0;

        if (ScoreText != null)
        {
            var sign = isProfitTile ? "+" : "";

            ScoreText.text = String.Format("{0}{1}", sign, Score.ToString());
            ScoreText.color = isProfitTile ? ProfitTileColor : DamageTileColor;
        }

        if (AccScoreText != null)
        {
            AccScoreText.text = UserPathLogic.AccScore != 0 && UserPathLogic.ShowAccScore ? UserPathLogic.AccScore.ToString() : "";
        }
    }

    private void WatchVisited()
    {
        if (UserPathLogic.IsVisited) Visited.SetActive(true);
        else Visited.SetActive(false);
    }

    private void WatchAvailable()
    {
        if (UserPathLogic.IsAvailable) Available.SetActive(true);
        else Available.SetActive(false);
    }

    private void WatchErrorAvailable()
    {
        if (UserPathLogic.IsErrorAvailable) ErrorAvailable.SetActive(true);
        else ErrorAvailable.SetActive(false);
    }

    public void OnMouseEnter()
    {
        Hover.SetActive(true);
    }

    public void OnMouseExit()
    {
        Hover.SetActive(false);
    }

    public void SetPartOfBestPath()
    {
        PartOfBestPath.SetActive(true);
    }

    public void OnMouseDown()
    {
        if (UserPathManager == null) return;
        UserPathManager.OnTileClick(this);
    }

    public void SetVisited()
    {
        UserPathLogic.SetVisited();
    }

    public void SetStartPoint()
    {
        int startScore = 15;

        UserPathLogic.AccScore = startScore;
        BestPathLogic.AccScore = startScore;
        SetScore(startScore);
        UserPathLogic.SetVisited();
    }

    public void SetScore(int newScore)
    {
        Score = newScore;
    }

    public void ResetAvailableUserLogic()
    {
        UserPathLogic.UnsetAvailable();
        UserPathLogic.HideAccScore();
    }

    public void ResetVisitUserLogic()
    {
        UserPathLogic.UnsetAvailable();
        UserPathLogic.UnsetVisited();
    }

    public void FullReset(int score)
    {
        BestPathLogic = new BestTileLogic(this, Position, Score);
        UserPathLogic = new UserTileLogic(this, Position);
        SetScore(score);
    }

    public bool IsUserVisited() => UserPathLogic.IsVisited;
    public int UserAccScore() => UserPathLogic.AccScore;
    public bool IsUserErrorAvailable() => UserPathLogic.IsErrorAvailable;
    public void SetUserAvailable(int accScore) => UserPathLogic.SetAvailable(accScore);
    public void SetUserErrorAvailable(int accScore) => UserPathLogic.SetErrorAvailable(accScore);
}
