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
    public LevelRecords playerRecords;
    public ThemeResourceLookup themeLookupTable;
    public GameObject levelHolder;
    public GameObject levelPreset;
    
    private void Update()
    {
        //debug tool
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
        //Debug.Log(string.Format("{0} level setups loaded from setup data folder", levelSetupTable.Count));
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
    public SheetItem_LevelSetup GetLevelSetupDataByUID(int uid)
    {
        for (int i = 0; i < levelSetupTable.Count; i++)
        {
            if (levelSetupTable[i].levelUID == uid)
            {
                return levelSetupTable[i];
            }
        }
        return null;
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
                BgCtrl.singleton.SetToPhase(dConstants.Gameplay.GamePhase.Level);
                if (!playerRecords.seenHiddenGemNotice && levelSetupTable[i].isHard)
                {
                    ShowHiddenGemLevelNotification();
                }
                return true;
            }
        }
        Debug.Log(string.Format("unable to find level setup data for levelUID {0}", uid));
        return false;
    }
    public void LaunchLevelBySheetItem(SheetItem_LevelSetup setupData)
    {
        ClearExistingLevel();
        Debug.Log(string.Format("Launch level ({0}) as theme ({1})", setupData.levelUID, LocalizedAssetLookup.singleton.Translate(setupData.theme)));
        //clone a level preset to start setup
        GameObject levelObj = Instantiate(levelPreset, levelHolder.transform);
        //get correct master script and assign to the level object
        //MonoScript master = themeLookupTable.GetThemeScript(setupData.themeIndex);
        string masterScriptName = themeLookupTable.GetThemeScriptName(setupData.themeIndex);
        if (masterScriptName == "")
        {
            return;
        }
        else
        {
            //levelObj.AddComponent(master.GetClass());
            Debug.Log(string.Format("get master script name as {0}", masterScriptName));
            levelObj.AddComponent(System.Type.GetType(masterScriptName));
        }
        HiddenObjectLauncher.singleton.LaunchBackgroundPage(setupData.themeIndex);
        //get correct additional hub object. if exist, clone it and assign
        GameObject addition = themeLookupTable.GetThemeSpecialHub(setupData.themeIndex);
        if (addition != null)
        {
            addition = Instantiate(addition, levelObj.transform);  
        }
        levelObj.GetComponent<LevelMasterBase>().levelSetupData = setupData;
        levelObj.GetComponent<LevelMasterBase>().ObjectInit(addition);
        levelObj.GetComponent<LevelMasterBase>().LevelInit();
    }
    void ShowHiddenGemLevelNotification()
    {
        playerRecords.seenHiddenGemNotice = true;
        string title = "@Loc=ui_hiddengem_notice_title@@";
        string desc = "@Loc=ui_hiddengem_notice_desc@@";
        MsgBox.singleton.ShowBox(title, desc, "", null);
    }
}
