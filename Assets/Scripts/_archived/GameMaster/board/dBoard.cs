using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;



[System.Serializable]
public class Cell
{
    public int coordinates;
    public int value;
    public int status;

    public Cell()
    {
        coordinates = -1;
        value = -1;
        status = -1;
        //-1 is illegal
    }

    public Cell(Cell copyCell)
    {
        coordinates = copyCell.coordinates;
        value = copyCell.value;
        status = copyCell.status;
    }
}
[CreateAssetMenu(menuName = "Gameplay/BoardData")]
public class dBoard : ScriptableObject
{
    public string boardName;
    public int boardSizeX;
    public int boardSizeY;
    public int toolCount;
    public int toolStatus;
    public List<Cell> boardCells;

    public dBoard()
    {
        boardName = "";
        boardSizeX = 0;
        boardSizeY = 0;
        boardCells = new List<Cell>();
        toolCount = 0;
        toolStatus = 0;
    }

    public dBoard(dBoard copyBoard):base()
    {
        boardName = copyBoard.boardName;
        boardSizeX = copyBoard.boardSizeX;
        boardSizeY = copyBoard.boardSizeY;
        toolCount = copyBoard.toolCount;
        toolStatus = copyBoard.toolStatus;
        for (int i=0;i< copyBoard.boardCells.Count; i++)
        {
            boardCells.Add(new Cell(copyBoard.boardCells[i]));
        }
    }
    public void SetBoard(dBoard targetBoard)
    {
        boardName = targetBoard.boardName;
        boardSizeX = targetBoard.boardSizeX;
        boardSizeY = targetBoard.boardSizeY;
        boardCells.Clear();
        toolCount = targetBoard.toolCount;
        toolStatus = targetBoard.toolStatus;
        for (int i = 0; i < targetBoard.boardCells.Count; i++)
        {
            boardCells.Add(new Cell(targetBoard.boardCells[i]));
        }
        BoardLegitCheck();
    }
    public Cell GetCellByCoord(int coord)
    {
        Cell cell = new Cell();
        for (int i = 0; i < boardCells.Count; i++)
        {
            if (boardCells[i].coordinates == coord)
            {
                cell = new(boardCells[i]);
                return cell;
            }
        }
        return cell;
    }
    public bool BoardLegitCheck()
    {
        bool legit = true;
        //check cell count
        if (boardCells.Count != boardSizeX * boardSizeY)
        {
            legit = false;
            Debug.LogError(string.Format("wrong cell count {1} (should be {2}) on Board {0}", boardName, boardCells.Count, boardSizeX * boardSizeY));
        }
        //check each cell uniqueness 
        for (int i = 1; i <= boardSizeX; i++)
        {
            for (int j = 1; j <= boardSizeY; j++)
            {
                bool found = false;
                for (int k = 0; k < boardCells.Count; k++)
                {
                    if (boardCells[k].coordinates == (i * 10 + j))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    legit = false;
                    Debug.LogError(string.Format("unbale to find coordinate {1} on Board {0}", boardName, i * 10 + j));
                }
            }
        }
        /* for (int i = 0; i < boardCells.Count-1; i++)
        {
            for (int j = i+1; j < boardCells.Count; j++)
            {
                if (boardCells[i].coordinates == boardCells[j].coordinates)
                {
                    legit = false;
                    Debug.LogError(string.Format("duplicate coordinates {1} found on Board {0}", BoardName, boardCells[i].coordinates));
                }

            }
        } */
        return legit;
    }
    public bool CoordLegitCheck(int coord)
    {
        bool legit = false;
        for(int i = 0; i < boardCells.Count; i++)
        {
            if (boardCells[i].coordinates == coord)
            {
                legit = true;
                return legit;
            }
        }
        return legit;
    }
}
