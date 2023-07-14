using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class LevelLauncher : MonoBehaviour
{
    static public LevelLauncher singleton;
    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        }
        else
        {
            Destroy(this);
        }
        LoadAllLevelSetups();
    }
    [Header("Debug Options")]
    public int LaunchLevelUID;
    public SheetItem_LevelSetup LevelSetup;
    public bool LAUNCH_IT;

    [Header("Data & Objs")]
    public List<SheetItem_LevelSetup> levelSetupTable;
    public ThemeResourceLookup themeLookupTable;
    public GameObject levelHolder;
    public GameObject levelPreset;
    
    private void Update()
    {
        if (LAUNCH_IT)
        {
            LAUNCH_IT = false;
            if(LaunchLevelUID != 0)
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
    void LoadAllLevelSetups()
    {
        levelSetupTable = Resources.LoadAll<SheetItem_LevelSetup>("DataFromCSV/LevelSetup").ToList();
        Debug.Log(string.Format("{0} level setups loaded from setup data folder", levelSetupTable.Count));
    }
    void ClearExistingLevel()
    {
        foreach (Transform child in levelHolder.transform)
        {
            Destroy(child.gameObject);
        }
    }
    public bool CheckLevelSetupDataByUID(int uid)
    {
        for (int i = 0; i < levelSetupTable.Count; i++)
        {
            if (levelSetupTable[i].levelUID == uid)
            {
                //found = true;
                return true;
            }
        }
        return false;
    }
    public bool LaunchLevelByUID(int uid)
    {
        ClearExistingLevel();
        //bool found = false;
        for(int i = 0; i < levelSetupTable.Count; i++)
        {
            if (levelSetupTable[i].levelUID == uid)
            {
                //found = true;
                LaunchLevelBySheetItem(levelSetupTable[i]);
                return true;
            }
        }
        Debug.Log(string.Format("unable to find level setup data for levleUID {0}", uid));
        return false;
    }
    public void LaunchLevelBySheetItem(SheetItem_LevelSetup setupData)
    {
        ClearExistingLevel();
        Debug.Log(string.Format("Launch level ({0}) as theme ({1})", setupData.levelUID, LocalizedAssetLookup.singleton.Translate(setupData.theme)));
        //clone a level preset to start setup
        GameObject levelObj = Instantiate(levelPreset, levelHolder.transform);
        //get correct master script and assign to the level object
        MonoScript master = themeLookupTable.GetThemeScript(setupData.themeIndex);
        if (master == null)
        {
            return;
        }
        else
        {
            levelObj.AddComponent(master.GetClass());
        }
        //get correct additional hub object. if exist, clone it and assign
        GameObject addition = themeLookupTable.GetThemeSpecialHub(setupData.themeIndex);
        if (addition != null)
        {
            addition = Instantiate(addition, levelObj.transform);  
        }
        levelObj.GetComponent<LevelMasterBase>().levelSetupData = LevelSetup;
        levelObj.GetComponent<LevelMasterBase>().ObjectInit(addition);
        levelObj.GetComponent<LevelMasterBase>().LevelInit();


    }
}
