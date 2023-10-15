using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Rendering.Universal;
using TMPro;

public class LevelSelector : MonoBehaviour
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
    [SerializeField] List<SelectorNode> nodes;
    [SerializeField] List<SelectorTheme> themes;
    [Header("Children Objs")]
    [SerializeField] GameObject page;
    [SerializeField] Transform nodeParent;
    [SerializeField] TextMeshPro tokenCount;
    


    public void GoToSelector()
    {
        for(int i = 0; i < themes.Count; i++)
        {
            themes[i].UpdateStatus();
        }
        for (int i = 0; i < nodes.Count; i++)
        {
            nodes[i].UpdateStatus();
        }
        page.SetActive(true);
    }
    public void CloseSelector()
    {
        page.SetActive(false);
    }

    private void Update()
    {
        if (unlockAll)
        {
            Debug_UnlockAllNodes();
            unlockAll = false;
        }
        if (getTokens)
        {
            TokenCountAdjust(20);
            getTokens = false;
        }
    }
    private void Start()
    {
        NodeParentInit();

        themes.Clear();
        nodes.Clear();

        CollectAllThemes();
        //CollectAllNodes();

        TokenCountAdjust(0);

        page.SetActive(false);
    }
    public void UnlockTheme(int themeIndex, int tokenCost)
    {
        if (!playerLevelRecords.isThemeUnlocked(themeIndex))
        {
            playerLevelRecords.unlockedThemes.Add(themeIndex);
        }
        TokenCountAdjust(-tokenCost);
    }
    public void FinishLevel(int levelUID)
    {
        if (!playerLevelRecords.isLevelFinished(levelUID))
        {
            playerLevelRecords.finishedLevels.Add(levelUID);
            TokenCountAdjust(1);
        }
    }
    void TokenCountAdjust(int count)
    {
        playerLevelRecords.tokens += count;
        if(playerLevelRecords.tokens < 0)
        {
            Debug.LogError(string.Format("Selector Token Count Reacn invalid number:{0}.", playerLevelRecords.tokens));
        }
        tokenCount.SetText(playerLevelRecords.tokens.ToString());
    }
    void NodeParentInit()
    {
        nodeParent.localPosition = new Vector3(-45f, 0f, 0f);
    }
    void CollectAllThemes()
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
    public void Debug_UnlockAllNodes()
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            nodes[i].Debug_UnlockNode();
        }
        Debug.Log(string.Format("{0} level nodes unlocked by debug", nodes.Count));
    }
}
