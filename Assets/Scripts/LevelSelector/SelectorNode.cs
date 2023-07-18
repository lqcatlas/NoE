using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SelectorNode : MonoBehaviour
{
    enum NodeStatus { locked = 1, unlocked = 2, finished = 3};
    [Header("Selector Logic")]
    public LevelSelector master;
    public SheetItem_LevelSetup setupData;
    //[SerializeField] int unlockLevelUID;
    //[SerializeField] int targetLevelUID;

    [Header("Node Status")]
    
    [SerializeField] NodeStatus status = NodeStatus.locked;

    [Header("Children Objs")]
    [SerializeField] CircleCollider2D inputCollider;
    [SerializeField] SpriteRenderer fill;
    [SerializeField] SpriteRenderer frame;
    [SerializeField] TextMeshPro levelName;


    public void HoverOn()
    {
        if (status == NodeStatus.unlocked)
        {
            fill.DOFade(0.8f, dConstants.UI.StandardizedBtnAnimDuration);
            fill.transform.DOScale(1f, dConstants.UI.StandardizedBtnAnimDuration);
            frame.DOFade(0f, dConstants.UI.StandardizedBtnAnimDuration);
            levelName.DOFade(1f, dConstants.UI.StandardizedBtnAnimDuration);
        }
        else if (status == NodeStatus.finished)
        {
            fill.DOFade(0.8f, dConstants.UI.StandardizedBtnAnimDuration/2);
            levelName.DOFade(1f, dConstants.UI.StandardizedBtnAnimDuration/2);
        }
    }
    public void HoverOff()
    {
        if (status == NodeStatus.unlocked)
        {
            fill.DOFade(0f, dConstants.UI.StandardizedBtnAnimDuration);
            fill.transform.DOScale(0f, dConstants.UI.StandardizedBtnAnimDuration);
            frame.DOFade(1f, dConstants.UI.StandardizedBtnAnimDuration);
            levelName.DOFade(0f, dConstants.UI.StandardizedBtnAnimDuration);
        }
        else if (status == NodeStatus.finished)
        {
            fill.DOFade(1f, dConstants.UI.StandardizedBtnAnimDuration/2);
            levelName.DOFade(0f, dConstants.UI.StandardizedBtnAnimDuration / 2);
        }
    }
    public void MouseUp()
    {
        if (status == NodeStatus.unlocked || status == NodeStatus.finished)
        {
            LevelLauncher.singleton.LaunchLevelByUID(setupData.levelUID);
        }
    }
    public int InitStatus()
    {
        levelName.SetText(LocalizedAssetLookup.singleton.Translate(setupData.title));
        if (master.playerLevelRecords.isLevelFinished(setupData.levelUID))
        {
            SetToFinished();
            return 3;
        }
        else if (setupData.previousLevel == null)
        {
            SetToUnlocked();
            return 2;
        }

        else if(master.playerLevelRecords.isLevelFinished(setupData.previousLevel.levelUID))
        {
            SetToUnlocked();
            return 2;
        }
        else
        {
            SetToLocked();
            return 1;
        }
    }
    public void Debug_UnlockNode()
    {
        SetToUnlocked();
    }
    void SetToFinished()
    {
        status = NodeStatus.finished;
        fill.gameObject.SetActive(true);
        fill.color = new Color(1f, 1f, 1f, 1f);
        frame.gameObject.SetActive(false);
        inputCollider.enabled = true;
        levelName.color = new Color(0.5f, 0.5f, 0.5f, 1f);
    }
    void SetToLocked()
    {
        status = NodeStatus.locked;
        fill.gameObject.SetActive(false);
        frame.gameObject.SetActive(false);
        inputCollider.enabled = false;
        levelName.color = new Color(1f, 1f, 1f, 0f);
    }
    void SetToUnlocked()
    {
        status = NodeStatus.unlocked;
        fill.gameObject.SetActive(true);
        fill.color = new Color(1f, 1f, 1f, 0f);
        fill.transform.localScale = Vector3.zero;
        frame.gameObject.SetActive(true);
        inputCollider.enabled = true;
        levelName.color = new Color(1f, 1f, 1f, 0f);
    }
}
