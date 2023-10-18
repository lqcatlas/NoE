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
        for(int i = 0; i < finishedLevels.Count; i++)
        {
            if (finishedLevels[i] == uid)
            {
                //Debug.Log(string.Format("check level UID {0} in finished level, return true", uid));
                return true;
            }
        }
        //Debug.Log(string.Format("check level UID {0} in finished level, return false", uid));
        return false;
    }
    public bool isThemeUnlocked(int uid)
    {
        for (int i = 0; i < unlockedThemes.Count; i++)
        {
            if (unlockedThemes[i] == uid)
            {
                return true;
            }
        }
        return false;
    }
    
}
