using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coin_goal : gGoalBase
{
    override public bool GoalCheck(dBoard board)
    {

        if (level == 1 || level == 2)
        {
            //set all into X
            bool allTarget = true;
            int targetValue = 4;
            for (int i = 0; i < board.boardCells.Count; i++)
            {
                if (board.boardCells[i].value != targetValue)
                {
                    allTarget = false;
                }
            }
            return allTarget;
        }
        else if (level == 3 || level == 6 || level == 8)
        {
            //set all into same numbers
            bool allSame = true;
            int targetValue = board.boardCells[0].value;
            for (int i = 0; i < board.boardCells.Count; i++)
            {
                if (targetValue != board.boardCells[i].value)
                {
                    allSame = false;
                }
            }
            return allSame;
        }
        else if (level == 4)
        {
            //set Y cell into X
            int targetValue = 9;
            int minCell = 2;

            int passedCell = 0;
            for (int i = 0; i < board.boardCells.Count; i++)
            {
                if (board.boardCells[i].value == targetValue)
                {
                    passedCell += 1;
                }
            }
            return passedCell >= minCell;
        }
        else if (level == 5)
        {
            //set Y cell into X
            int targetValue = 1;
            int minCell = 2;

            int passedCell = 0;
            for (int i = 0; i < board.boardCells.Count; i++)
            {
                if (board.boardCells[i].value == targetValue)
                {
                    passedCell += 1;
                }
            }
            return passedCell >= minCell;
        }
        else if (level == 7)
        {
            //set all into unique numbers
            bool allUnique = true;
            for (int i = 0; i < board.boardCells.Count - 1; i++)
            {
                for (int j = i + 1; j < board.boardCells.Count; j++)
                {
                    if (board.boardCells[i].value == board.boardCells[j].value)
                    {
                        allUnique = false;
                    }
                }
            }
            return allUnique;
        }
        else
        {
            Debug.LogError(string.Format("clock goal is invalid on level {0}", level));
            return false;
        }

    }
}
