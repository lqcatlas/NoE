using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEditor;
using UnityEngine;

public class Load_Themes : MonoBehaviour
{
    public CSVSheetData dataSheet = new CSVSheetData();
    [SerializeField] string filename_theme;
    [SerializeField] string filename_narrative;
    [SerializeField] string filename_level;
    public List<SheetItem_ThemeSetup> themes;
    public List<SheetItem_LevelSetup> levels;
#if UNITY_EDITOR
    private void Start()
    {
        //load theme csv
        dataSheet.sheet = CSVReader.Read("csv/" + filename_theme);
        dataSheet.sheetName = filename_theme;
        themes = new List<SheetItem_ThemeSetup>();
        themes.Clear();
        themes.AddRange(Resources.LoadAll<SheetItem_ThemeSetup>("dataFromCSV").ToList());
        
        //read data from csv for all theme assets, only overwrite valid data
        for (int i = 0; i < themes.Count; i++)
        {
            UpdateThemeDataFromCSV(themes[i], dataSheet);
            EditorUtility.SetDirty(themes[i]);
        }
        //load narrative data
        dataSheet.sheet = CSVReader.Read("csv/" + filename_narrative);
        dataSheet.sheetName = filename_narrative;
        UpdateNarrativceDataFromCSV(ref themes, dataSheet);
        
        Debug.Log(string.Format("theme setup data loaded. total of {0} theme processed", themes.Count));

        //load level data
        dataSheet.sheet = CSVReader.Read("csv/" + filename_level);
        dataSheet.sheetName = filename_level;
        levels = new List<SheetItem_LevelSetup>();
        levels.Clear();
        levels.AddRange(Resources.LoadAll<SheetItem_LevelSetup>("dataFromCSV").ToList());
        //read data from csv for all level assets, only overwrite valid data
        for (int i = 0; i < levels.Count; i++)
        {
            UpdateLevelDataFromCSV(levels[i], dataSheet);
            RegisterLevelOnTheme(ref themes, levels[i]);
        }
        //doing a 2nd pass to generate previous level narratives and next level reference
        for (int i = 0; i < levels.Count; i++)
        {
            for (int j = 0; j < levels.Count; j++)
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
            if (levels[i].previousLevel != null)
            {
                levels[i].oldNarratives.AddRange(levels[i].previousLevel.oldNarratives);
                if(levels[i].previousLevel.newInitNarrative != null)
                {
                    levels[i].oldNarratives.Add(levels[i].previousLevel.newInitNarrative);
                }
                levels[i].oldNarratives.AddRange(levels[i].previousLevel.newPlayNarratives);
            }
            EditorUtility.SetDirty(levels[i]);
        }
        Debug.Log(string.Format("level setup data loaded. total of {0} level processed", levels.Count));
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    void UpdateThemeDataFromCSV(SheetItem_ThemeSetup theme, CSVSheetData csv)
    {
        Dictionary<string, object> csvRow = csv.GetLineByUID(theme.themeUID);
        if (csvRow == null)
        {
            return;
        }
        else
        {
            //Debug.Log(string.Format("loading data from csv for level({0})", level.levelUID));
        }
        object result = null;
        result = null;
        if (csvRow.TryGetValue("themename", out result))
        {
            if (((string)result).Length > 0)
            {
                theme.themeTitle = (string)result;
            }
        }
        result = null;
        if (csvRow.TryGetValue("requirement", out result))
        {
            //Debug.Log(result);
            int.TryParse((string)result, out theme.unlockPrereq);
        }
        result = null;
        if (csvRow.TryGetValue("cost", out result))
        {
            //Debug.Log(result);
            int.TryParse((string)result, out theme.unlockCost);
        }
        result = null;
        if (csvRow.TryGetValue("locked", out result))
        {
            if (((string)result).Length > 0)
            {
                theme.lockedLine = (string)result;
            }
        }
        result = null;
        if (csvRow.TryGetValue("unlocked", out result))
        {
            if (((string)result).Length > 0)
            {
                theme.unlockedLine = (string)result;
            }
        }
        result = null;
        if (csvRow.TryGetValue("hint", out result))
        {
            if (((string)result).Length > 0)
            {
                theme.hint = (string)result;
            }
        }
        result = null;
        if (csvRow.TryGetValue("date", out result))
        {
            if (((string)result).Length > 0)
            {
                theme.date = (string)result;
            }
        }
        result = null;
        if (csvRow.TryGetValue("manifesto", out result))
        {
            if (((string)result).Length > 0)
            {
                theme.manifesto = (string)result;
            }
        }
        result = null;
        if (csvRow.TryGetValue("prompt", out result))
        {
            if (((string)result).Length > 0)
            {
                theme.prompt = (string)result;
            }
        }
        theme.narratives.Clear();
        theme.levels.Clear();
        theme.TotalStars = 0;
        theme.TotalGems = 0;
    }
    void UpdateNarrativceDataFromCSV(ref List<SheetItem_ThemeSetup> themes, CSVSheetData csv)
    {
        for (int i = 0; i < csv.sheet.Count; i++)
        {
            Dictionary<string, object> csvRow = csv.GetLineByRow(i);
            if (csvRow == null)
            {
                return;
            }
            else
            {
                //Debug.Log(string.Format("loading data from csv for level({0})", level.levelUID));
            }
            object result = null;
            int targetThemeUID = 0;
            SheetItem_ThemeSetup targetTheme = null;
            result = null;
            if (csvRow.TryGetValue("themeindex", out result))
            {
                if (((string)result).Length > 0)
                {
                    int.TryParse((string)result, out targetThemeUID);
                }
            }
            if (targetThemeUID > 0)
            {
                for (int j = 0; j < themes.Count; j++)
                {
                    if (themes[j].themeUID == targetThemeUID)
                    {
                        targetTheme = themes[j];
                    }
                }
            }
            if (targetTheme == null)
            {
                return;
            }
            else
            {
                result = null;
                if (csvRow.TryGetValue("text", out result))
                {
                    if (((string)result).Length > 0)
                    {
                        targetTheme.narratives.Add((string)result);
                    }
                }
            }
        }
    }
    void UpdateLevelDataFromCSV(SheetItem_LevelSetup level, CSVSheetData csv)
    {
        Dictionary<string, object> levelDic = csv.GetLineByUID(level.levelUID);
        if (levelDic == null)
        {
            return;
        }
        else
        {
            //Debug.Log(string.Format("loading data from csv for level({0})", level.levelUID));
        }
        object result = null;
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
        int line = 0;
        level.newInitNarrative = null;
        if (levelDic.TryGetValue("init", out result))
        {
            if (((string)result).Length > 0)
            {
                int.TryParse((string)result, out line);
                level.newInitNarrative = GetNarrativeString(level.themeIndex, line);
            }
        }
        result = null;
        line = 0;
        level.newPlayNarratives.Clear();
        if (levelDic.TryGetValue("play1", out result))
        {
            if (((string)result).Length > 0)
            {
                int.TryParse((string)result, out line);
                level.newPlayNarratives.Add(GetNarrativeString(level.themeIndex, line));
            }
        }
        result = null;
        line = 0;
        if (levelDic.TryGetValue("play2", out result))
        {
            if (((string)result).Length > 0)
            {
                int.TryParse((string)result, out line);
                level.newPlayNarratives.Add(GetNarrativeString(level.themeIndex, line));
            }
        }
        result = null;
        line = 0;
        if (levelDic.TryGetValue("play3", out result))
        {
            if (((string)result).Length > 0)
            {
                int.TryParse((string)result, out line);
                level.newPlayNarratives.Add(GetNarrativeString(level.themeIndex, line));
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
    string GetNarrativeString(int themeUID, int line)
    {
        for(int i = 0; i < themes.Count; i++)
        {
            if (themes[i].themeUID == themeUID)
            {
                if(line <= themes[i].narratives.Count)
                {
                    return themes[i].narratives[line - 1];
                }
            }
        }
        return "missing narrative line";
    }
    bool RegisterLevelOnTheme(ref List<SheetItem_ThemeSetup> themes, SheetItem_LevelSetup level)
    {
        for(int i = 0; i < themes.Count; i++)
        {
            if (themes[i].themeUID == level.themeIndex)
            {
                themes[i].levels.Add(level);
                if (level.isHard)
                {
                    themes[i].TotalGems += 1;
                }
                else
                {
                    themes[i].TotalStars += 1;
                }
                return true;
            }
        }
        return false;
    }
#endif
}
