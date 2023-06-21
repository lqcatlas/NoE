using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coin_rule : gRuleBase
{
    override public void Play(int coord, dBoard board)
    {
        if (board.toolCount > 0)
        {
            board.toolCount -= 1;
            //phase 1
            //coin(head): play to +1
            if (phase == 1)
            {
                for (int i = 0; i < board.boardCells.Count; i++)
                {
                    if (board.boardCells[i].coordinates == coord)
                    {
                        head_play_1(board.boardCells[i]);
                    }
                }
            }
            //phase 2
            //coin(tail): play to -1
            else if (phase == 2)
            {
                for (int i = 0; i < board.boardCells.Count; i++)
                {
                    if (board.boardCells[i].coordinates == coord)
                    {
                        tail_play_1(board.boardCells[i]);
                    }
                }
            }
            //phase 3
            //coin(head): play to +1
            //coin(tail): play to -1
            //coin flips
            else if (phase == 3)
            {
                for (int i = 0; i < board.boardCells.Count; i++)
                {
                    if (board.boardCells[i].coordinates == coord)
                    {
                        if(board.toolStatus == 0)
                        {
                            head_play_1(board.boardCells[i]);
                        }
                        else if(board.toolStatus == 1)
                        {
                            tail_play_1(board.boardCells[i]);
                        }
                        else
                        {
                            Debug.LogError("invalid ToolStatus on coin_rule play");
                        }
                    }
                }
                coin_flip(board);
            }
            //phase 4
            //coin(head): play to +X
            //coin(tail): play to -1
            //coin flips
            else if (phase == 4)
            {
                for (int i = 0; i < board.boardCells.Count; i++)
                {
                    if (board.boardCells[i].coordinates == coord)
                    {
                        if (board.toolStatus == 0)
                        {
                            head_play_X(board.boardCells[i]);
                        }
                        else if (board.toolStatus == 1)
                        {
                            tail_play_1(board.boardCells[i]);
                        }
                        else
                        {
                            Debug.LogError("invalid ToolStatus on coin_rule play");
                        }
                    }
                }
                coin_flip(board);
            }
            //phase 5
            //coin(head): play to +X
            //coin(tail): play to -X
            //coin flips
            else if (phase == 5)
            {
                for (int i = 0; i < board.boardCells.Count; i++)
                {
                    if (board.boardCells[i].coordinates == coord)
                    {
                        if (board.toolStatus == 0)
                        {
                            head_play_X(board.boardCells[i]);
                        }
                        else if (board.toolStatus == 1)
                        {
                            tail_play_X(board.boardCells[i]);
                        }
                        else
                        {
                            Debug.LogError("invalid ToolStatus on coin_rule play");
                        }
                    }
                }
                coin_flip(board);
            }
            //phase 6
            //coin(head): play to +X
            //coin(tail): play to -X
            //coin tossed
            else if (phase == 6)
            {
                for (int i = 0; i < board.boardCells.Count; i++)
                {
                    if (board.boardCells[i].coordinates == coord)
                    {
                        if (board.toolStatus == 0)
                        {
                            head_play_X(board.boardCells[i]);
                        }
                        else if (board.toolStatus == 1)
                        {
                            tail_play_X(board.boardCells[i]);
                        }
                        else
                        {
                            Debug.LogError("invalid ToolStatus on coin_rule play");
                        }
                    }
                }
                coin_toss(board);
            }
            else
            {
                Debug.LogError(string.Format("fail to find coin rule for phase {0}", phase));
            }
        }
        else
        {
            Debug.LogError(string.Format("run out of tools"));
        }
    }
    void coin_flip(dBoard board)
    {
        board.toolStatus = 1 - board.toolStatus;
    }
    void coin_toss(dBoard board)
    {
        board.toolStatus = Random.Range(0, 2);
    }
    void head_play_1(Cell cell)
    {
        cell.value += 1;
        cell.value = SetToNumber0_9(cell.value);
        cell.status += 1;
    }
    void head_play_X(Cell cell)
    {
        
        cell.value += cell.status;
        cell.value = SetToNumber0_9(cell.value);
        cell.status += 1;
    }
    void tail_play_1(Cell cell)
    {
        cell.value -= 1;
        cell.value = SetToNumber0_9(cell.value);
        cell.status += 1;
    }
    void tail_play_X(Cell cell)
    {
        cell.value -= cell.status;
        cell.value = SetToNumber0_9(cell.value);
        cell.status += 1;
    }
    int SetToNumber0_9(int anyNumber)
    {
        if(anyNumber >= 0 && anyNumber <= 9)
        {
            return anyNumber;
        }
        else if(anyNumber > 9)
        {
            return anyNumber % 10;
        }
        else
        {
            return 10 + anyNumber % 10;
        }
    }
}

