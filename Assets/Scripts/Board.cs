
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections;

public class Board : MonoBehaviour
{
    public GameObject cellPrefab; 
    public int boardSize = 8;
    public GameObject[,] cells;
    ClickAndMove cam;
    public GameObject gameOverPanel;

    void Start()
    {
        cam = new ClickAndMove(this);
        GenerateBoard();
    }

    void GenerateBoard()
    {
        cells = new GameObject[boardSize, boardSize];
        for (int x = 0; x < boardSize; x++)
        {
            for (int y = 0; y < boardSize; y++)
            {
                // Instantiate a cell at each position
                GameObject cell = Instantiate(cellPrefab, new Vector2(x * (float)0.9791937 - (float)3.43, y * (float)0.975811 - (float)3.43), Quaternion.identity);
                //GameObject cell = Instantiate(cellPrefab, new Vector2(x * (float)0.95 - (float)3.43, y * (float)0.95 - (float)3.43), Quaternion.identity);
                cell.transform.parent = this.transform; // Set the cell's parent to the board

                // Optional: Give each cell a name in the format "Cell X Y" for easier debugging
                cell.name = "Cell " + x + " " + y;

                cells[x, y] = cell;
            }
        }
    }

    
    void Update()
    {
        //cam.machine.UpdateBoard();
      
        
        cam.Action();
    }
}

public class ClickAndMove
{
    public State state;
    GameObject item;
    Vector2 offset;
    GameObject selectedCell;
    CheckerMoveCalculator moveCalculator;
    public VerySmartMachine machine;
    public int size = 8;
    public int repeatchecker = 0;
    public Vector3 targetPosition;
    public float speed = 1f;

    public ClickAndMove(Board board)
    {
        state = State.none;
        item = null;
        moveCalculator = new CheckerMoveCalculator(this);
        machine = new VerySmartMachine(board, this);

    }



    public void Action()
    {
        switch (state)
        {
            case State.none: // Состояние спокойствия (то есть когда мы ничего не делаем - ни пользователь, ни ИИшка)
                if (IsMouseButtonPressed())
                    pickup();
                break;

            case State.chosen: // Состояние когда пользователь выбрал шашку, тогда ждем следующее нажатие, чтобы передвинуть шашку
                if (IsMouseButtonPressed())
                    move();

                break;
            case State.machine: // Состояние, когда пора отдавать данные ИИшке для принятия решения
                                //Debug.Log("Computer turn");
                                //machine.ShowSituation();

                machine.MoveMaker();
                //StartCoroutine(machine.MoveMaker());

                break;
        }
    }

   


    public bool IsMouseButtonPressed()
    {
        return Input.GetMouseButtonDown(0);
    }

    public Vector2 GetClickPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public Transform GetCheckerAt(Vector2 position)
    {
        int layerMask = LayerMask.GetMask("Checker"); // Шашка из соответствующего слоя
        Collider2D hitCollider = Physics2D.OverlapPoint(position, layerMask);
        
        if (hitCollider == null)
            return null;
        return hitCollider.transform;
    }

    public Transform GetCellAt(Vector2 position)
    {
        int layerMask = LayerMask.GetMask("Cells"); // Клетка из соответствующего слоя
        Collider2D hitCollider = Physics2D.OverlapPoint(position, layerMask);

        if (hitCollider == null)
            return null;
        return hitCollider.transform;
    }

    public GameObject GetGOCellAt(Vector2 position)
    {
        int layerMask = LayerMask.GetMask("Cells"); // Клетка из соответствующего слоя
        Collider2D hitCollider = Physics2D.OverlapPoint(position, layerMask);

        if (hitCollider == null)
            return null;
        return hitCollider.gameObject;
    }

    public void pickup()
    {
        Vector2 clickPosition = GetClickPosition();
        Transform clickedItem = GetCheckerAt(clickPosition);

        //if (clickedItem == null || clickedItem.tag == "WhiteChecker")
        //return;
        if (clickedItem == null)
            return;
        
        
        Transform clickedCell = GetCellAt(clickPosition);

        if (!machine.HumanJumpPossibles(clickedCell.gameObject))
        {
            return;
        }

        if (clickedCell != null)
        {
            selectedCell = clickedCell.gameObject;
            SpriteRenderer cellRenderer = selectedCell.GetComponent<SpriteRenderer>();
            if (cellRenderer != null)
            {
                cellRenderer.color = Color.yellow;
                moveCalculator.CalculatePossibleMoves(clickedItem.gameObject);
                moveCalculator.HighlightPossibleMoves(clickedItem.gameObject);
            }
        }

        state = State.chosen;
        item = clickedItem.gameObject;

        offset = (Vector2)clickedItem.position - clickPosition;
        Debug.Log("Picked " + item.name);
    }

    public void move()
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
            moveCalculator.ClearPossibleMoves(item);
            pickup();
            return;
        }
        if (clickedCell == null || !moveCalculator.IsValidMove(item, clickedCell.gameObject) && item.transform.tag != "WhiteChecker")
            return;
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

        float diff = Math.Abs(clickedCell.position[0] - item.transform.position[0]);
        machine.UpdateBoard(item.transform.position, clickedCell.position, 1);
        item.transform.position = clickedCell.position;
        //item.transform.position = Vector2.Lerp(item.transform.position, clickedCell.position, speed);


        moveCalculator.RemoveJumpedOverChecker(clickedCell.gameObject);
        moveCalculator.ClearPossibleMoves(item);
        moveCalculator.CalculatePossibleMoves(item);

        if (machine.HumanHasJumpCells(clickedCell.gameObject)==false && diff > (float)1.5 || diff < (float)1.6){
            moveCalculator.RemoveJumpedOverChecker(clickedCell.gameObject);
            moveCalculator.ClearPossibleMoves(item);
            state = State.machine;
            //Debug.Log(diff);
            return;
        }
        state = State.none;

        moveCalculator.ClearPossibleMoves(item);
        item = null;
        Debug.Log("Moved to " + clickPosition);
    }



    public enum State
    {
        none,
        chosen,
        machine
    }
}


public class CheckerMoveCalculator
{
    //private Dictionary<GameObject, List<GameObject>> possibleMoves = new Dictionary<GameObject, List<GameObject>>();
    private Dictionary<GameObject, List<GameObject>> possibleMoves = new Dictionary<GameObject, List<GameObject>>();
    private Dictionary<GameObject, GameObject> jumpOverCheckers = new Dictionary<GameObject, GameObject>();
  
    private ClickAndMove clickAndMove;

    public CheckerMoveCalculator(ClickAndMove clickAndMove)
    {
        this.clickAndMove = clickAndMove;
    }

    public void RemoveJumpedOverChecker(GameObject cell)
    {
        if (jumpOverCheckers.ContainsKey(cell))
        {
            GameObject.Destroy(jumpOverCheckers[cell]);
            jumpOverCheckers.Remove(cell);
        }
    }

    public void CalculatePossibleMoves(GameObject checker)
    {
        Vector2 checkerPosition = checker.transform.position;

        List<GameObject> nearbyCells = new List<GameObject>();
        List<GameObject> jumpOverCells = new List<GameObject>();

        for (int dx = -1; dx <= 1; dx = dx + 2)
        {
            for (int dy = -1; dy <= 1; dy = dy + 2)
            {
                Vector2 nearbyPosition = new Vector2(checkerPosition.x + dx, checkerPosition.y + dy);
                Transform nearbyCell = this.clickAndMove.GetCellAt(nearbyPosition);
                if (nearbyCell != null && nearbyCell.gameObject != checker)
                {
                    // Check if there's an enemy checker in the cell and if the jump position is inside the board
                    Vector2 jumpPosition = new Vector2(checkerPosition.x + 2 * dx, checkerPosition.y + 2 * dy);
                    Transform checkerInCell = this.clickAndMove.GetCheckerAt(nearbyPosition);
                    Transform cellInJumpPosition = this.clickAndMove.GetCellAt(jumpPosition);
                    if (checkerInCell != null && checkerInCell.gameObject.tag != checker.tag && cellInJumpPosition != null)
                    {
                        if (this.clickAndMove.GetCheckerAt(jumpPosition) == null)
                        {
                            jumpOverCells.Add(cellInJumpPosition.gameObject);
                            jumpOverCheckers[cellInJumpPosition.gameObject] = checkerInCell.gameObject;
                        }
                    }
                    else
                    {
                        nearbyCells.Add(nearbyCell.gameObject);
                    }
                }
            }
        }

        // If there are jump over moves, add only them. Otherwise, add the nearby cells
        if (jumpOverCells.Count > 0)
        {
            possibleMoves[checker] = jumpOverCells;
        }
        else
        {
            possibleMoves[checker] = nearbyCells.Where(cell =>
                (checker.transform.position.y < cell.transform.position.y) && this.clickAndMove.GetCheckerAt(cell.transform.position) == null).ToList();
        }
    }


    public void HighlightPossibleMoves(GameObject checker)
    {
        foreach (GameObject cell in possibleMoves[checker])
        {
            SpriteRenderer cellRenderer = cell.GetComponent<SpriteRenderer>();
            if (cellRenderer != null)
            {
                cellRenderer.color = Color.yellow;
            }
        }
    }


    

    public void ClearPossibleMoves(GameObject checker)
    {
        if (possibleMoves.ContainsKey(checker))
        {
            foreach (GameObject cell in possibleMoves[checker])
            {
                SpriteRenderer cellRenderer = cell.GetComponent<SpriteRenderer>();
                if (cellRenderer != null)
                {
                    cellRenderer.color = Color.clear;
                }
            }
            possibleMoves.Remove(checker);
        }
    }

    public bool IsValidMove(GameObject checker, GameObject cell)
    {
        return possibleMoves.ContainsKey(checker) && possibleMoves[checker].Contains(cell);
    }
}

public class VerySmartMachine
{
    private Board board;
    private const int BoardSize = 8;
    private int[,] numeralBoard = new int[8, 8];
    private ClickAndMove clickandmove;
    

    private Dictionary<GameObject, GameObject> jumpOverMachineCheckers = new Dictionary<GameObject, GameObject>();

    public VerySmartMachine(Board board, ClickAndMove cam)
    {
        this.board = board;
        this.clickandmove = cam;
        BoardInit();
    }
    public void BoardInit()
    {
        int cnt = 0;
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                if (cnt == 12)
                {
                    return;
                }
                if ((x+y)%2 == 1)
                {
                    cnt += 1;
                    numeralBoard[x, y] = -1;
                    numeralBoard[7 - x, 7 - y] = 1;
                }
            }
        }
    }


    public bool HumanJumpPossibles(GameObject cell)
    {
        var name = cell.name;
        (int, int) ccords = ((int)(name[name.Length - 3] - '0'), (int)(name[name.Length - 1] - '0'));
        var cords = UnconvertCord(ccords);
        List<(int, int)> resultPossibles = new List<(int, int)>();
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                if (numeralBoard[x, y] == 1)
                {
                    for (int dx = -2; dx <= 2; dx += 2)
                    {
                        for (int dy = -2; dy <= 2; dy += 2)
                        {
                            (int, int) temppos = (x, y);
                            (int, int) nextpos = (x + dx, y + dy);
                            if (nextpos.Item1 >= 0 && nextpos.Item1 <= 7 && nextpos.Item2 >= 0 && nextpos.Item2 <= 7)
                            {
                                if (numeralBoard[(temppos.Item1+nextpos.Item1)/2, (temppos.Item2 + nextpos.Item2)/2] == -1 && numeralBoard[nextpos.Item1, nextpos.Item2] == 0)
                                {
                                    resultPossibles.Add(temppos);
                                }
                            }
                        }
                    }
                }
            }
        }

        if (resultPossibles.Count == 0 || resultPossibles.Count > 0 && resultPossibles.Contains(cords))
        {
            return true;
        }
        return false;
    }


    public bool HumanHasJumpCells(GameObject cell)
    {
        var name = cell.name;
        (int, int) ccords = ((int)(name[name.Length - 3] - '0'), (int)(name[name.Length - 1] - '0'));
        var cords = UnconvertCord(ccords);
        for (int dx = -2; dx <= 2; dx += 2)
        {
            for (int dy = -2; dy <= 2; dy += 2)
            {
                (int, int) rescords = (cords.Item1 + dx, cords.Item2 + dy);
                if (rescords.Item1 >= 0 && rescords.Item1 <= 7  && rescords.Item2 >= 0 && rescords.Item2 <= 7)
                {
                    if (numeralBoard[(cords.Item1 + rescords.Item1)/2,(cords.Item2 + rescords.Item2)/2] == -1 && numeralBoard[rescords.Item1, rescords.Item2] == 0)
                        return true;
                }
            }
        }
        return false;

    }
    /*public void BoardInit()
    {
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                //Transform ischecker = clickandmove.GetCheckerAt(board.cells[x, y].transform.position);
                Transform isBlack = GetBlackChecker(board.cells[x, y].transform.position);
                Transform isWhite = GetWhiteChecker(board.cells[x, y].transform.position);
                //Transform ischecker = currentcheckers[x, y];
                //int saved_x = x;
                //x = 8 - y- 1;
                //y = saved_x;
                //y = 8 - saved_x - 1;
                int new_x = 8 - y - 1;
                int new_y = x;
               
                if (isBlack != null)
                {
                    numeralBoard[new_x, new_y] = 1;
                }
                else if (isWhite != null)
                {
                    numeralBoard[new_x, new_y] = -1;
                }
                else
                {
                    numeralBoard[new_x, new_y] = 0;
                }
            }
        }
    }*/

    
    public void UpdateBoard(Vector2 startpos, Vector2 targetpos, int isuser)
    {
        GameObject currentcell = clickandmove.GetGOCellAt(startpos);
        GameObject targetcell = clickandmove.GetGOCellAt(targetpos);
        string curname = currentcell.name;
        string targname = targetcell.name;
        (int, int) cords1 = UnconvertCord(((int)(curname[curname.Length - 3]-'0'), (int)(curname[curname.Length - 1] - '0')));
        (int, int) cords2 = UnconvertCord(((int)(targname[targname.Length - 3] - '0'), (int)(targname[targname.Length - 1] - '0')));

        int diffx = cords2.Item1 - cords1.Item1;
        int diffy = cords2.Item2 - cords2.Item1;

        numeralBoard[cords1.Item1, cords1.Item2] = 0;
        numeralBoard[cords2.Item1, cords2.Item2] = isuser;

        if (isuser == 1 && Math.Abs(diffx) > 1)
        {
            numeralBoard[(cords1.Item1 + cords2.Item1) / 2, (cords1.Item2 + cords2.Item2) / 2] = 0;
        }


        /*for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                //Transform ischecker = clickandmove.GetCheckerAt(board.cells[x, y].transform.position);
                Transform isBlack = GetBlackChecker(board.cells[x, y].transform.position);
                Transform isWhite = GetWhiteChecker(board.cells[x, y].transform.position);
                //Transform ischecker = currentcheckers[x, y];
                //int saved_x = x;
                //x = 8 - y- 1;
                //y = saved_x;
                //y = 8 - saved_x - 1;
                int new_x = 8 - y - 1;
                int new_y = x;
                /*if (ischecker != null)
                {
                    if (ischecker.tag == "Checker")
                    {
                        numeralBoard[new_x, new_y] = 1;
                    }
                    else
                    {
                        numeralBoard[new_x, new_y] = -1;
                    }
                }
                else
                {
                    numeralBoard[new_x, new_y] = 0;
                }
                if (isBlack != null)
                {
                    numeralBoard[new_x, new_y] = 1;
                }
                else if (isWhite != null)
                {
                    numeralBoard[new_x, new_y] = -1;
                }
                else
                {
                    numeralBoard[new_x, new_y] = 0;
                }
            }
        }*/

            }



            public void ShowSituation()
    {

        string boardString = "";
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                boardString += numeralBoard[x, y] + " ";
            }
            boardString += "\n";
        }
        Debug.Log(boardString);
    }

    public bool CheckPosition((int, int) position)
    {
        return position.Item1 >= 0 && position.Item1 < 8 && position.Item2 >= 0 && position.Item2 < 8;
    }

    public List<(int, int)> PossibleMoves(int[,] board, (int, int) currentPosition, int isUser)
    {
        List<(int, int)> moves = new List<(int, int)> { (-1, 1), (-1, -1), (-2, 2), (-2, -2), (2, -2), (2, 2) };
        List<(int, int)> resultPossibles = new List<(int, int)>();
        List<int> indexesMemory = new List<int>();
        int[,] new_board = (int[,])board.Clone(); 
        if (isUser == 0)
        {
            return new List<(int, int)>();
        }
        int flag = 0;
        for (int ind = 0; ind < moves.Count; ind++)
        {
            var move = moves[ind];

            //if ((ind >= 2) && ((indexesMemory.Contains(0)) || (indexesMemory.Contains(1))))
            //{
                //continue;
            //}
            var nextMove = (currentPosition.Item1 + move.Item1 * isUser, currentPosition.Item2 + move.Item2 * isUser);
            
            if (CheckPosition(nextMove) && new_board[nextMove.Item1, nextMove.Item2] == 0)
            {
                
                if (Math.Abs(nextMove.Item1 - currentPosition.Item1) == 2 && Math.Abs(nextMove.Item2 - currentPosition.Item2) == 2)
                {
                    if (new_board[(currentPosition.Item1 + nextMove.Item1) / 2, (currentPosition.Item2 + nextMove.Item2) / 2] == isUser ||
                        new_board[(currentPosition.Item1 + nextMove.Item1) / 2, (currentPosition.Item2 + nextMove.Item2) / 2] == 0)
                    {
                        continue;
                    }
                    flag = 1;
                }
                resultPossibles.Add(nextMove);
                indexesMemory.Add(ind);
                //Debug.Log(currentPosition + " " + nextMove);
            }
        }
        if (flag == 1)
        {
            return resultPossibles.Where(obj => (Math.Abs(obj.Item1 - currentPosition.Item1) == 2)).ToList();
        }
                
        return resultPossibles;
    }

    public List<(int, int)> GetPairPosesOfSide(int[,] board, int side)
    {
        List<(int, int)> poses = new List<(int, int)>();
        int GLob = 0;
        List<(int, int)> MoreGoodposes = new List<(int, int)>();
        for (int x = 0; x<8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                if (board[x, y] == side)
                {
                    poses.Add((x, y));
                    List<(int, int)> posmoves = PossibleMoves(board, (x, y), side);
                    
                    foreach (var element in posmoves)
                    {
                        if (Math.Abs(element.Item1 - x) == 2)
                        {
                            //Debug.Log("Good");
                            GLob = 1;
                            MoreGoodposes.Add((x, y));
                            break;
                        }
                    }
                }
            }
        }
        if (GLob == 1)
        {
            return MoreGoodposes;
        }
        
        return poses;
    }



    public List<(int, int)> MachineCheckers()
    {
        List<(int, int)> selfCheckers = new List<(int, int)>();
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                var cell = (x, y);
                Debug.Log(cell);
                if (numeralBoard[x, y] == -1)
                {

                    selfCheckers.Add(cell);
                }
            }
        }
        return selfCheckers;
    }

    public (int, int) ConvertToCellCord((int, int) pair)
    {
        return (pair.Item2, 7 - pair.Item1);
    }

    public (int, int) UnconvertCord((int, int) pair)
    {
        return (7 - pair.Item2, pair.Item1);
    }

    public Transform GetWhiteChecker(Vector2 position)
    {
        int layerMask = LayerMask.GetMask("Checker"); // Only interact with the Checkers layer
        Collider2D hitCollider = Physics2D.OverlapPoint(position, layerMask);

        if (hitCollider == null || hitCollider.transform.tag == "Checker")
            return null;
        return hitCollider.transform;
    }
    public Transform GetBlackChecker(Vector2 position)
    {
        int layerMask = LayerMask.GetMask("Checker"); // Only interact with the Checkers layer
        Collider2D hitCollider = Physics2D.OverlapPoint(position, layerMask);

        if (hitCollider == null || hitCollider.transform.tag == "WhiteChecker")
            return null;
        return hitCollider.transform;
    }

    public Transform ModifiedGetter(Vector2 position)
    {
        RaycastHit2D[] figures = Physics2D.RaycastAll(position, position, 0.5f);

        if (figures.Length == 0)
            return null;
        Debug.Log($"При объекте {figures[0].transform} количество {figures.Length}");
        return figures[0].transform;
    }

   

    public void MoveMaker()
    {
        Physics2D.SyncTransforms();
        /*float start = 0;
        while (start < 15)
        {
            start += Time.deltaTime;
        }*/

        //gameOverPanel.setActive(true);
        //UpdateBoard();
        //ShowSituation();
        var pair = GeniusMove(numeralBoard, 6, -1);
        //Debug.Log(pair[0] + " " + pair[1]);
        var cellpair = ConvertToCellCord(pair[0]);
        Transform pickedChecker = GetWhiteChecker(this.board.cells[cellpair.Item1, cellpair.Item2].transform.position);
        //Transform pickedChecker = ModifiedGetter(this.board.cells[cellpair.Item1, cellpair.Item2].transform.position);


        var move = pair[1];
        var cellmove = ConvertToCellCord(move);

       
        Transform targetcell = board.cells[cellmove.Item1, cellmove.Item2].transform;

        int flag = 0;
        if (Math.Abs(pair[1].Item1 - pair[0].Item1) == 2 && Math.Abs(pair[1].Item2 - pair[0].Item2) == 2)
        {
            flag = 1;
            var oppcords = ((pair[0].Item1 + pair[1].Item1) / 2, (pair[0].Item2 + pair[1].Item2) / 2);
            var oppospair = ConvertToCellCord(oppcords);
            //Debug.Log(pair[0]);
            //Debug.Log(pair[1]);
            
            //Transform opposChecker = null;
            Transform opposChecker = GetBlackChecker(board.cells[oppospair.Item1, oppospair.Item2].transform.position);
            
            //Debug.Log(opposChecker);
            GameObject.Destroy(opposChecker.gameObject);
            numeralBoard[oppcords.Item1, oppcords.Item2] = 0;
            
        }
        else
        {
            Debug.Log($"Daaamn {pair[0]} {pair[1]}");
            if (clickandmove.repeatchecker == 1)
            {
                clickandmove.state = ClickAndMove.State.none;
                clickandmove.repeatchecker = 0;
                clickandmove.Action();
                return;

            }
        }
        Debug.Log($"Начальная клетка: {pair[0]}");
        Debug.Log($"Конечная клетка: {pair[1]}");
        Debug.Log($"Взятая шашка: {pickedChecker}");
        Debug.Log($"Целевая клетка: {targetcell}");
        

        
        UpdateBoard(pickedChecker.position, targetcell.position, -1);
        pickedChecker.position = targetcell.position;
        
        




        if (flag == 1)
        {
            clickandmove.repeatchecker = 1;
            MoveMaker();
        }
        clickandmove.repeatchecker = 0;
        clickandmove.state = ClickAndMove.State.none;
        
    }

    public int SituationController(int[,] board, int side_of_attacker)
    {
        int users = 0;
        int enemy = 0;
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                if (board[x,y] == side_of_attacker)
                    users++;
                if (board[x,y] == -1 * side_of_attacker)
                    enemy++;
            }
        }
        return users - enemy;
    }


    public string movesshower(List<(int, int)> moves)
    {
        string res = "";
        foreach (var el in moves)
        {
            res += el.ToString() + " ";
        }
        return res;
    }

    public int[,] process_move(int[,] board, (int, int) currentposition, (int, int) movetarget, int is_user)
    {
        int i = currentposition.Item1;
        int j = currentposition.Item2;
        int[,] new_board = (int[,])board.Clone();
        new_board[i, j] = 0;
        new_board[movetarget.Item1, movetarget.Item2] = is_user;

        //if (movetarget.Item1 == -2 && movetarget.Item2 == 2 || movetarget.Item1 == -2 && movetarget.Item2 == -2)
        //{
        //board[movetarget.Item1 - is_user, movetarget.Item2 - is_user] = 0;
        //}

        if (Math.Abs(movetarget.Item1 - i) == 2 && Math.Abs(movetarget.Item2 - j) == 2)
        {
            new_board[(i + movetarget.Item1) / 2, (j + movetarget.Item2) / 2] = 0;
        }
        return new_board;
    }

    public bool bad_cell(int[,] board, (int, int) currentposition, int is_user)
    {
        return PossibleMoves(board, currentposition, is_user).Count == 0;
    }

    public float lose_condition(int[,] board)
    {
        int users = 0;
        int enemy = 0;
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                if (board[x,y] == 1 && bad_cell(board, (x, y), 1)==false)
                    users++;
                if (board[x,y] == -1 && bad_cell(board, (x,y), -1)==false)
                    enemy++;
            }
        }
        if (users == 0 || enemy == 0)
            return 1;
        return 0;
    }


    public float minimax_algo(int[,] board, int depth, int maximizing_side)
    {
        if (depth == 0 || lose_condition(board) == 1)
        {
            //return lose_condition(board);
           
            return SituationController(board, -1);
        }

        if (maximizing_side == -1)
        {
            float maxscore = float.NegativeInfinity;
            foreach (var pos in GetPairPosesOfSide(board, maximizing_side))
            {
                List<(int, int)> posmoves = PossibleMoves(board, pos, maximizing_side);
                //Debug.Log($"{pos} {movesshower(posmoves)}");
                foreach (var move in posmoves)
                {
                    int[,] updatedboard = process_move(board, pos, move, maximizing_side);
                    float eval = minimax_algo(updatedboard, depth - 1, 1);
                    //Debug.Log(eval);
                    maxscore = Math.Max(maxscore, eval);
                }
                
            }
            return maxscore;
        }
        else
        {
            float minscore = float.PositiveInfinity;
            foreach (var pos in GetPairPosesOfSide(board, maximizing_side))
            {
                List<(int, int)> posmoves = PossibleMoves(board, pos, maximizing_side);
                foreach (var move in posmoves)
                {
                    int[,] updatedboard = process_move(board, pos, move, maximizing_side);
                    float eval = minimax_algo(updatedboard, depth - 1, -1);
                    //Debug.Log("��� " + pos + " - " + move + " ���� " + eval);
                    minscore = Math.Min(minscore, eval);
                }

            }
            return minscore;
        }
    }

    public List<(int, int)> GeniusMove(int[,] board, int depth, int maximizing_side)
    {
        List<(int, int)> geniusmove = new List<(int, int)>();
        
        if (maximizing_side == -1)
        {
            float bestMoveEv = float.NegativeInfinity;
            foreach (var pos in GetPairPosesOfSide(board, maximizing_side))
            {
                
                List<(int, int)> posmoves = PossibleMoves(board, pos, maximizing_side);
                foreach (var move in posmoves)
                {
                    //ShowSituation();
                    //Debug.Log(GetPairPosesOfSide(board, maximizing_side).Count);
                    //Debug.Log(pos + ": " + move);
                    int[,] updatedboard = process_move(board, pos, move, maximizing_side);
                    int[,] copied = (int[,])updatedboard;
                    //float eval = minimax_algo(updatedboard, depth - 1, 1);
                    float eval = minimax_algo(copied, depth - 1, 1);
                    //Debug.Log(eval);
                    if (eval > bestMoveEv)
                    {
                        bestMoveEv = eval;
                        geniusmove.Add(pos);
                        geniusmove.Add(move);
                    }
                }

            }
        }
        else
        {
            float bestMoveEv = float.PositiveInfinity;
            foreach (var pos in GetPairPosesOfSide(board, maximizing_side))
            {
                List<(int, int)> posmoves = PossibleMoves(board, pos, maximizing_side);
                foreach (var move in posmoves)
                {
                    //Debug.Log(pos + ": " + move);
                    int[,] updatedboard = process_move(board, pos, move, maximizing_side);
                    int[,] copied = (int[,])updatedboard;
                    //float eval = minimax_algo(updatedboard, depth - 1, -1);
                    float eval = minimax_algo(copied, depth - 1, -1);
                    //Debug.Log(eval);
                    if (eval < bestMoveEv)
                    {
                        bestMoveEv = eval;
                        geniusmove.Add(pos);
                        geniusmove.Add(move);
                    }
                }

            }
        }
        return geniusmove;
    }






}