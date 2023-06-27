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
    private TextMeshPro ScoreText;
    private TextMeshPro AccScoreText;

    private UserPathManager UserPathManager;
    public Vector2 Position { get; private set; }
    public BestTileLogic BestPathLogic { get; private set; }
    public UserTileLogic UserPathLogic { get; private set; }
    public int Score { get; private set; }

    public void Start()
    {
        TextMeshPro[] texts = GetComponentsInChildren<TextMeshPro>();

        if (texts[0] != null) ScoreText = texts[0];
        if (texts[1] != null) AccScoreText = texts[1];
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
            AccScoreText.text = UserPathLogic.AccScore != 0 ? UserPathLogic.AccScore.ToString() : "";
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
        UserPathLogic.AccScore = 10;
        SetScore(10);
        UserPathLogic.SetVisited();
    }

    public void SetScore(int newScore)
    {
        Score = newScore;
    }

    public void ResetAvailableUserLogic()
    {
        UserPathLogic.UnsetAvailable();
        UserPathLogic.AccScore = 0;
    }

    public void ResetVisitUserLogic()
    {
        UserPathLogic.UnsetAvailable();
        UserPathLogic.UnsetVisited();
    }

    public bool IsUserVisited() => UserPathLogic.IsVisited;
    public int UserAccScore() => UserPathLogic.AccScore;
    public bool IsUserErrorAvailable() => UserPathLogic.IsErrorAvailable;
    public void SetUserAvailable(int accScore) => UserPathLogic.SetAvailable(accScore);
    public void SetUserErrorAvailable(int accScore) => UserPathLogic.SetErrorAvailable(accScore);
}
