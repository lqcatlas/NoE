using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Rendering.Universal;

public class LevelSelector : MonoBehaviour
{
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
    public LevelRecords playerLevelRecords;
    [SerializeField] Transform nodeParent;
    [SerializeField] List<SelectorNode> nodes;

    private void Start()
    {
        nodes = nodeParent.GetComponentsInChildren<SelectorNode>().ToList();
        int LockCount = 0;
        int UnlockCount = 0;
        int FinishCount = 0;
        for(int i = 0; i < nodes.Count; i++)
        {
            nodes[i].master = this;
            int result = nodes[i].CheckStatus();
            if(result == 1)
            {
                LockCount += 1;
            }
            else if(result == 2)
            {
                UnlockCount += 1;
            }
            else if(result == 3)
            {
                FinishCount += 1;
            }
        }
        Debug.Log(string.Format("level selector launched, with {0} level nodes loaded. {1} locked, {2} unlocked, {3} finished.", nodes.Count, LockCount, UnlockCount, FinishCount));
    }
}
