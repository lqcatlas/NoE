using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorNode : MonoBehaviour
{
    enum NodeStatus { locked = 1, unlocked = 2, finished = 3};
    [Header("Selector Logic")]
    public LevelSelector master;
    [SerializeField] string unlockLevelUID;
    [SerializeField] string targetLevelUID;

    [Header("Node Status")]
    
    [SerializeField] NodeStatus status = NodeStatus.locked;

    [Header("Children Objs")]
    [SerializeField] CircleCollider2D inputCollider;
    [SerializeField] SpriteRenderer fill;
    [SerializeField] SpriteRenderer frame;

    
    public void HoverOn()
    {
        if (status == NodeStatus.unlocked)
        {
            fill.DOFade(0.8f, dConstants.UI.StandardizedBtnAnimDuration);
            fill.transform.DOScale(1f, dConstants.UI.StandardizedBtnAnimDuration);
            frame.DOFade(0f, dConstants.UI.StandardizedBtnAnimDuration);
        }
        else if (status == NodeStatus.finished)
        {
            fill.DOFade(0.8f, dConstants.UI.StandardizedBtnAnimDuration/2);
        }
    }
    public void HoverOff()
    {
        if (status == NodeStatus.unlocked)
        {
            fill.DOFade(0f, dConstants.UI.StandardizedBtnAnimDuration);
            fill.transform.DOScale(0f, dConstants.UI.StandardizedBtnAnimDuration);
            frame.DOFade(1f, dConstants.UI.StandardizedBtnAnimDuration);
        }
        else if (status == NodeStatus.finished)
        {
            fill.DOFade(1f, dConstants.UI.StandardizedBtnAnimDuration/2);
        }
    }
    public void MouseUp()
    {
        if (status == NodeStatus.unlocked || status == NodeStatus.finished)
        {
            LevelLauncher.singleton.LaunchLevelByUID(targetLevelUID);
        }
    }
    public int CheckStatus()
    {
        if (master.playerLevelRecords.isLevelFinished(targetLevelUID))
        {
            SetToFinished();
            return 3;
        }
        else if (unlockLevelUID == "-1")
        {
            SetToUnlocked();
            return 2;
        }

        else if(master.playerLevelRecords.isLevelFinished(unlockLevelUID))
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
    void SetToFinished()
    {
        status = NodeStatus.finished;
        fill.gameObject.SetActive(true);
        fill.color = new Color(1f, 1f, 1f, 1f);
        frame.gameObject.SetActive(false);
        inputCollider.enabled = true;
    }
    void SetToLocked()
    {
        status = NodeStatus.locked;
        fill.gameObject.SetActive(false);
        frame.gameObject.SetActive(false);
        inputCollider.enabled = false;
    }
    void SetToUnlocked()
    {
        status = NodeStatus.unlocked;
        fill.gameObject.SetActive(true);
        fill.color = new Color(1f, 1f, 1f, 0f);
        fill.transform.localScale = Vector3.zero;
        frame.gameObject.SetActive(true);
        inputCollider.enabled = true;
    }
}
