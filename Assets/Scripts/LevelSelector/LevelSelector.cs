using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Rendering.Universal;
using TMPro;
using DG.Tweening;

public class LevelSelector : MonoBehaviour, ISaveData
{
    [Header("Debug")]
    [SerializeField] bool unlockAll = false;
    [SerializeField] bool getTokens = false;

    static public LevelSelector singleton;
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
    }
    [Header("Player Data")]
    public LevelRecords playerLevelRecords;
    [Header("Gameplay Data")]
    //[SerializeField] List<SelectorNode> nodes;
    //[SerializeField] List<SelectorTheme> themes;
    //public ThemeResourceLookup themeResourceLookup;
    [SerializeField] List<ThemePhotoGroup> photos;
    [Header("Children Objs")]
    [SerializeField] GameObject page;
    [SerializeField] Transform nodeParent;
    [SerializeField] TextMeshPro curStarCount;
    [SerializeField] TextMeshPro gemCount;
    public MsgBox DesignerNoteBox;

    public void GoToSelector()
    {

        /*for (int i = 0; i < nodes.Count; i++)
        {
            nodes[i].UpdateStatus();
        }
        for (int i = 0; i < themes.Count; i++)
        {
            themes[i].UpdateStatus();
        }*/
        for (int i = 0; i < photos.Count; i++)
        {

            photos[i].UpdatePhotoGroup();
        }
        page.SetActive(true);
        BgCtrl.singleton.SetToPhase(dConstants.Gameplay.GamePhase.Selector);

        //vfx
        /*for (int i = 0; i < themes.Count; i++)
        {
            themes[i].gameObject.SetActive(false);
        }
        Sequence seq = DOTween.Sequence();
        for (int i = 0; i < themes.Count; i++)
        {
            SelectorTheme temp = themes[i];
            seq.AppendCallback(() => temp.AnimateToPopup());
            seq.AppendInterval(0.15f);
        }*/
        //vfx end
    }
    public void CloseSelector()
    {
        page.SetActive(false);
    }
    public int LocateFirstUnlockableTheme()
    {
        int result = -1;
        /*for(int i = 0; i < themes.Count; i++)
        {
            if (themes[i].isUnlockable() && themes[i].status == SelectorTheme.ThemeStatus.locked)
            {
                result = i;
            }
        }*/
        return result;
    }
    private void Update()
    {
        if (unlockAll)
        {
            Debug_UnlockAllNodes();
            unlockAll = false;
        }
        if (Input.GetKeyUp(KeyCode.T)||getTokens)
        {
            TokenEarned(10);
            GemEarned(1);
            getTokens = false;
        }
    }
    private void Start()
    {
        InitSelector();
    }
    void InitSelector()
    {
        NodeParentInit();
        //themes.Clear();
        //nodes.Clear();
        photos.Clear();
        //CollectAllThemes();
        CollectAllPhotos();
        TokenEarned(0);
        GemEarned(0);
        page.SetActive(false);
    }
    public void UnlockTheme(int themeIndex, int tokenCost)
    {
        if (!playerLevelRecords.isThemeUnlocked(themeIndex))
        {
            playerLevelRecords.unlockedThemes.Add(themeIndex);
        }
        TokenSpent(tokenCost);
    }
    public void FinishLevel(int levelUID, bool isHard = false)
    {
        if (!playerLevelRecords.isLevelFinished(levelUID))
        {
            playerLevelRecords.finishedLevels.Add(levelUID);
            if (!isHard)
            {
                TokenEarned(1);
            }
            else
            {
                GemEarned(1);
            }
        }
    }
    void TokenEarned(int count)
    {
        playerLevelRecords.tokens += count;
        if (playerLevelRecords.tokens < 0)
        {
            Debug.LogError(string.Format("Selector Token Count Reach invalid number:{0}.", playerLevelRecords.tokens));
        }
        curStarCount.SetText(playerLevelRecords.tokens.ToString());
    }
    void TokenSpent(int count)
    {
        playerLevelRecords.tokens -= count;
        playerLevelRecords.spentTokens += count;
        if (playerLevelRecords.tokens < 0)
        {
            Debug.LogError(string.Format("Selector Token Count Reacn invalid number:{0}.", playerLevelRecords.tokens));
        }
        curStarCount.SetText(playerLevelRecords.tokens.ToString());
    }
    void GemEarned(int count)
    {
        playerLevelRecords.tokens += count;
        if (playerLevelRecords.tokens < 0)
        {
            Debug.LogError(string.Format("Selector Token Count Reach invalid number:{0}.", playerLevelRecords.tokens));
        }
        gemCount.SetText(playerLevelRecords.gems.ToString());
    }
    void NodeParentInit()
    {
        nodeParent.localPosition = new Vector3(-45f, 0f, 0f);
    }
    void CollectAllPhotos()
    {
        photos = nodeParent.GetComponentsInChildren<ThemePhotoGroup>(true).ToList();
        for(int i = 0; i < photos.Count; i++)
        {
            photos[i].UpdatePhotoGroup();
        }
    }
    /*void CollectAllThemes()
    {
        themes = nodeParent.GetComponentsInChildren<SelectorTheme>(true).ToList();
        int LockCount = 0;
        int UnlockCount = 0;
        int FinishCount = 0;
        for (int i = 0; i < themes.Count; i++)
        {
            themes[i].master = this;
            int result = themes[i].InitStatus();
            if (result == 1)
            {
                LockCount += 1;
            }
            else if (result == 2)
            {
                UnlockCount += 1;
            }
            else if (result == 3)
            {
                FinishCount += 1;
            }
            nodes.AddRange(themes[i].CollectMyNodes()); ;
        }
        Debug.Log(string.Format("Level Selector launched, with {0} themes loaded. {1} locked, {2} unlocked, {3} finished.", themes.Count, LockCount, UnlockCount, FinishCount));
        int NodeLockCount = 0;
        int NodeUnlockCount = 0;
        int NodeFinishCount = 0;
        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].status == SelectorNode.NodeStatus.locked)
            {
                NodeLockCount += 1;
            }
            else if (nodes[i].status == SelectorNode.NodeStatus.unlocked)
            {
                NodeUnlockCount += 1;
            }
            else if (nodes[i].status == SelectorNode.NodeStatus.finished)
            {
                NodeFinishCount += 1;
            }
        }
        Debug.Log(string.Format("Additional, with {0} levels loaded. {1} locked, {2} unlocked, {3} finished.", nodes.Count, NodeLockCount, NodeUnlockCount, NodeFinishCount));
    }
    */
    public void Debug_UnlockAllNodes()
    {
        /*for (int i = 0; i < nodes.Count; i++)
        {
            nodes[i].Debug_UnlockNode();
        }*/
        for (int i = 0; i < photos.Count; i++)
        {
            photos[i].curStatus = ThemePhotoGroup.ThemePhotoStatus.perfect;
            photos[i].UIUpdateBasedOnStatus();
        }
        Debug.Log(string.Format("{0} photos unlocked by debug", photos.Count));
    }
    #region save
    private const string TOKEN_SAVE_KEY = "record.tokens";
    private const string TOKEN_SPENT_SAVE_KEY = "record.spentTokens";
    private const string GEM_SAVE_KEY = "record.gems";
    private const string LEVEL_SAVE_KEY = "record.finishedLevels";
    private const string THEME_SAVE_KEY = "record.unlockedThemes";
    public void LoadFromSaveManager()
    {
        string str = SaveManager.controller.Inquire(string.Format(TOKEN_SAVE_KEY));
        if (str != null)
        {
            int.TryParse(SaveManager.controller.Inquire(string.Format(TOKEN_SAVE_KEY)), out playerLevelRecords.tokens);
        }
        else
        {
            playerLevelRecords.tokens = dConstants.Gameplay.DefaultInitialTokenCount;
        }
        str = SaveManager.controller.Inquire(string.Format(TOKEN_SPENT_SAVE_KEY));
        if (str != null)
        {
            int.TryParse(SaveManager.controller.Inquire(string.Format(TOKEN_SPENT_SAVE_KEY)), out playerLevelRecords.spentTokens);
        }
        else
        {
            playerLevelRecords.spentTokens = 0;
        }
        str = SaveManager.controller.Inquire(string.Format(GEM_SAVE_KEY));
        if (str != null)
        {
            int.TryParse(SaveManager.controller.Inquire(string.Format(GEM_SAVE_KEY)), out playerLevelRecords.gems);
        }
        else
        {
            playerLevelRecords.gems = 0;
        }

        str = SaveManager.controller.Inquire(string.Format(LEVEL_SAVE_KEY));
        playerLevelRecords.finishedLevels.Clear();
        if (str != null)
        {
            //playerLevelRecords.finishedLevels.Clear();
            //convert into level IDs
            List<string> level_str = str.Split('|').ToList();
            for (int i = 0; i < level_str.Count; i++)
            {
                int.TryParse(level_str[i], out int uid);
                playerLevelRecords.finishedLevels.Add(uid);
            }
        }
        str = SaveManager.controller.Inquire(string.Format(THEME_SAVE_KEY));
        playerLevelRecords.unlockedThemes.Clear();
        if (str != null)
        {
            //convert into theme IDs
            List<string> theme_str = str.Split('|').ToList();
            for (int i = 0; i < theme_str.Count; i++)
            {
                int.TryParse(theme_str[i], out int uid);
                playerLevelRecords.unlockedThemes.Add(uid);
            }
        }
        //playerLevelRecords.SetDirty();
        //Debug.Log(string.Format("selector load data from file, tokens:{0}, ", playerLevelRecords.tokens));
        InitSelector();
    }
    public void SaveToSaveManager()
    {
        //Debug.Log(string.Format("selector save data to file, tokens:{0}", playerLevelRecords.tokens));
        if (playerLevelRecords.finishedLevels.Count > 0)
        {
            //convert level ids into a string
            string str = string.Join('|', playerLevelRecords.finishedLevels);
            SaveManager.controller.Insert(string.Format(LEVEL_SAVE_KEY), str);
        }
        if (playerLevelRecords.unlockedThemes.Count > 0)
        {
            //convert theme ids into a string
            string str = string.Join('|', playerLevelRecords.unlockedThemes);
            SaveManager.controller.Insert(string.Format(THEME_SAVE_KEY), str);
        }
        SaveManager.controller.Insert(string.Format(TOKEN_SAVE_KEY), playerLevelRecords.tokens.ToString());
        SaveManager.controller.Insert(string.Format(TOKEN_SPENT_SAVE_KEY), playerLevelRecords.spentTokens.ToString());
        SaveManager.controller.Insert(string.Format(GEM_SAVE_KEY), playerLevelRecords.gems.ToString());
    }
    #endregion save
}
