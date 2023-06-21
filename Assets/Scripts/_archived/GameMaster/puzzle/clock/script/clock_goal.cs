using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clock_goal : gGoalBase
{
    override public bool GoalCheck(dBoard board)
    {
        if(level == 1)
        {
            //set all into X
            bool allTarget = true;
            int targetValue = 8;
            for (int i = 0; i < board.boardCells.Count; i++)
            {
                if (board.boardCells[i].value != targetValue)
                {
                    allTarget = false;
                }
            }
            return allTarget;
        }
        else if(level == 2)
        {
            //set all into X
            bool allTarget = true;
            int targetValue = 10;
            for (int i = 0; i < board.boardCells.Count; i++)
            {
                if (board.boardCells[i].value != targetValue)
                {
                    allTarget = false;
                }
            }
            return allTarget;
        }
        else if (level == 3)
        {
            //set all into X
            bool allTarget = true;
            int targetValue = 1;
            for (int i = 0; i < board.boardCells.Count; i++)
            {
                if (board.boardCells[i].value != targetValue)
                {
                    allTarget = false;
                }
            }
            return allTarget;
        }
        else if (level == 4 || level == 7)
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
        else if (level == 5)
        {
            //set all into X
            bool allTarget = true;
            int targetValue = 3;
            for (int i = 0; i < board.boardCells.Count; i++)
            {
                if (board.boardCells[i].value != targetValue)
                {
                    allTarget = false;
                }
            }
            return allTarget;
        }
        else if (level == 6)
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
        else if (level == 8)
        {
            //set all into X
            bool allTarget = true;
            int targetValue = 6;
            for (int i = 0; i < board.boardCells.Count; i++)
            {
                if (board.boardCells[i].value != targetValue)
                {
                    allTarget = false;
                }
            }
            return allTarget;
            //clear all cells at the same time
            /*bool allClearSim = true;
            for (int i = 0; i < board.boardCells.Count; i++)
            {
                if (board.boardCells[i].status != 2)
                {
                    allClearSim = false;
                }
            }
            return allClearSim;
            */
        }
        else
        {
            Debug.LogError(string.Format("clock goal is invalid on level {0}",level));
            return false;
        }
        
    }
}
