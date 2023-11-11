using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "PlayingData/LevelRecords")]
public class LevelRecords : ScriptableObject, ISaveData
{
    public int tokens;
    public List<int> finishedLevels;
    public List<int> unlockedThemes;

    private const string TOKEN_SAVE_KEY = "record.tokens";
    private const string LEVEL_SAVE_KEY = "record.finishedLevels";
    private const string THEME_SAVE_KEY = "record.unlockedThemes";
    public void LoadFromSaveManager()
    {
        string str = SaveManager.controller.Inquire(string.Format(TOKEN_SAVE_KEY));
        if (str != null)
        {
            int.TryParse(SaveManager.controller.Inquire(string.Format(TOKEN_SAVE_KEY)), out tokens);
        }
        str = SaveManager.controller.Inquire(string.Format(LEVEL_SAVE_KEY));
        if (str != null)
        {
            finishedLevels.Clear();
            //convert into level IDs
            List<string> level_str = str.Split('|').ToList();
            for (int i = 0; i < level_str.Count; i++)
            {
                int.TryParse(level_str[i], out int uid);
                finishedLevels.Add(uid);
            }
        }
        str = SaveManager.controller.Inquire(string.Format(THEME_SAVE_KEY));
        if (str != null)
        {
            unlockedThemes.Clear();
            //convert into theme IDs
            List<string> theme_str = str.Split('|').ToList();
            for (int i = 0; i < theme_str.Count; i++)
            {
                int.TryParse(theme_str[i], out int uid);
                unlockedThemes.Add(uid);
            }
        }
        //Debug.Log(string.Format("selector load data from file, tokens:{0}, ", playerLevelRecords.tokens));
    }
    public void SaveToSaveManager()
    {
        //Debug.Log(string.Format("selector save data to file, tokens:{0}", playerLevelRecords.tokens));
        if (finishedLevels.Count > 0)
        {
            //convert level ids into a string
            string str = string.Join('|', finishedLevels);
            SaveManager.controller.Insert(string.Format(LEVEL_SAVE_KEY), str);
        }
        if (unlockedThemes.Count > 0)
        {
            //convert theme ids into a string
            string str = string.Join('|', unlockedThemes);
            SaveManager.controller.Insert(string.Format(THEME_SAVE_KEY), str);
        }
        SaveManager.controller.Insert(string.Format(TOKEN_SAVE_KEY), tokens.ToString());
    }

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
