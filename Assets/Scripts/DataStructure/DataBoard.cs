using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class DataBoard
{
    //A board is the collection of all variants of a level (for now, cells + tool).
    //TBD: Whether this should be a mono / scriptable / etc

    //size of the board
    public Vector2Int boardSize;
    //a list of cells in the board
    public List<DataCell> cells;
    //number of tools left can be played.
    public int toolCount;
    //tool status: status is the secondary value of a tool to support gameplay. eg. in coin theme, the status represents head/tail of a coin.
    public int toolStatus;

    public DataBoard()
    {
        boardSize = new Vector2Int(0, 0);
        cells = new List<DataCell>();
        toolCount = 0;
        toolStatus = 0;
    }
    public DataBoard(DataBoard copyBoard)
    {
        boardSize = copyBoard.boardSize;
        cells = new List<DataCell>();
        for(int i=0;i< copyBoard.cells.Count; i++)
        {
            cells.Add(new DataCell(copyBoard.cells[i]));
        }
        //clone by ToList() does not work on customized class
        //cells = copyBoard.cells.ToList();
        toolCount = copyBoard.toolCount;
        toolStatus = copyBoard.toolStatus;
    }
    public DataCell GetCellDataByCoord(Vector2Int targetCoord)
    {
        for(int i = 0; i < cells.Count; i++)
        {
            if (cells[i].coord == targetCoord)
            {
                return cells[i];
            }
        }
        return null;
    }
    public int CurrentSum()
    {
        int sum = 0;
        for (int i = 0; i < cells.Count; i++)
        {
            sum += cells[i].value;
        }
        return sum;
    }
}
