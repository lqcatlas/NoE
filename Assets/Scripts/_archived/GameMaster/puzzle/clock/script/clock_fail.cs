using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clock_fail : gFailBase
{
    override public bool FailCheck(dBoard board)
    {
        if (board.toolCount == 0)
        {
            return true;
        }
        //cell is played get +1
        if (phase == 1)
        {
            return false;
        }
        /*else if(phase == 2)
        {
            bool hasClear = false;
            for (int i = 0; i < board.boardCells.Count; i++)
            {
                if (board.boardCells[i].status == 2)
                {
                    hasClear = true;
                }
            }
            return hasClear;
        }*/
        else
        {
            Debug.LogError(string.Format("fail to find clock fail check for phase {0}", phase));
            return false;
        }
    }
}
