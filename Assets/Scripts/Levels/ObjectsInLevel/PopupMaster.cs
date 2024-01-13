using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupMaster : MonoBehaviour
{
    [Header("Children Objs")]
    public GameObject popupMask;
    public GameObject victoryPopupGroup;
    public GameObject failurePopupGroup;

    public GameObject rewardGroup;
    public GameObject starReward;
    public GameObject gemReward;
    public GameObject star;
    public GameObject gem;

    public GameObject rewindGroup;

    [Header("Parent")]
    public LevelMasterBase levelMaster;
    public void RegisterLevelMaster(LevelMasterBase _master)
    {
        levelMaster = _master;
    }
    public void ResetAllPopups()
    {
        popupMask.SetActive(false);
        victoryPopupGroup.SetActive(false);
        failurePopupGroup.SetActive(false);
    }
    public void ShowVictoryPopup(bool isHard)
    {
        popupMask.SetActive(true);
        victoryPopupGroup.SetActive(true);
        starReward.SetActive(!isHard);
        gemReward.SetActive(isHard);
        popupMask.GetComponent<SpriteRenderer>().DOFade(0f, dConstants.UI.StandardizedBtnAnimDuration).From();
        victoryPopupGroup.transform.DOMoveY(-15f, dConstants.UI.StandardizedBtnAnimDuration).From(true, true);
        star.transform.DOScale(0f, dConstants.UI.StandardizedBtnAnimDuration * 2f).From().SetEase(Ease.OutBounce).SetDelay(dConstants.UI.StandardizedBtnAnimDuration);
        gem.transform.DOScale(0f, dConstants.UI.StandardizedBtnAnimDuration * 2f).From().SetEase(Ease.OutBounce).SetDelay(dConstants.UI.StandardizedBtnAnimDuration);
    }
    public void ShowFailurePopup(bool isRewind)
    {
        popupMask.SetActive(true);
        failurePopupGroup.SetActive(true);
        rewindGroup.SetActive(isRewind);
        popupMask.GetComponent<SpriteRenderer>().DOFade(0f, dConstants.UI.StandardizedBtnAnimDuration).From();
        failurePopupGroup.transform.DOMoveY(-15f, dConstants.UI.StandardizedBtnAnimDuration).From(true, true);
    }

    public void CollectStar()
    {
        levelMaster.StartNextLevel();
    }
    public void CollectGem()
    {
        levelMaster.StartNextLevel();
    }
}
