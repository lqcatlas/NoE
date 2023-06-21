using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coin_fail : gFailBase
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
        else
        {
            Debug.LogError(string.Format("fail to find coin fail check for phase {0}", phase));
            return false;
        }
    }
}
