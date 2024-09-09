using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "CSVData/ThemeSetup")]
public class SheetItem_ThemeSetup : ScriptableObject
{
    [Header("IDs")]
    //an unique ID to locate the theme among all themes
    public int themeUID;
    [Header("unlocks")]
    //star cost before the theme can be unlocked 
    public int unlockPrereq;
    //star cost to unlock the theme
    public int unlockCost;

    [Header("player-facing texts")]
    //below are player-facing values
    public string themeTitle; //title of the theme. eg. Clock
    public string lockedLine; //line used in selector when the theme in locked.
    public string unlockedLine; //line used in selector when the theme in unlocked.
    public string hint; //line used in hidden object scene to find the theme object.
    public List<int> tags; //tags on theme photo  
    public string date; //date of the theme is finished 
    public string manifesto; //manifesto about the design of the theme
    public string prompt; //prompt about the design of the theme
    public List<string> narratives; //narrative lines used in levels

    [Header("levels")]
    public List<SheetItem_LevelSetup> levels;
    public int TotalStars;
    public int TotalGems;
    /*
    //gameplay page for hidden object 
    public GameObject objectScene;
    //theme-specific master script that overwrite original levelmaster
    public LevelMasterBase themeMaster;
    */
}
