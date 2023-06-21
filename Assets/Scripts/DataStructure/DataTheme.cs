using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public struct LevelNode
{
    public int previousLevelUID;
    public DataLevel level;
}
public class DataTheme
{
    //Theme data includes initial states of all levels in this theme.
    //also includes the level unlock order between those levels
    //also includes unlock order between themes
    //Theme data should be static

    //an unique ID to locate the theme among all themes
    public int themeUID;
    //theme UID that unlock this theme
    public int previousThemeUID;
    //level structure inside this theme
    public List<LevelNode> levels;
    //theme-specific master script that overwrite original levelmaster
    public LevelMasterBase themeMaster;

    //below are player-facing values
    //title of the theme. eg. Clock
    public string themeTitle;

    public DataTheme()
    {
        themeUID = -1;
        previousThemeUID = -1;

        levels = new List<LevelNode>();
        themeMaster = null;

        themeTitle = "Title of a theme";
    }
    public DataTheme(DataTheme copyTheme)
    {
        themeUID = copyTheme.themeUID;
        themeUID = copyTheme.previousThemeUID;

        levels = copyTheme.levels.ToList();
        themeMaster = copyTheme.themeMaster;

        themeTitle = copyTheme.themeTitle;
    }
}
