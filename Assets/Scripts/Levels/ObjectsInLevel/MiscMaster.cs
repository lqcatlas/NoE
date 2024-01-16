using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class MiscMaster : MonoBehaviour
{
    const float ScreenFadeDuration = 1f;
    [SerializeField] Color MaskColor;
    public SpriteRenderer background;
    public Transform maskGroup;
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
    public void InitThemeBackground()
    {
        //clear all existing children;
        while(maskGroup.childCount > 0)
        {
            Destroy(maskGroup.GetChild(0).gameObject);
        }
        /*List<Transform> children = maskGroup.GetComponentsInChildren<Transform>(true).ToList();
        for(int i = 0; i < children.Count; i++)
        {
            Destroy(children[i].gameObject);
        }
        */
        fadePhoto = Instantiate(HiddenObjectLauncher.singleton.GetCurrentBgPhotoObject(), maskGroup);
        if(fadePhoto == null)
        {
            fadePhoto = fadeMask.gameObject;
        }
        fadePhoto.GetComponent<SpriteRenderer>().sortingLayerID = fadeMask.sortingLayerID;
        fadePhoto.GetComponent<SpriteRenderer>().sortingOrder = fadeMask.sortingOrder + 1;
        fadePhoto.GetComponent<BoxCollider2D>().enabled = false;
    }
    public void ResetMiscs()
    {
        closeBtn.SetActive(true);
        retryHint.gameObject.SetActive(false);
        retryBtn.SetActive(true);
        loseBanner.SetActive(false);
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
    
    #endregion

    #region rewind
    public void RewindStep()
    {
        levelMaster.LevelRewind();
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
