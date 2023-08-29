using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class BoardCalculation
{
    //shared board related calculation functions
    //used in goal and play calculation across themes
    static public int Manhattan_Dist(Vector2Int coord1, Vector2Int coord2)
    {
        return Mathf.Abs(coord1.x - coord2.x) + Mathf.Abs(coord1.y - coord2.y);
    }
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
    static public int ConstrainX_Range(int X, Vector2Int Range)
    {
        int rangeDistance = Range.y - Range.x + 1;
        if (X >= Range.x && X <= Range.y)
        {
            return X;
        }
        else if (X > Range.y)
        {
            return Range.y;
        }
        else
        {
            return Range.x;
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
    static public bool CountXplus_Ytimes(DataBoard board, int X, int Y)
    {
        int requiredTargetCount = Y;
        int targetCount = 0;
        int targetValue = X;
        for (int i = 0; i < board.cells.Count; i++)
        {
            if (board.cells[i].value >= targetValue)
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
    static public bool ExactLineMatchX(DataBoard board, List<int> X)
    {
        //number must > -1
        if(X.Count != board.boardSize.x)
        {
            Debug.LogError(string.Format("ExactLineMatchX function is called on a dismatching X({0}) and board size({1})", X.Count, board.boardSize.y));
            return false;
        }
        List<List<int>> NumbersByLine = new List<List<int>>();
        for(int i=0;i< board.boardSize.x; i++)
        {
            NumbersByLine.Add(new List<int>());
            for (int j = 0; j < board.boardSize.y; j++)
            {
                NumbersByLine[i].Add(-1);
            }
        }
        for (int i = 0; i < board.cells.Count; i++)
        {
            NumbersByLine[board.cells[i].coord.x][board.cells[i].coord.y] = board.cells[i].value;
        }
        for (int i = 0; i < board.boardSize.y; i++)
        {
            bool lineMatch = true;
            for (int j = 0; j < board.boardSize.x; j++)
            {
                if(NumbersByLine[j][i] != X[j])
                {
                    lineMatch = false;
                    break;
                }
            }
            if (lineMatch)
            {
                return true;
            }
        }
        return false;
    }
    static public bool ExactRowMatchX(DataBoard board, List<int> X)
    {
        //number must > -1
        if (X.Count != board.boardSize.y)
        {
            Debug.LogError(string.Format("ExactLineMatchX function is called on a dismatching X({0}) and board size({1})", X.Count, board.boardSize.y));
            return false;
        }
        List<List<int>> NumbersByLine = new List<List<int>>();
        for (int i = 0; i < board.boardSize.y; i++)
        {
            NumbersByLine.Add(new List<int>());
            for (int j = 0; j < board.boardSize.x; j++)
            {
                NumbersByLine[i].Add(-1);
            }
        }
        for (int i = 0; i < board.cells.Count; i++)
        {
            NumbersByLine[board.cells[i].coord.y][board.cells[i].coord.x] = board.cells[i].value;
        }
        for (int i = 0; i < board.boardSize.x; i++)
        {
            bool rowMatch = true;
            for (int j = board.boardSize.y - 1; j >= 0 ; j--)
            {
                if (NumbersByLine[i][j] != X[board.boardSize.y - j])
                {
                    rowMatch = false;
                    break;
                }
            }
            if (rowMatch)
            {
                return true;
            }
        }
        return false;
    }
}
