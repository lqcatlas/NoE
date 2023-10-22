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
    public GameObject retryBtn;
    public GameObject closeBtn;
    public GameObject loseBanner;
    public TextMeshPro retryHint;
    [Header("Data")]
    public LevelMasterBase levelMaster;

    public void RegisterLevelMaster(LevelMasterBase _master)
    {
        levelMaster = _master;
    }
    #region close
    public void CloseLevel()
    {
        levelMaster.LevelExit();
    }
    public void CloseBtnHoverEnter()
    {
        closeBtn.transform.DOScale(Vector3.one * 6f, dConstants.UI.StandardizedBtnAnimDuration);
    }
    public void CloseBtnHoverExit()
    {
        closeBtn.transform.DOScale(Vector3.one * 5f, dConstants.UI.StandardizedBtnAnimDuration);
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

    #region mask
    public void ScreenMaskInit()
    {
        fadeMask.GetComponent<Collider2D>().enabled = true;
        fadeMask.color = MaskColor;
    }
    public void ScreenMaskFadeOut(float _duration = ScreenFadeDuration)
    {
        fadeMask.DOFade(0f, _duration).OnComplete(()=>fadeMask.GetComponent<Collider2D>().enabled = false).SetEase(Ease.InCubic);
    }
    public void ScreenMaskFadeIn(float _duration = ScreenFadeDuration)
    {
        fadeMask.GetComponent<Collider2D>().enabled = true;
        fadeMask.DOFade(1f, _duration).SetEase(Ease.OutCubic);
    }
    #endregion
}
