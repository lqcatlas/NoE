using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class BoardCalculation
{
    //shared board related calculation functions
    //used in goal and play calculation across themes
    static public int ModX_Range(int X, Vector2Int Range)
    {
        int rangeDistance = Range.y - Range.x + 1;
        if (X >= Range.x && X <= Range.y)
        {
            return X;
        }
        else if (X > Range.y)
        {
            int deduction = Mathf.CeilToInt((float)(X - Range.y) / rangeDistance) * rangeDistance;
            return X - deduction;
        }
        else
        {
            int addition = Mathf.CeilToInt((float)(Range.x - X) / rangeDistance) * rangeDistance;
            return X + addition;
        }
    }
    static public bool CountX_Ytimes(DataBoard board, int X, int Y)
    {
        int requiredTargetCount = Y;
        int targetCount = 0;
        int targetValue = X;
        for (int i = 0; i < board.cells.Count; i++)
        {
            if (board.cells[i].value == targetValue)
            {
                targetCount += 1;
            }
        }
        return targetCount >= requiredTargetCount;
    }
    static public bool CountStatusX_Ytimes(DataBoard board, int X, int Y)
    {
        int requiredTargetCount = Y;
        int targetCount = 0;
        int targetValue = X;
        for (int i = 0; i < board.cells.Count; i++)
        {
            if (board.cells[i].status == targetValue)
            {
                targetCount += 1;
            }
        }
        return targetCount >= requiredTargetCount;
    }
    static public bool CountX_All(DataBoard board, int X)
    {
        bool allTarget = true;
        int targetValue = X;
        for (int i = 0; i < board.cells.Count; i++)
        {
            if (board.cells[i].value != targetValue)
            {
                allTarget = false;
                return allTarget;
            }
        }
        return allTarget;
    }
    static public bool CountStatusX_All(DataBoard board, int X)
    {
        bool allTarget = true;
        int targetValue = X;
        for (int i = 0; i < board.cells.Count; i++)
        {
            if (board.cells[i].status != targetValue)
            {
                allTarget = false;
                return allTarget;
            }
        }
        return allTarget;
    }
    static public bool CountXorY_All(DataBoard board, int X, int Y)
    {
        bool allTarget = true;
        int targetValue1 = X;
        int targetValue2 = Y;
        for (int i = 0; i < board.cells.Count; i++)
        {
            if (!(board.cells[i].value == targetValue1 || board.cells[i].value == targetValue2))
            {
                allTarget = false;
                return allTarget;
            }
        }
        return allTarget;
    }
    static public bool CountStatusXorY_All(DataBoard board, int X, int Y)
    {
        bool allTarget = true;
        int targetValue1 = X;
        int targetValue2 = Y;
        for (int i = 0; i < board.cells.Count; i++)
        {
            if (!(board.cells[i].status == targetValue1 || board.cells[i].status == targetValue2))
            {
                allTarget = false;
                return allTarget;
            }
        }
        return allTarget;
    }
    static public bool Same_All(DataBoard board)
    {
        bool allSame = true;
        for (int i = 0; i < board.cells.Count; i++)
        {
            for (int j = i + 1; j < board.cells.Count; j++)
            {
                if (board.cells[i].value != board.cells[j].value)
                {
                    allSame = false;
                    return allSame;
                }
            }
        }
        return allSame;
    }
    static public bool Same_Ytimes(DataBoard board, int Y)
    {
        int requiredSameCount = Y;
        Debug.LogError(string.Format("unfinished function SameYTimes called"));
        return false;
        //to do
    }
    static public bool Unique_All(DataBoard board)
    {
        bool allUnique = true;
        for (int i = 0; i < board.cells.Count; i++)
        {
            for (int j = i + 1; j < board.cells.Count; j++)
            {
                if (board.cells[i].value == board.cells[j].value)
                {
                    allUnique = false;
                    return allUnique;
                }
            }
        }
        return allUnique;
    }
    static public bool Unique_Ytimes(DataBoard board, int Y)
    {
        int requiredUniqueCount = Y;
        int uniqueCount = board.cells.Count;
        bool firstDup = true;
        for (int i = 0; i < board.cells.Count; i++)
        {
            for (int j = i + 1; j < board.cells.Count; j++)
            {
                if (board.cells[i].value == board.cells[j].value)
                {
                    uniqueCount -= 1;
                    if (firstDup)
                    {
                        uniqueCount -= 1;
                        firstDup = false;
                    }
                }
            }
        }
        return uniqueCount >= requiredUniqueCount;
    }
    static public bool Sum_As_X(DataBoard board, int X)
    {
        int requiredSum = X;
        int sum = 0;
        for (int i = 0; i < board.cells.Count; i++)
        {
            sum += board.cells[i].value;
        }
        Debug.Log(string.Format("Sum_As_X() now sum is {0}", sum));
        return sum == requiredSum;
    }
    static public bool Sum_Larger_X(DataBoard board, int X)
    {
        int requiredSum = X;
        int sum = 0;
        for (int i = 0; i < board.cells.Count; i++)
        {
            sum += board.cells[i].value;
        }
        return sum >= requiredSum;
    }
    static public bool Sum_Lesser_X(DataBoard board, int X)
    {
        int requiredSum = X;
        int sum = 0;
        for (int i = 0; i < board.cells.Count; i++)
        {
            sum += board.cells[i].value;
        }
        return sum <= requiredSum;
    }
}
