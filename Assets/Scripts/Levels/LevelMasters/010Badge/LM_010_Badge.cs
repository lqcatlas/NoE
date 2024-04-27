using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LM_010_Badge : LevelMasterBase
{
    enum CellStatus { hidden = 0, locked = 1, normal = 2, important = 3, found = 4, wrong = 5};
    
    [Header("Theme Additions")]
    public LMHub_010_Badge themeHub;

    private bool wrongSelection;
    private int RULE1_LVINDEX = 1;
    private int RULE2_LVINDEX = 3;
    private int RULE3_LVINDEX = 5;
    private int RULE4_LVINDEX = 6;
    public override void GetObjectReferences(GameObject _themeHub)
    {
        base.GetObjectReferences(null);
        themeHub = _themeHub.GetComponent<LMHub_010_Badge>();
    }
    public override void InitCells()
    {
        //clear old bgs
        List<Transform> oldBgs = themeHub.bgHolder.GetComponentsInChildren<Transform>().ToList();
        oldBgs.Remove(themeHub.bgHolder.transform);
        for (int i = 0; i < oldBgs.Count; i++)
        {
            Destroy(oldBgs[i].gameObject);
        }
        themeHub.bgHolder.transform.localScale = hub.boardMaster.cellHolder.localScale;
        //set existing circles
        for (int i = 0; i < hub.boardMaster.cells.Count; i++)
        {
            DataCell temp_cellData = levelData.curBoard.GetCellDataByCoord(hub.boardMaster.cells[i].coord);
            if(temp_cellData.status == (int)CellStatus.found)
            {
                GameObject obj = Instantiate(themeHub.drawingTemplate, themeHub.bgHolder);
                obj.GetComponent<CellChoice_Badge>().SetToCorrect(temp_cellData.status == (int)CellStatus.found);
                obj.transform.position = hub.boardMaster.cells[i].transform.position;
                //hub.boardMaster.cells[i].SetCellInteractable(false);
            }
        }
        //set each cell based on status
        for (int i = 0; i < hub.boardMaster.cells.Count; i++)
        {
            DataCell temp_cellData = levelData.curBoard.GetCellDataByCoord(hub.boardMaster.cells[i].coord);
            if (temp_cellData != null)
            {
                switch (temp_cellData.status)
                {
                    case (int)CellStatus.hidden:
                        hub.boardMaster.cells[i].gameObject.SetActive(false);
                        break;
                    case (int)CellStatus.locked:
                        hub.boardMaster.cells[i].numberGroup.localScale = Vector3.one * 0.65f;
                        hub.boardMaster.cells[i].SetColor(dConstants.UI.DefaultColor_4th);
                        hub.boardMaster.cells[i].SetFrameColor(dConstants.UI.DefaultColor_4th);
                        hub.boardMaster.cells[i].SetCellInteractable(false);
                        hub.boardMaster.cells[i].DisplayNumber(temp_cellData.value);
                        break;
                    case (int)CellStatus.found:
                        //Debug.LogError(string.Format("found is an unsupported status in Badge's CellInit() at {0},{1}", temp_cellData.coord.x, temp_cellData.coord.y));
                        hub.boardMaster.cells[i].SetCellInteractable(false);
                        break;
                    case (int)CellStatus.wrong:
                        //Debug.LogError(string.Format("wrong is an unsupported status in Badge's CellInit() at {0},{1}", temp_cellData.coord.x, temp_cellData.coord.y));
                        hub.boardMaster.cells[i].SetCellInteractable(false);
                        break;
                    default:
                        hub.boardMaster.cells[i].DisplayNumber(temp_cellData.value);
                        hub.boardMaster.cells[i].SetCellInteractable(true);
                        break;
                }
            }
        }
    }
    public override void InitTool()
    {
        base.InitTool();
        hub.toolMaster.toolIcon.sprite = themeHub.toolSprite;
    }
    public override void HandlePlayerInput(Vector2Int coord)
    {
        wrongSelection = false;
        if (levelData.levelIndex >= RULE1_LVINDEX)
        {
            if(!Rule1Check(levelData.curBoard, coord))
            {
                wrongSelection = true;
            }
        }
        if (levelData.levelIndex >= RULE2_LVINDEX)
        {
            if (!Rule2Check(levelData.curBoard, coord))
            {
                wrongSelection = true;
            }
        }
        if (levelData.levelIndex >= RULE3_LVINDEX)
        {
            if (!Rule3Check(levelData.curBoard, coord))
            {
                wrongSelection = true;
            }
        }
        if (levelData.levelIndex >= RULE4_LVINDEX)
        {
            if (!Rule4Check(levelData.curBoard, coord))
            {
                wrongSelection = true;
            }
        }
        if (wrongSelection)
        {
            //to do
            levelData.curBoard.GetCellDataByCoord(coord).status = (int)CellStatus.wrong;
        }
        else
        {
            levelData.curBoard.GetCellDataByCoord(coord).status = (int)CellStatus.found;
        }
    }
    public override void UpdateCells(Vector2Int coord)
    {
        DataCell temp_cellData = levelData.curBoard.GetCellDataByCoord(coord);
        for (int i = 0; i < hub.boardMaster.cells.Count; i++)
        {
            if (hub.boardMaster.cells[i].coord == coord)
            {
                GameObject obj = Instantiate(themeHub.drawingTemplate, themeHub.bgHolder);
                obj.GetComponent<CellChoice_Badge>().SetToCorrect(temp_cellData.status == (int)CellStatus.found);
                obj.transform.position = hub.boardMaster.cells[i].transform.position;
                hub.boardMaster.cells[i].SetCellInteractable(false);
            }
        }
    }
    public override bool CheckWinCondition()
    {
        if (wrongSelection)
        {
            return false;
        }
        else if(levelData.levelIndex >= 1)
        {
            return levelData.curBoard.toolCount == 0;
        }
        else
        {
            Debug.LogError(string.Format("reach undefined level in CheckWinCondition of ({0})", levelData.theme));
            return false;
        }
    }
    public override bool CheckLoseCondition()
    {
        if (levelData.curBoard.toolCount == 0)
        {
            return true;
        }
        else if (wrongSelection)
        {
            //lose if any choice is wrong
            return true;
        }
        return false;
    }
    bool Rule1Check(DataBoard board, Vector2Int coord)
    {
        //与十字相邻数字都不同
        DataCell taregt_cellData = board.GetCellDataByCoord(coord);
        for (int i = 0; i < board.cells.Count; i++)
        {
            if(BoardCalculation.Manhattan_Dist(board.cells[i].coord, coord) == 1)
            {
                if(board.cells[i].value == taregt_cellData.value)
                {
                    Debug.Log(string.Format("rule 1 check failed on {0},{1} with {2},{3}", coord.x, coord.y, board.cells[i].coord.x, board.cells[i].coord.y));
                    return false;
                }
            }
        }
        return true;
    }
    bool Rule2Check(DataBoard board, Vector2Int coord)
    {
        //与上下或左右数字奇偶性一致
        DataCell taregt_cellData = board.GetCellDataByCoord(coord);
        DataCell up_cellData = board.GetCellDataByCoord(new Vector2Int(coord.x, coord.y + 1));
        DataCell down_cellData = board.GetCellDataByCoord(new Vector2Int(coord.x, coord.y - 1));
        DataCell left_cellData = board.GetCellDataByCoord(new Vector2Int(coord.x - 1, coord.y));
        DataCell right_cellData = board.GetCellDataByCoord(new Vector2Int(coord.x + 1, coord.y));
        bool AllOdd = (up_cellData.value % 2 == 1 && down_cellData.value % 2 == 1 && taregt_cellData.value % 2 == 1)
            || (left_cellData.value % 2 == 1 && right_cellData.value % 2 == 1 && taregt_cellData.value % 2 == 1);
        bool AllEven = (up_cellData.value % 2 == 0 && down_cellData.value % 2 == 0 && taregt_cellData.value % 2 == 0)
            || (left_cellData.value % 2 == 0 && right_cellData.value % 2 == 0 && taregt_cellData.value % 2 == 0);
        bool result = (AllOdd || AllEven);
        if (!result)
        {
            Debug.Log(string.Format("rule 2 check failed on {0},{1}", coord.x, coord.y));
        }
        return result;
    }
    bool Rule3Check(DataBoard board, Vector2Int coord)
    {
        //与至少一个对角线上的数字相同
        DataCell taregt_cellData = board.GetCellDataByCoord(coord);
        DataCell diagonal_cellData1 = board.GetCellDataByCoord(new Vector2Int(coord.x - 1, coord.y - 1));
        DataCell diagonal_cellData2 = board.GetCellDataByCoord(new Vector2Int(coord.x - 1, coord.y + 1));
        DataCell diagonal_cellData3 = board.GetCellDataByCoord(new Vector2Int(coord.x + 1, coord.y - 1));
        DataCell diagonal_cellData4 = board.GetCellDataByCoord(new Vector2Int(coord.x + 1, coord.y + 1));
        bool result = (taregt_cellData.value == diagonal_cellData1.value || taregt_cellData.value == diagonal_cellData2.value
             || taregt_cellData.value == diagonal_cellData3.value || taregt_cellData.value == diagonal_cellData4.value);
        if (!result)
        {
            Debug.Log(string.Format("rule 3 check failed on {0},{1}", coord.x, coord.y));
        }
        return result;
    }
    bool Rule4Check(DataBoard board, Vector2Int coord)
    {
        //在环形相邻数字中不是最大或最小的
        List<int> applicableNumbers = new List<int>();
        DataCell taregt_cellData = board.GetCellDataByCoord(coord);
        applicableNumbers.Add(taregt_cellData.value);
        for (int i = 0; i < board.cells.Count; i++)
        {
            if (BoardCalculation.Ring_Dist(board.cells[i].coord, coord) == 1)
            {
                applicableNumbers.Add(board.cells[i].value);
            }
        }
        bool result = (taregt_cellData.value != Mathf.Max(applicableNumbers.ToArray()) && taregt_cellData.value != Mathf.Min(applicableNumbers.ToArray()));
        if (!result)
        {
            Debug.Log(string.Format("rule 4 check failed on {0},{1}", coord.x, coord.y));
        }
        return result;
    }
}
