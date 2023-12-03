using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Load_Levels : MonoBehaviour
{
    public CSVSheetData dataSheet = new CSVSheetData();
    [SerializeField] string filename;
    public List<SheetItem_LevelSetup> levels;
#if UNITY_EDITOR
    private void Start()
    {
        dataSheet.sheet = CSVReader.Read("csv/" + filename);
        dataSheet.sheetName = filename;
        levels = new List<SheetItem_LevelSetup>();
        levels.Clear();
        levels.AddRange(Resources.LoadAll<SheetItem_LevelSetup>("dataFromCSV").ToList());
        //read data from csv for all level assets, only overwrite valid data
        for(int i = 0; i < levels.Count; i++)
        {
            UpdateLevelDataFromCSV(levels[i], dataSheet);
        }
        //doing a 2nd pass to generate previous level narratives and next level reference
        for (int i = 0; i < levels.Count; i++)
        {
            for(int j = 0; j < levels.Count; j++)
            {
                if (levels[i].nextLevelIndex == levels[j].levelUID)
                {
                    levels[i].nextLevel = levels[j];
                    levels[j].previousLevel = levels[i];
                }
            }
        }
        for (int i = 0; i < levels.Count; i++)
        {
            levels[i].oldNarratives.Clear();
        }
        for (int i = 0; i < levels.Count; i++)
        {
            //levels[i].oldNarratives.Clear();
            if(levels[i].previousLevel != null)
            {
                levels[i].oldNarratives.AddRange(levels[i].previousLevel.oldNarratives);
                levels[i].oldNarratives.Add(levels[i].previousLevel.newInitNarrative);
                levels[i].oldNarratives.AddRange(levels[i].previousLevel.newPlayNarratives);
            }
            EditorUtility.SetDirty(levels[i]);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    void UpdateLevelDataFromCSV(SheetItem_LevelSetup level, CSVSheetData csv)
    {
        Dictionary<string, object> levelDic = csv.GetLineByUID(level.levelUID);
        if(levelDic == null)
        {
            return;
        }
        else
        {
            //Debug.Log(string.Format("loading data from csv for level({0})", level.levelUID));
        }
        object result = null;
        result = null;
        if (levelDic.TryGetValue("themeindex", out result))
        {
            //Debug.Log(result);
            int.TryParse((string)result, out level.themeIndex);
        }
        result = null;
        if (levelDic.TryGetValue("levelindex", out result))
        {
            int.TryParse((string)result, out level.levelIndex);
        }
        result = null;
        if (levelDic.TryGetValue("nextlevelindex", out result))
        {
            int.TryParse((string)result, out level.nextLevelIndex);
        }
        result = null;
        if (levelDic.TryGetValue("themename", out result))
        {
            if (((string)result).Length > 0)
            {
                level.theme = (string)result;
            }
        }
        result = null;
        if (levelDic.TryGetValue("levelname", out result))
        {
            if (((string)result).Length > 0)
            {
                level.title = (string)result;
            }
        }
        result = null;
        if (levelDic.TryGetValue("init", out result))
        {
            if (((string)result).Length > 0)
            {
                level.newInitNarrative = (string)result;
            }
        }
        result = null;
        level.newPlayNarratives.Clear();
        if (levelDic.TryGetValue("play1", out result))
        {
            if (((string)result).Length > 0)
            {
                level.newPlayNarratives.Add((string)result);
            }
        }
        result = null;
        if (levelDic.TryGetValue("play2", out result))
        {
            if (((string)result).Length > 0)
            {
                level.newPlayNarratives.Add((string)result);
            }
        }
        result = null;
        if (levelDic.TryGetValue("play3", out result))
        {
            if (((string)result).Length > 0)
            {
                level.newPlayNarratives.Add((string)result);
            }
        }
        result = null;
        if (levelDic.TryGetValue("goal", out result))
        {
            if (((string)result).Length > 0)
            {
                level.goal = (string)result;
            }
        }
        result = null;
        if (levelDic.TryGetValue("ruletag1", out result))
        {
            if (((string)result).Length > 0)
            {
                object additionalResult;
                levelDic.TryGetValue("rule1", out additionalResult);
                RuleItem item = new RuleItem((string)result, (string)additionalResult);
                level.ruleset.Clear();
                level.ruleset.Add(item);
            }
        }
        result = null;
        if (levelDic.TryGetValue("ruletag2", out result))
        {
            if (((string)result).Length > 0)
            {
                object additionalResult;
                levelDic.TryGetValue("rule2", out additionalResult);
                RuleItem item = new RuleItem((string)result, (string)additionalResult);
                level.ruleset.Add(item);
            }
        }
            result = null;
        if (levelDic.TryGetValue("ruletag3", out result))
        {
            if (((string)result).Length > 0)
            {
                object additionalResult;
                levelDic.TryGetValue("rule3", out additionalResult);
                RuleItem item = new RuleItem((string)result, (string)additionalResult);
                level.ruleset.Add(item);
            }
        }
        result = null;
        if (levelDic.TryGetValue("ruletag4", out result))
        {
            if (((string)result).Length > 0)
            {
                object additionalResult;
                levelDic.TryGetValue("rule4", out additionalResult);
                RuleItem item = new RuleItem((string)result, (string)additionalResult);
                level.ruleset.Add(item);
            }
        }
        result = null;
        if (levelDic.TryGetValue("ruletag5", out result))
        {
            if (((string)result).Length > 0)
            {
                object additionalResult;
                levelDic.TryGetValue("rule5", out additionalResult);
                RuleItem item = new RuleItem((string)result, (string)additionalResult);
                level.ruleset.Add(item);
            }
        }

    }
#endif
}
