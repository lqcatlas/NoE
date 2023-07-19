using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Rendering.Universal;

public class LevelSelector : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] bool unlockAll = false;

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
    [Header("Children Objs")]
    [SerializeField] Transform nodeParent;
    [SerializeField] List<SelectorNode> nodes;
    
    
    public void SelectorShow()
    {
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (unlockAll)
        {
            Debug_UnlockAllNodes();
            unlockAll = false;
        }
    }
    private void Start()
    {
        CollectAllNodes();
        NodeParentInit();
    }
    void NodeParentInit()
    {
        nodeParent.localPosition = Vector3.zero;
    }
    void CollectAllNodes()
    {
        nodes = nodeParent.GetComponentsInChildren<SelectorNode>().ToList();
        int LockCount = 0;
        int UnlockCount = 0;
        int FinishCount = 0;
        for (int i = 0; i < nodes.Count; i++)
        {
            nodes[i].master = this;
            int result = nodes[i].InitStatus();
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
        }
        Debug.Log(string.Format("level selector launched, with {0} level nodes loaded. {1} locked, {2} unlocked, {3} finished.", nodes.Count, LockCount, UnlockCount, FinishCount));
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
