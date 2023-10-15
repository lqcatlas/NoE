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

    [Header("Selector Data")]
    [SerializeField] float NodeStartDegree = 0f;
    [SerializeField] List<Sprite> ThemeSpriteByStatus;
    [SerializeField] List<string> ThemeDescByStatus;
    
    [Header("Children")]
    [SerializeField] ObjFloating FloatingGroup;
    [SerializeField] ObjSwinging SwingingGroup;
    [SerializeField] Transform NodesParent;

    [Header("Theme")]
    [SerializeField] SpriteRenderer themeFrame;
    [SerializeField] SpriteRenderer themeBackground;
    [SerializeField] SpriteRenderer themeIcon;
    [SerializeField] TextMeshPro themeDesc;

    [Header("Tokens")]
    [SerializeField] SpriteRenderer tokenFrame;
    [SerializeField] SpriteRenderer tokenIcon;
    [SerializeField] TextMeshPro tokenNeed;

    
    public void HoverOn()
    {
        if (status == ThemeStatus.locked)
        {
            if (isUnlockable())
            {
                tokenIcon.DOFade(0.5f, dConstants.UI.StandardizedBtnAnimDuration);
                themeFrame.DOColor(dConstants.UI.DefaultColor_3rd, dConstants.UI.StandardizedBtnAnimDuration);
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
                themeFrame.DOColor(dConstants.UI.DefaultColor_4th, dConstants.UI.StandardizedBtnAnimDuration);
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
    public void UpdateStatus()
    {
        //title.SetText(LocalizedAssetLookup.singleton.Translate(setupData.title));
        if (master.playerLevelRecords.isThemeUnlocked(themeIndex))
        {
            //finished status check TBD
            SetToUnlocked();
        }
        else
        {
            SetToLocked();
        }
    }
    public void AnimateToPopup()
    {
        FloatingGroup.transform.DOScale(0f, dConstants.UI.StandardizedBtnAnimDuration).From();
        gameObject.SetActive(true);
    }
    public List<SelectorNode> CollectMyNodes()
    {
        nodes = NodesParent.GetComponentsInChildren<SelectorNode>(true).ToList();
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

        themeFrame.gameObject.SetActive(true);
        themeFrame.color = dConstants.UI.DefaultColor_4th;
        themeBackground.gameObject.SetActive(true);
        themeBackground.color = dConstants.UI.DefaultColor_4th;
        themeDesc.SetText(LocalizedAssetLookup.singleton.Translate(ThemeDescByStatus[0]));
        themeDesc.color = dConstants.UI.DefaultColor_3rd;
        themeIcon.sprite = ThemeSpriteByStatus[0];
        themeIcon.color = dConstants.UI.DefaultColor_3rd;

        tokenFrame.gameObject.SetActive(true);
        tokenFrame.color = dConstants.UI.DefaultColor_1st;
        tokenIcon.gameObject.SetActive(true);
        tokenIcon.color = new Color(dConstants.UI.DefaultColor_1st.r, dConstants.UI.DefaultColor_1st.g, dConstants.UI.DefaultColor_1st.b, 0f);
        tokenNeed.gameObject.SetActive(true);
        tokenNeed.color = dConstants.UI.DefaultColor_1st;
        tokenNeed.SetText(UnlockTokenRequired.ToString());

        FloatingGroup.enabled = false;
        SwingingGroup.enabled = false;
    }
    void SetToUnlocked()
    {
        status = ThemeStatus.unlocked;

        themeFrame.gameObject.SetActive(false);
        themeBackground.gameObject.SetActive(false);
        themeDesc.SetText(LocalizedAssetLookup.singleton.Translate(ThemeDescByStatus[1]));
        themeDesc.color = dConstants.UI.DefaultColor_2nd;
        themeIcon.sprite = ThemeSpriteByStatus[1];
        themeIcon.color = dConstants.UI.DefaultColor_1st;

        tokenFrame.gameObject.SetActive(false);
        tokenIcon.gameObject.SetActive(false);
        tokenNeed.gameObject.SetActive(false);

        FloatingGroup.enabled = true;
        SwingingGroup.enabled = true;
    }
    void AnimateToUnlocked()
    {
        themeFrame.DOFade(0f, dConstants.UI.StandardizedBtnAnimDuration);
        themeBackground.DOFade(0f, dConstants.UI.StandardizedBtnAnimDuration);
        themeDesc.DOFade(0f, dConstants.UI.StandardizedBtnAnimDuration / 2f).OnComplete(()=> themeDesc.SetText(LocalizedAssetLookup.singleton.Translate(ThemeDescByStatus[1])));
        themeDesc.DOFade(1f, dConstants.UI.StandardizedBtnAnimDuration / 2f).SetDelay(dConstants.UI.StandardizedBtnAnimDuration / 2f);
        tokenFrame.DOFade(0f, dConstants.UI.StandardizedBtnAnimDuration);
        tokenIcon.DOFade(0f, dConstants.UI.StandardizedBtnAnimDuration);
        tokenNeed.DOFade(0f, dConstants.UI.StandardizedBtnAnimDuration);
        themeIcon.DOColor(dConstants.UI.DefaultColor_1st, dConstants.UI.StandardizedBtnAnimDuration).OnComplete(() => SetToUnlocked());
        for (int i = 0; i < nodes.Count; i++)
        {
            nodes[i].AnimateToLocked();
        }
    }

    void SetToFinished()
    {
        status = ThemeStatus.finished;

        themeFrame.gameObject.SetActive(false);
        themeBackground.gameObject.SetActive(false);
        themeDesc.SetText(LocalizedAssetLookup.singleton.Translate(ThemeDescByStatus[1]));
        themeDesc.color = dConstants.UI.DefaultColor_2nd;
        themeIcon.sprite = ThemeSpriteByStatus[1];
        themeIcon.color = dConstants.UI.DefaultColor_1st;

        tokenFrame.gameObject.SetActive(false);
        tokenIcon.gameObject.SetActive(false);
        tokenNeed.gameObject.SetActive(false);

        FloatingGroup.enabled = true;
        SwingingGroup.enabled = true;
        
    }
    void NodeReposition()
    {
        int maxNodeCount = 14;
        float radius = 8.5f;
        float offsetPerNode = Mathf.PI * 2f / maxNodeCount;
        float startOffset = Mathf.PI * 2f * (NodeStartDegree / 360f);

        for (int i = 0; i < nodes.Count; i++)
        {
            float finalDegree = startOffset - offsetPerNode * i;
            //Debug.Log(string.Format("angle at {0}", finalDegree));
            nodes[i].transform.localPosition = radius * new Vector3(Mathf.Cos(finalDegree), Mathf.Sin(finalDegree), 0);
        }
    }
}
