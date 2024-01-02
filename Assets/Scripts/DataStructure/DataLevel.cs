using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UIElements;
[System.Serializable]
public class RuleItem
{
    public enum RuleItemTag:int { none = 0, addition = 1, transition = 2, deletion = 3 };
    public RuleItemTag tag;
    public string ruleDesc;

    public RuleItem()
    {
        tag = RuleItemTag.none;
        ruleDesc = "";
    }
    public RuleItem(RuleItem copyItem)
    {
        tag = copyItem.tag;
        ruleDesc = copyItem.ruleDesc;
    }
    public RuleItem(string tag_txt, string rule_txt)
    {
        switch (tag_txt)
        {
            case "None":
                tag = RuleItemTag.none;
                break;
            case "Addition":
                tag = RuleItemTag.addition;
                break;
            case "Transition":
                tag = RuleItemTag.transition;
                break;
            case "Deletion":
                tag = RuleItemTag.deletion;
                break;
            default:
                tag = RuleItemTag.none;
                break;
        }
        ruleDesc = rule_txt;
    }
}
[System.Serializable]
public class DataLevel
{
    //DataLevel covers all data in a puzzle level. eg. Clock Level 6.
    //TBD: Whether this should be a mono / scriptable / etc
    
    [Header("IDs")]
    //an unique ID to locate the level among all levels
    public int levelUID;
    public int themeIndex;
    public int levelIndex;


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

    [Header("Boards")]
    public bool isHard;
    //4 boards: record current, initial, previous, preview state of a level
    public DataBoard curBoard;
    public DataBoard initBoard;
    public DataBoard previousBoard;
    //preview board is not used for now
    public DataBoard previewBoard;

    public DataLevel()
    {
        levelUID = -1;

        curBoard = new DataBoard();
        initBoard = new DataBoard();
        previousBoard = new DataBoard();
        previewBoard = new DataBoard();

        theme = "theme of the level";
        title = "level title";
        newInitNarrative = "";
        newPlayNarratives = new List<string>();
        oldNarratives = new List<string>();
        goal = "level goal";
        ruleset = new List<RuleItem>();

        isHard = false;
    }
    public DataLevel(DataLevel copyLevel)
    {
        levelUID = copyLevel.levelUID;

        curBoard = new DataBoard(copyLevel.curBoard);
        initBoard = new DataBoard(copyLevel.initBoard);
        previousBoard = new DataBoard(copyLevel.previousBoard);
        previewBoard = new DataBoard(copyLevel.previewBoard);

        theme = copyLevel.theme;
        title = copyLevel.title;
        newInitNarrative = copyLevel.newInitNarrative;
        newPlayNarratives = copyLevel.newPlayNarratives.ToList();
        oldNarratives = copyLevel.oldNarratives.ToList();
        goal = copyLevel.goal;
        for (int i = 0; i < copyLevel.ruleset.Count; i++)
        {
            ruleset.Add(new RuleItem(copyLevel.ruleset[i]));
        }
        isHard = false;
    }
    public bool LoadLevelFromSheetItem(SheetItem_LevelSetup setupData)
    {
        if(setupData == null)
        {
            Debug.LogError("read level setup from a null setup");
            return false;
        }
        if(setupData.levelUID == -1)
        {
            Debug.LogError("read level setup from a invalid UID");
            return false;
        }
        //Debug.Log(string.Format("init a level w/ UID: {0}", setupData.levelUID));
        levelUID = setupData.levelUID;
        themeIndex = setupData.themeIndex;
        levelIndex = setupData.levelIndex;

        initBoard = new DataBoard(setupData.initBoard);

        theme = setupData.theme;
        title = setupData.title;
        newInitNarrative = setupData.newInitNarrative;
        newPlayNarratives = setupData.newPlayNarratives.ToList();
        oldNarratives = setupData.oldNarratives.ToList();
        goal = setupData.goal;
        ruleset.Clear();
        for (int i = 0; i < setupData.ruleset.Count; i++)
        {
            ruleset.Add(new RuleItem(setupData.ruleset[i]));
        }
        isHard = setupData.isHard;
        return true;
    }
}
