using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CSVData/LevelSetup")]
public class SheetItem_LevelSetup : ScriptableObject
{
    [Header("IDs")]
    //an unique ID to locate the level among all levels
    public int levelUID;
    public int themeIndex;
    public int levelIndex;
    public int nextLevelIndex;
    [Header("Connected Levels")]
    public SheetItem_LevelSetup previousLevel;
    public SheetItem_LevelSetup nextLevel;
    [Header("Playing-Facing Txt")]
    //below are player-facing values
    //theme of the level, might be better as enum. But string is good for now. eg. Clock
    public string theme;
    //title of the level. eg.  ±÷” 1
    public string title;
    //ONE new narrative line that should be added at the beginning of this level
    public string newInitNarrative;
    //multiple new narrative lines that should be added during this level triggered by certain behavior
    public List<string> newPlayNarratives;
    //old narrative lines that are derived from previous levels of the same theme 
    public List<string> oldNarratives;
    //goal description. Eg. set all cells to 3.
    public string goal;
    //tool ruleset description. Eg. Clock: when played, +1. blabla
    public List<RuleItem> ruleset;

    [Header("Board Setup")]
    public bool isHard;
    public bool allowRewind = true;
    public DataBoard initBoard;

    
}
