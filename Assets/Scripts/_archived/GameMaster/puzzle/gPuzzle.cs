using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gPuzzle : MonoBehaviour
{
    public string title;

    public string goalDesc;

    public string toolName;
    public string toolDesc;

    public List<string> narratives;

    public dBoard puzzleBoard;
    public gRuleBase ruleScript;
    public gGoalBase goalScript;
    public gFailBase failScript;

    public void Play(int coord, dBoard board)
    {
        ruleScript.Play(coord, board);
    }
    public bool GoalCheck(dBoard board)
    {
        return goalScript.GoalCheck(board);
    }
    public bool FailCheck(dBoard board)
    {
        return failScript.FailCheck(board);
    }
}
