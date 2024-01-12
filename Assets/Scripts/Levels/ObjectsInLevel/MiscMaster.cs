using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class MiscMaster : MonoBehaviour
{
    const float ScreenFadeDuration = 1f;
    [SerializeField] Color MaskColor;
    public SpriteRenderer background;
    public SpriteRenderer fadeMask;
    public GameObject fadePhoto;
    public GameObject retryBtn;
    public GameObject closeBtn;
    public GameObject rewindBtn;

    public GameObject prevBtn;
    public GameObject nextBtn;
    public TextMeshPro levelName;

    public GameObject loseBanner;
    public TextMeshPro retryHint;
    public TextMeshPro newthemeHint;
    [Header("Parent")]
    public LevelMasterBase levelMaster;

    public void RegisterLevelMaster(LevelMasterBase _master)
    {
        levelMaster = _master;
    }
    #region close
    public void CloseLevel()
    {
        levelMaster.LevelExit();
        HiddenObjectLauncher.singleton.ClearExistingPages();
    }
    #endregion

    #region retry
    public void RetryLevel()
    {
        levelMaster.LevelRetry();
    }
    public void RetryBtnHoverEnter()
    {
        retryBtn.transform.DORotate(new Vector3(0f, 0f, 90f), dConstants.UI.StandardizedBtnAnimDuration);
    }
    public void RetryBtnHoverExit()
    {
        retryBtn.transform.DORotate(Vector3.zero, dConstants.UI.StandardizedBtnAnimDuration);
    }
    #endregion

    #region rewind
    public void RewindStep()
    {
        levelMaster.Rewind();
    }
    public void RewindBtnHoverEnter()
    {
        retryBtn.transform.DORotate(new Vector3(0f, 0f, 15f), dConstants.UI.StandardizedBtnAnimDuration);
    }
    public void RewindBtnHoverExit()
    {
        retryBtn.transform.DORotate(Vector3.zero, dConstants.UI.StandardizedBtnAnimDuration);
    }
    #endregion

    #region navigation
    public void GoToPreviousLevel()
    {
        levelMaster.GoToPreviousLevel();
    }
    public void GoToNextLevel()
    {
        levelMaster.GoToNextLevel();
    }
    public void PrevBtnHoverEnter()
    {
        prevBtn.transform.DOScale(Vector3.one * 3f, dConstants.UI.StandardizedBtnAnimDuration);
    }
    public void PrevBtnHoverExit()
    {
        prevBtn.transform.DOScale(Vector3.one * 2.5f, dConstants.UI.StandardizedBtnAnimDuration);
    }
    public void NextBtnHoverEnter()
    {
        nextBtn.transform.DOScale(Vector3.one * 3f, dConstants.UI.StandardizedBtnAnimDuration);
    }
    public void NextBtnHoverExit()
    {
        nextBtn.transform.DOScale(Vector3.one * 2.5f, dConstants.UI.StandardizedBtnAnimDuration);
    }
    #endregion


    #region mask
    public void ScreenMaskInit()
    {
        fadeMask.GetComponent<Collider2D>().enabled = true;
        //fadeMask.color = MaskColor;
        fadePhoto.GetComponent<SpriteRenderer>().color = MaskColor;
    }
    public void ScreenMaskFadeOut(float _duration = ScreenFadeDuration)
    {
        fadePhoto.GetComponent<SpriteRenderer>().DOFade(0f, _duration).OnComplete(()=>fadeMask.GetComponent<Collider2D>().enabled = false).SetEase(Ease.InCubic);
    }
    public void ScreenMaskFadeIn(float _duration = ScreenFadeDuration)
    {
        fadeMask.GetComponent<Collider2D>().enabled = true;
        fadePhoto.GetComponent<SpriteRenderer>().DOFade(1f, _duration).SetEase(Ease.OutCubic);
    }
    #endregion
}
