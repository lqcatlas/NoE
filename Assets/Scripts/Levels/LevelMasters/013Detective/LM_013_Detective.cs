using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LM_013_Detective : LevelMasterBase
{
    enum InspectionResult { none = 0, accurate = 1, suspect = 2, exclude = 3 };
    [Header("Theme Additions")]
    public LMHub_013_Detective themeHub;

    private List<InspectionResult> ispResult;
    private List<Vector2Int> appointedCoord;
    private List<Vector2Int> murdererCoord;
    private List<int> murdererValue;

    private int RANDOMIZE_LVINDEX = 2;

    public override void GetObjectReferences(GameObject _themeHub)
    {
        base.GetObjectReferences(null);
        themeHub = _themeHub.GetComponent<LMHub_013_Detective>();
    }
    public override void InitTool()
    {
        base.InitTool();
        hub.toolMaster.toolIcon.sprite = themeHub.toolSprite;
    }
    public override void AddtionalInit_Theme(bool isRewind = false)
    {
        appointedCoord = new List<Vector2Int>(); //clear appointed
        //locate the murderers
        murdererCoord = new List<Vector2Int>();
        murdererValue = new List<int>();
        for (int i = 0; i < levelData.curBoard.cells.Count; i++)
        {
            if (levelData.curBoard.cells[i].coord.y == levelData.curBoard.boardSize.y - 1 && levelData.curBoard.cells[i].status > 0)
            {
                murdererCoord.Add(levelData.curBoard.cells[i].coord);
                murdererValue.Add(levelData.curBoard.cells[i].value);
            }
        }
        //init isp signs
        themeHub.bgHolder.transform.localScale = hub.boardMaster.cellHolder.localScale;
        themeHub.ispSigns = new List<GameObject>();
        ispResult = new List<InspectionResult>();
        float signXOffset = 0;
        List<float> signYOffsetPerY = new List<float>();
        for(int i = 0; i < levelData.initBoard.boardSize.y; i++)
        {
            signYOffsetPerY.Add(0);
        }
        for (int i = 0; i < hub.boardMaster.cells.Count; i++)
        {
            if (hub.boardMaster.cells[i].coord.x == levelData.initBoard.boardSize.x)
            {
                signXOffset = hub.boardMaster.cells[i].transform.position.x;
                signYOffsetPerY[hub.boardMaster.cells[i].coord.y] = hub.boardMaster.cells[i].transform.position.y;
            }
        }
        float cellSize = 2f;
        for (int i = 0; i < levelData.initBoard.boardSize.y; i++)
        {
            ispResult.Add(InspectionResult.none);
            GameObject obj = Instantiate(themeHub.ispTemplate, themeHub.bgHolder);
            obj.transform.position = new Vector3(signXOffset + cellSize * themeHub.bgHolder.transform.localScale.x, signYOffsetPerY[i], 0);
            obj.GetComponent<inspection_sign>().SetSign((int)ispResult[i]);
            themeHub.ispSigns.Add(obj);
        }
    }
    public override void HandlePlayerInput(Vector2Int coord)
    {
        if (coord.y == levelData.initBoard.boardSize.y - 1) //murder appointing 
        {
            appointedCoord.Add(coord);
        }
        else //row inspection
        {
            int inspectedRow = coord.y;
            InspectionResult result = InspectRowY(inspectedRow);
            ispResult[inspectedRow] = result;
        }
    }
    public override void AddtionalUpdate_Theme(Vector2Int coord)
    {
        //update inspection row
        //update murder appointment
    }
    public override bool CheckLoseCondition()
    {
        return base.CheckLoseCondition();
        //murderer appoint check
    }
    public override bool CheckWinCondition()
    {
        return base.CheckWinCondition();
        //murderer appoint check
    }
    public override void InitBoardData()
    {
        if(levelData.levelIndex >= RANDOMIZE_LVINDEX)
        {
            RandomizeDetectiveBoard(levelData.initBoard);
        }
        levelData.curBoard = new DataBoard(levelData.initBoard);
        levelData.previousBoard = null;
        levelData.previousBoards = new List<DataBoard>();
    }
    InspectionResult InspectRowY(int y)
    {
        InspectionResult result = InspectionResult.exclude;
        for (int i = 0;i < levelData.curBoard.cells.Count;i++)
        {
            if (levelData.curBoard.cells[i].coord.y == y)
            {
                if (murdererValue.Contains(levelData.curBoard.cells[i].value)) //ambiguous match
                {
                    result = InspectionResult.suspect;
                    for(int j = 0; j < murdererCoord.Count; j++)
                    {
                        if (murdererCoord[j].x == levelData.curBoard.cells[i].coord.x 
                            && murdererValue[j] == levelData.curBoard.cells[i].value)  //exact match
                        {
                            result = InspectionResult.accurate;
                        }
                    }
                }
            }
        }
        return result;
    }
    void RandomizeDetectiveBoard(DataBoard board)
    {
        //shuffle on numbers
        List<int> numberShift = Enumerable.Range(1, 9).ToList();
        numberShift.OrderBy(x => Guid.NewGuid());
        numberShift.Insert(0, 0);
        Debug.LogWarning("numbershift list: " + string.Join(", ", numberShift));

        int[,] valueInArray = new int[board.boardSize.x, board.boardSize.y];
        int[,] statusInArray = new int[board.boardSize.x, board.boardSize.y];
        for (int i = 0; i < board.cells.Count; i++)
        {
            if(board.cells[i].value >= 0 && board.cells[i].value <= 9)
            {
                board.cells[i].value = numberShift[board.cells[i].value]; //shift numbers
            }
            valueInArray[board.cells[i].coord.x, board.cells[i].coord.y] = board.cells[i].value; //create a 2D array at the same time
            statusInArray[board.cells[i].coord.x, board.cells[i].coord.y] = board.cells[i].status;
        }
        //shuffle on col
        for (int i = board.boardSize.x - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            if (i != j)
            {
                for (int k = 0; k < board.boardSize.y; k++)
                {
                    (valueInArray[k, i], valueInArray[k, j]) = (valueInArray[k, j], valueInArray[k, i]);
                    (statusInArray[k, i], statusInArray[k, j]) = (statusInArray[k, j], statusInArray[k, i]);
                }
            }
        }
        //shuffle on row but keep the last row stay untouched
        for (int i = board.boardSize.y - 2; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(1, i + 1);
            if (i != j)
            {
                for (int k = 0; k < board.boardSize.x; k++)
                {
                    (valueInArray[k, i], valueInArray[k, j]) = (valueInArray[k, j], valueInArray[k, i]);
                    (statusInArray[k, i], statusInArray[k, j]) = (statusInArray[k, j], statusInArray[k, i]);
                }
            }
        }
        //copy array back to data cells
        for (int i = 0; i < board.cells.Count; i++)
        {
            board.cells[i].value = valueInArray[board.cells[i].coord.x, board.cells[i].coord.y];
            board.cells[i].status = statusInArray[board.cells[i].coord.x, board.cells[i].coord.y];
        }
    }
}
