using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayingData/LevelRecords")]
public class LevelRecords : ScriptableObject
{
    public List<int> finishedLevels;

    public bool isLevelFinished(int uid)
    {
        bool found = false;
        for(int i = 0; i < finishedLevels.Count; i++)
        {
            if (finishedLevels[i] == uid)
            {
                found = true;
                break;
            }
        }
        return found;
    }
}
