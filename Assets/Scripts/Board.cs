
using UnityEngine;


public class Board : MonoBehaviour
{
    public GameObject cellPrefab; // Drag your cell prefab here
    public int boardSize = 8;

    void Start()
    {

        GenerateBoard();
    }

    void GenerateBoard()
    {
        for (int x = 0; x < boardSize; x++)
        {
            for (int y = 0; y < boardSize; y++)
            {
                // Instantiate a cell at each position
                GameObject cell = Instantiate(cellPrefab, new Vector2(x * (float)0.9791937 - (float)3.44, y * (float)0.975811 - (float)3.44), Quaternion.identity);
                cell.transform.parent = this.transform; // Set the cell's parent to the board

                // Optional: Give each cell a name in the format "Cell X Y" for easier debugging
                cell.name = "Cell " + x + " " + y;
            }
        }
    }

    ClickAndMove cam = new ClickAndMove();
    void Update()
    {
        cam.Action();
    }
}

class ClickAndMove
{
    State state;
    GameObject item;
    Vector2 offset;
    GameObject selectedCell;

    public ClickAndMove()
    {
        state = State.none;
        item = null;

    }



    public void Action()
    {
        switch (state)
        {
            case State.none:
                if (IsMouseButtonPressed())
                    pickup();
                break;

            case State.chosen:
                if (IsMouseButtonPressed())
                    move();

                break;
        }
    }

    bool IsMouseButtonPressed()
    {
        return Input.GetMouseButtonDown(0);
    }

    Vector2 GetClickPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    Transform GetCheckerAt(Vector2 position)
    {
        int layerMask = LayerMask.GetMask("Checker"); // Only interact with the Checkers layer
        Collider2D hitCollider = Physics2D.OverlapPoint(position, layerMask);

        if (hitCollider == null)
            return null;
        return hitCollider.transform;
    }

    Transform GetCellAt(Vector2 position)
    {
        int layerMask = LayerMask.GetMask("Cells"); // Only interact with the Cells layer
        Collider2D hitCollider = Physics2D.OverlapPoint(position, layerMask);

        if (hitCollider == null)
            return null;
        return hitCollider.transform;
    }

    void pickup()
    {
        Vector2 clickPosition = GetClickPosition();
        Transform clickedItem = GetCheckerAt(clickPosition);

        if (clickedItem == null || clickedItem.tag == "WhiteChecker")
            return;

        // Get the cell under the checker and change its color
        Transform clickedCell = GetCellAt(clickPosition);
        if (clickedCell != null)
        {
            selectedCell = clickedCell.gameObject;
            SpriteRenderer cellRenderer = selectedCell.GetComponent<SpriteRenderer>();
            if (cellRenderer != null)
            {
                cellRenderer.color = Color.yellow;
            }
        }

        state = State.chosen;
        item = clickedItem.gameObject;

        offset = (Vector2)clickedItem.position - clickPosition;
        Debug.Log("Picked " + item.name);
    }

    void move()
    {
        Vector2 clickPosition = GetClickPosition();

        // Check if there's a checker at the clicked position
        Transform clickedChecker = GetCheckerAt(clickPosition);
        Transform clickedCell = GetCellAt(clickPosition);

        if (clickedChecker != null && clickedChecker.tag == "Checker")
        {
            if (selectedCell != null)
            {
                SpriteRenderer cellRenderer = selectedCell.GetComponent<SpriteRenderer>();
                if (cellRenderer != null)
                {
                    // Change back to fully transparent color
                    cellRenderer.color = Color.clear;
                }
            }
            pickup();
            return;
        }
        if (clickedChecker != null)
        {
            Debug.Log("Cannot move to " + clickPosition + " because there's already a checker there.");
            return;
        }

        

        if (clickedCell == null)
            return;

        // Reset the color of the previously selected cell
        if (selectedCell != null)
        {
            SpriteRenderer cellRenderer = selectedCell.GetComponent<SpriteRenderer>();
            if (cellRenderer != null)
            {
                // Change back to fully transparent color
                cellRenderer.color = Color.clear;
            }
        }

        item.transform.position = clickedCell.position;

        state = State.none;
        item = null;

        Debug.Log("Moved to " + clickPosition);
    }



    enum State
    {
        none,
        chosen
    }
}