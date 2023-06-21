using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clock_rule : gRuleBase
{
    override public void Play(int coord, dBoard board)
    {
        //cell is played get +1
        if (phase == 1)
        {
            
            if (board.toolCount > 0)
            {
                board.toolCount -= 1;
                for (int i = 0; i < board.boardCells.Count; i++)
                {
                    if (board.boardCells[i].coordinates == coord)
                    {
                        board.boardCells[i].value += 1;
                        if (board.boardCells[i].value > 12)
                        {
                            board.boardCells[i].value = 1;
                        }
                    }
                }
            }
        }
        //cell is played get +1
        //cell was played all get +1 as well
        else if (phase == 2)
        {
            if (board.toolCount > 0)
            {
                board.toolCount -= 1;
                for (int i = 0; i < board.boardCells.Count; i++)
                {
                    if (board.boardCells[i].status == 1)
                    {
                        board.boardCells[i].value += 1;
                        if (board.boardCells[i].value > 12)
                        {
                            board.boardCells[i].value = 1;
                        }
                    }
                }
                for (int i = 0; i < board.boardCells.Count; i++)
                {
                    if (board.boardCells[i].coordinates == coord)
                    {
                        board.boardCells[i].value += 1;
                        if (board.boardCells[i].value > 12)
                        {
                            board.boardCells[i].value = 1;
                        }
                        board.boardCells[i].status = 1;
                    }
                }
            }
        }
        //deprecated
        //cell is played get +1
        //cell was played get +1 as well
        //cell ends played ends at 6 get a 3x3 clear()
        /*else if (phase == 3)
        {
            List<int> CoordAt6 = new List<int>();
            if (board.toolCount > 0)
            {
                board.toolCount -= 1;
                for (int i = 0; i < board.boardCells.Count; i++)
                {
                    if (board.boardCells[i].status == 1)
                    {
                        board.boardCells[i].value += 1;
                        if (board.boardCells[i].value > 12)
                        {
                            board.boardCells[i].value = 1;
                        }
                        else if (board.boardCells[i].value == 6)
                        {
                            CoordAt6.Add(board.boardCells[i].coordinates);
                        }
                    }
                }
                for (int i = 0; i < board.boardCells.Count; i++)
                {
                    if (board.boardCells[i].status == 2)
                    {
                        //do nothing
                    }
                    else if (board.boardCells[i].coordinates == coord)
                    {
                        board.boardCells[i].value += 1;
                        if (board.boardCells[i].value > 12)
                        {
                            board.boardCells[i].value = 1;
                        }
                        else if(board.boardCells[i].value == 6)
                        {
                            CoordAt6.Add(board.boardCells[i].coordinates);
                        }
                        board.boardCells[i].status = 1;
                    }
                }
                //handle cell clear regarding cell at 6
                for(int i=0;i< board.boardCells.Count; i++)
                {
                    for(int j=0;j< CoordAt6.Count; j++)
                    {
                        if(PuzzleUtility.WithinRange_3x3(board.boardCells[i].coordinates, CoordAt6[j]))
                        {
                            board.boardCells[i].value = 0;
                            board.boardCells[i].status = 2;
                        }
                    }
                }
            }
        }*/
        else
        {
            Debug.LogError(string.Format("fail to find clock rule for phase {0}", phase));
        }
    }
}
