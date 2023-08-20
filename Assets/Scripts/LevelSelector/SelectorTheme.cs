using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class SelectorTheme : MonoBehaviour
{
    public enum ThemeStatus { locked = 1, unlocked = 2, finished = 3 };

    [Header("Gameplay Data")]
    public LevelSelector master;
    public ThemeStatus status = ThemeStatus.locked;
    [SerializeField] int UnlockTokenRequired = dConstants.Gameplay.DefaultThemeUnlockTokenRequirement;
    [SerializeField] int themeIndex;
    [SerializeField] List<SelectorNode> nodes;

    [Header("Children Objs")]
    [SerializeField] Transform NodesParent;
    [SerializeField] SpriteRenderer frame;
    [SerializeField] SpriteRenderer tokenFrame;
    [SerializeField] TextMeshPro title;
    [SerializeField] SpriteRenderer tokenIcon;
    [SerializeField] TextMeshPro tokenNeed;
    public void HoverOn()
    {
        if (status == ThemeStatus.locked)
        {
            if (isUnlockable())
            {
                tokenIcon.DOFade(0.5f, dConstants.UI.StandardizedBtnAnimDuration);
            }
        }
    }
    public void HoverOff()
    {
        if (status == ThemeStatus.locked)
        {
            if (isUnlockable())
            {
                tokenIcon.DOFade(0f, dConstants.UI.StandardizedBtnAnimDuration);
            }
        }
    }
    public void MouseUp()
    {
        if (status == ThemeStatus.locked)
        {
            if (isUnlockable())
            {
                master.UnlockTheme(themeIndex, UnlockTokenRequired);
                AnimateToUnlocked();
                UnlockDefaultLevels();
            }
        }
        if(status == ThemeStatus.unlocked || status == ThemeStatus.finished)
        {
            //to do: manifesto
        }
    }
    public int InitStatus()
    {
        //title.SetText(LocalizedAssetLookup.singleton.Translate(setupData.title));
        if (master.playerLevelRecords.isThemeUnlocked(themeIndex))
        {
            //finished status check TBD
            SetToUnlocked();
            return 2;
        }
        else
        {
            SetToLocked();
            return 1;
        }
    }
    public List<SelectorNode> CollectMyNodes()
    {
        nodes = NodesParent.GetComponentsInChildren<SelectorNode>().ToList();
        int LockCount = 0;
        int UnlockCount = 0;
        int FinishCount = 0;
        for (int i = 0; i < nodes.Count; i++)
        {
            nodes[i].master = master;
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
        NodeReposition();
        return nodes;
        //Debug.Log(string.Format("level selector launched, with {0} level nodes loaded. {1} locked, {2} unlocked, {3} finished.", nodes.Count, LockCount, UnlockCount, FinishCount));
    }
    bool isUnlockable()
    {
        if (master != null)
        {
            return master.playerLevelRecords.tokens >= UnlockTokenRequired;
        }
        else
        {
            return false;
        }
    }
    void SetToLocked()
    {
        status = ThemeStatus.locked;
        title.gameObject.SetActive(false);
        
        tokenFrame.gameObject.SetActive(true);
        tokenFrame.color = new Color(1f, 1f, 1f, 1f);
        tokenIcon.gameObject.SetActive(true);
        tokenIcon.color = new Color(1f, 1f, 1f, 0f);
        tokenNeed.gameObject.SetActive(true);
        tokenNeed.color = new Color(1f, 1f, 1f, 1f);
        tokenNeed.SetText(UnlockTokenRequired.ToString());
    }
    void SetToUnlocked()
    {
        status = ThemeStatus.unlocked;
        title.gameObject.SetActive(true);

        tokenFrame.gameObject.SetActive(false);
        tokenIcon.gameObject.SetActive(false);
        tokenNeed.gameObject.SetActive(false);
    }
    void AnimateToUnlocked()
    {
        title.DOFade(0f, dConstants.UI.StandardizedBtnAnimDuration).From().OnComplete(()=> SetToUnlocked());
        tokenFrame.DOFade(0f, dConstants.UI.StandardizedBtnAnimDuration);
        tokenIcon.DOFade(0f, dConstants.UI.StandardizedBtnAnimDuration);
        tokenNeed.DOFade(0f, dConstants.UI.StandardizedBtnAnimDuration);
    }

    void SetToFinished()
    {
        //for now same as unlocked, manifesto to be added
        SetToUnlocked();
        status = ThemeStatus.finished;
    }
    void UnlockDefaultLevels()
    {
        //Debug.Log("execute unlock default level ()");
        for(int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].setupData.previousLevel == null)
            {
                nodes[i].UnlockLevel();
            }
        }
    }
    void NodeReposition()
    {
        int minNodeCount = 10;
        float radius = 7f;
        float degree = Mathf.PI * 2 / Mathf.Max(nodes.Count, minNodeCount);
        float startDegree = Mathf.PI / 3;
        for(int i = 0; i < nodes.Count; i++)
        {
            float finalDegree = startDegree - degree * i;
            Debug.Log(string.Format("angle at {0}", finalDegree));
            nodes[i].transform.localPosition = radius * new Vector3(Mathf.Cos(finalDegree), Mathf.Sin(finalDegree), 0);
        }
    }
}
