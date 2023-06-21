using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCoinManager : LevelMasterBaseTest
{
    public override void clickCell() {
        base.clickCell();
        int count = target.index;
        count++;//硬币，点击后+1
        //这里做任何运算
        target.index = count;
        base.renderCell(target);
        DoSomethingAfter(1, runClock);
        DoSomethingAfter(2, startTurn);
    }
    void runClock()
    {
        foreach(KeyValuePair<GameObject,CellInfo> pair in board)
        {
            pair.Value.index++;//
            renderCell(pair.Value);
        }
    }
}
