using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class LevelLauncher : MonoBehaviour
{
    [Header("Debug Options")]
    public string LaunchLevelUID;
    public SheetItem_LevelSetup LevelSetup;
    public bool LAUNCH_IT;

    [Header("Data & Objs")]
    public ThemeResourceLookup lookupTable;
    public GameObject levelHolder;
    public GameObject levelPreset;
    
    private void Update()
    {
        if (LAUNCH_IT)
        {
            LAUNCH_IT = false;
            if(LaunchLevelUID != "")
            {
                LaunchLevelByUID(LaunchLevelUID);
            }
            else if(LevelSetup != null)
            {
                LaunchLevelBySheetItem(LevelSetup);
            }
            else
            {
                Debug.LogError(string.Format("empty levelUID and SetupData in Launcher!"));
            }
        }
    }
    void ClearExistingLevel()
    {
        foreach (Transform child in levelHolder.transform)
        {
            Destroy(child.gameObject);
        }
    }
    public void LaunchLevelByUID(string uid)
    {
        //TBD
        ClearExistingLevel();
        Debug.LogError("unfinished function launch by Level UID");
    }
    public void LaunchLevelBySheetItem(SheetItem_LevelSetup setupData)
    {
        ClearExistingLevel();
        Debug.Log(string.Format("Launch level ({0}) as theme ({1})", setupData.levelUID, setupData.theme));
        //clone a level preset to start setup
        GameObject levelObj = Instantiate(levelPreset, levelHolder.transform);
        //get correct master script and assign to the level object
        MonoScript master = lookupTable.GetThemeScript(setupData.themeIndex);
        if (master == null)
        {
            return;
        }
        else
        {
            levelObj.AddComponent(master.GetClass());
        }
        //get correct additional hub object. if exist, clone it and assign
        GameObject addition = lookupTable.GetThemeSpecialHub(setupData.themeIndex);
        if (addition != null)
        {
            addition = Instantiate(addition, levelObj.transform);  
        }
        levelObj.GetComponent<LevelMasterBase>().levelSetupData = LevelSetup;
        levelObj.GetComponent<LevelMasterBase>().ObjectInit(addition);
        levelObj.GetComponent<LevelMasterBase>().LevelInit();


    }
}
