using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayingData/LevelRecords")]
public class LevelRecords : ScriptableObject
{
    public int tokens;
    public List<int> finishedLevels;
    public List<int> unlockedThemes;

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
    public bool isThemeUnlocked(int uid)
    {
        bool found = false;
        for (int i = 0; i < unlockedThemes.Count; i++)
        {
            if (unlockedThemes[i] == uid)
            {
                found = true;
                break;
            }
        }
        return found;
    }
}
