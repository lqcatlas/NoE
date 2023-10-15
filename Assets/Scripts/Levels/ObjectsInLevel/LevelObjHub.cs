using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelObjHub : MonoBehaviour
{
    public BoardMaster boardMaster;
    public NarrativeMaster narrativeMaster;
    public ToolMaster toolMaster;
    public GoalMaster goalMaster;
    public RulesetMaster rulesetMaster;
    public MiscMaster miscMaster;

    public void DebugMsg()
    {
        Debug.Log(string.Format("debug msg sent from obj:{0}", gameObject.name));
    }
}
