using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public CurrencySet currencySet;

    public TextMeshPro timeUsed;
    public TextMeshPro stepUsed;

    private bool isNewLevelFinished;

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
    public void ShowVictoryPopup(bool isNew, bool isHard, string time, string step)
    {
        popupMask.SetActive(true);
        victoryPopupGroup.SetActive(true);
        timeUsed.SetText(string.Format("<sprite name=ui_time_sign> {0}", time));
        stepUsed.SetText(string.Format("<sprite name=ui_step_sign> {0}", step));
        star.gameObject.SetActive(true);
        gem.gameObject.SetActive(true);
        starReward.SetActive(!isHard);
        gemReward.SetActive(isHard);
        popupMask.GetComponent<SpriteRenderer>().DOFade(0f, dConstants.UI.StandardizedBtnAnimDuration).From();
        victoryPopupGroup.transform.DOMoveY(-15f, dConstants.UI.StandardizedBtnAnimDuration).From(true, true);
        star.transform.DOScale(0f, dConstants.UI.StandardizedBtnAnimDuration * 2f).From().SetEase(Ease.OutBounce).SetDelay(dConstants.UI.StandardizedBtnAnimDuration);
        gem.transform.DOScale(0f, dConstants.UI.StandardizedBtnAnimDuration * 2f).From().SetEase(Ease.OutBounce).SetDelay(dConstants.UI.StandardizedBtnAnimDuration);

        isNewLevelFinished = isNew;
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
        if (isNewLevelFinished)
        {
            GameObject starVFX = Instantiate(star, star.transform.parent);
            star.SetActive(false);
            Sequence seq = DOTween.Sequence();
            seq.Append(starVFX.transform.DOMove(currencySet.starIcon.transform.position, dConstants.UI.StandardizedVFXAnimDuration).SetEase(Ease.InSine));
            seq.AppendCallback(() => currencySet.starIcon.SetActive(true));
            seq.AppendCallback(() => currencySet.starIcon.transform.DOScale(1.5f, dConstants.UI.StandardizedBtnAnimDuration / 2f).SetRelative(true).SetLoops(2, LoopType.Yoyo));
            seq.AppendCallback(() => currencySet.curStarCount.gameObject.SetActive(true));
            seq.AppendCallback(() => Destroy(starVFX));
            seq.AppendCallback(() => currencySet.StarCountAdjustAnimation(1));
            seq.AppendInterval(dConstants.UI.StandardizedBtnAnimDuration + dConstants.UI.StandardizedVFXAnimDuration);
            seq.AppendCallback(() => currencySet.starIcon.SetActive(false));
            seq.AppendCallback(() => currencySet.curStarCount.gameObject.SetActive(false));
            seq.AppendCallback(() => levelMaster.StartNextLevel());
        }
        else
        {
            star.SetActive(false);
            Sequence seq = DOTween.Sequence();
            seq.AppendCallback(() => levelMaster.StartNextLevel());
        }
        
    }
    public void CollectGem()
    {
        if (isNewLevelFinished)
        {

            GameObject gemVFX = Instantiate(gem, gem.transform.parent);
            gem.SetActive(false);
            Sequence seq = DOTween.Sequence();
            seq.Append(gemVFX.transform.DOMove(currencySet.gemIcon.transform.position, dConstants.UI.StandardizedVFXAnimDuration).SetEase(Ease.InSine));
            seq.AppendCallback(() => currencySet.gemIcon.SetActive(true));
            seq.AppendCallback(() => currencySet.gemIcon.transform.DOScale(1.5f, dConstants.UI.StandardizedBtnAnimDuration / 2f).SetRelative(true).SetLoops(2, LoopType.Yoyo));
            seq.AppendCallback(() => currencySet.gemCount.gameObject.SetActive(true));
            seq.AppendCallback(() => Destroy(gemVFX));
            seq.AppendCallback(() => currencySet.GemCountAdjustAnimation(1));
            seq.AppendInterval(dConstants.UI.StandardizedBtnAnimDuration + dConstants.UI.StandardizedVFXAnimDuration);
            seq.AppendCallback(() => currencySet.gemIcon.SetActive(false));
            seq.AppendCallback(() => currencySet.gemCount.gameObject.SetActive(false));
            seq.AppendCallback(() => levelMaster.StartNextLevel());
        }
        else
        {
            star.SetActive(false);
            Sequence seq = DOTween.Sequence();
            seq.AppendCallback(() => levelMaster.StartNextLevel());
        }
    }

}
