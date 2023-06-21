using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PuzzleUtility
{
    public static bool WithinRange_3x3(int coord1, int coord2)
    {
        int x_1 = getX(coord1);
        int y_1 = getY(coord1);
        int x_2 = getX(coord2);
        int y_2 = getY(coord2);
        if(coord1 == coord2)
        {
            return true;
        }
        else if(Mathf.Abs(x_1 - x_2) <= 1 && Mathf.Abs(y_1 - y_2) <= 1)
        {
            return true;
        }
        return false;
    }
    public static int getX(int coord)
    {
        return Mathf.FloorToInt(coord / 10f);
    }
    public static int getY(int coord)
    {
        return coord % 10;
    }
}
