using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "PlayingData/LevelRecords")]
public class LevelRecords : ScriptableObject
{
    public int tokens;
    public int spentTokens;
    public int gems;
    public List<int> finishedLevels;
    public List<int> unlockedThemes;
    public bool seeHiddenGemNotice;

    public bool addLevelFinished(int uid)
    {
        for (int i = 0; i < finishedLevels.Count; i++)
        {
            if (finishedLevels[i] == uid)
            {
                //Debug.Log(string.Format("check level UID {0} in finished level, return true", uid));
                return false;
            }
        }
        finishedLevels.Add(uid);
        return true;
    }
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
    public bool isLevelPlayable(int uid)
    {
        if(uid % 100 == 1 && isThemeUnlocked(Mathf.CeilToInt(uid/100)))
        {
            //Debug.Log(string.Format("check level UID {0} in playable level, return true coz its first level", uid));
            return true;
        }
        for (int i = 0; i < finishedLevels.Count; i++)
        {
            if(uid == finishedLevels[i] + 1)
            {
                //Debug.Log(string.Format("check level UID {0} in playable level, return true coz {1} is finished", uid, finishedLevels[i]));
                return true;
            }
        }
        return false;
    }
    public bool addThemeUnlocked(int uid)
    {
        for (int i = 0; i < unlockedThemes.Count; i++)
        {
            if (unlockedThemes[i] == uid)
            {
                return false;
            }
        }
        unlockedThemes.Add(uid);
        return true;
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
