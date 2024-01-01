using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public struct LevelNode
{
    public int previousLevelUID;
    public DataLevel level;
}
public class DataTheme
{
    public SheetItem_ThemeSetup setupData;

    public DataTheme()
    {
        
    }
    public DataTheme(DataTheme copyTheme)
    {
        
    }
    public bool InitThemeData(SheetItem_ThemeSetup setupData, LevelRecords record)
    {
        return false;
    }
}
