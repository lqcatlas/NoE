using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ToolStatusDisplay
{
    public string statusName;
    public Sprite playInfograph;
    public ToolStatusDisplay()
    {
        statusName = null;
        playInfograph = null;
    }
}
[System.Serializable]
public class ToolStatusGroup
{
    public List<ToolStatusDisplay> toolStatus;
    public ToolStatusGroup()
    {
        toolStatus = new List<ToolStatusDisplay>();
    }
    public string GetStatusName(int status)
    {
        if(status < toolStatus.Count)
        {
            if (toolStatus[status].statusName != null)
            {
                if(toolStatus[status].statusName.Length > 0)
                {
                    return toolStatus[status].statusName;
                }
            }
        }
        return null;
    }
    public Sprite GetStatusInfograph(int status)
    {
        if (status < toolStatus.Count)
        {
            if (toolStatus[status].playInfograph != null)
            {
                return toolStatus[status].playInfograph;
            }
        }
        return null;
    }
}
