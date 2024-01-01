using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;



public class ThemePhotoGroup : MonoBehaviour
{
    public enum ThemePhotoStatus { hidden = 0, locked = 1, unlocked = 2, finished = 3, perfect = 4 };

    [Header("Gameplay Data")]
    //public LevelSelector selector;
    [SerializeField] SheetItem_ThemeSetup themeData;
    public ThemePhotoStatus curStatus;

    [Header("Children Objs")]
    public Transform photoGroup;
    public Transform infoGroup;

    public GameObject photoBg;
    public GameObject photoCover;
    public TextMeshPro unlockReq;
    public GameObject photo;
    public GameObject themeIcon;
    public GameObject completeSign;
    public TextMeshPro photoLine;

    public TextMeshPro themeName;
    public TextMeshPro starCollection;
    public TextMeshPro gemCollection;
    public NoteLauncher designNote;

    int curStars = 0;
    int curGems = 0;
    ThemePhotoStatus DetermineCurStatus()
    {
        LevelRecords records = LevelSelector.singleton.playerLevelRecords;
        bool preReqMet = records.spentTokens >= themeData.unlockPrereq;
        bool unlocked = records.isThemeUnlocked(themeData.themeUID);
        bool finished = true;
        bool perfect = true;
        curStars = 0;
        curGems = 0;
        for (int i = 0; i < themeData.levels.Count; i++)
        {
            if (!records.isLevelFinished(themeData.levels[i].levelUID))
            {
                perfect = false;
                if (!themeData.levels[i].isHard)
                {
                    finished = false;
                }
            }
            else
            {
                if (!themeData.levels[i].isHard)
                {
                    curStars += 1;
                }
                else
                {
                    curGems += 1;
                }
            }
        }
        StaticTextUpdate();
        if (!preReqMet)
        {
            return ThemePhotoStatus.hidden;
        }
        else if (!unlocked)
        {
            return ThemePhotoStatus.locked;
        }
        else if (!finished)
        {
            return ThemePhotoStatus.unlocked;
        }
        else if (!perfect)
        {
            return ThemePhotoStatus.finished;
        }
        else
        {
            return ThemePhotoStatus.perfect;
        }
        
    }
    void UIUpdateBasedOnStatus()
    {
        if (curStatus == ThemePhotoStatus.hidden)
        {
            gameObject.SetActive(false);
        }
        else if (curStatus == ThemePhotoStatus.locked)
        {
            gameObject.SetActive(true);
            photoGroup.gameObject.SetActive(true);
            infoGroup.gameObject.SetActive(false);

            photoCover.GetComponent<SpriteRenderer>().enabled = true;
            unlockReq.gameObject.SetActive(true);
            themeIcon.SetActive(false);
            completeSign.SetActive(false);
            photoLine.SetText(LocalizedAssetLookup.singleton.Translate(themeData.lockedLine));
        }
        else if (curStatus == ThemePhotoStatus.unlocked || curStatus == ThemePhotoStatus.finished || curStatus == ThemePhotoStatus.perfect)
        {
            gameObject.SetActive(true);
            photoGroup.gameObject.SetActive(true);
            infoGroup.gameObject.SetActive(true);

            photoCover.GetComponent<SpriteRenderer>().enabled = false;
            unlockReq.gameObject.SetActive(true);
            themeIcon.SetActive(true);
            photoLine.SetText(LocalizedAssetLookup.singleton.Translate(themeData.unlockedLine));

            designNote.gameObject.SetActive(curStatus == ThemePhotoStatus.finished || curStatus == ThemePhotoStatus.perfect);
            completeSign.gameObject.SetActive(curStatus == ThemePhotoStatus.perfect);
        }
    }
    void StaticTextUpdate()
    {
        //init all texts that used in a theme photo group whenever progress changed
        unlockReq.SetText(string.Format(LocalizedAssetLookup.singleton.Translate("<sprite name=currency_star_frame> {0}<br>½âËø"), themeData.unlockCost));
        themeName.SetText(LocalizedAssetLookup.singleton.Translate(themeData.themeTitle));
        starCollection.SetText(string.Format(LocalizedAssetLookup.singleton.Translate("<sprite name=currency_star> {0}/{1}"), curStars, themeData.TotalStars));
        gemCollection.SetText(string.Format(LocalizedAssetLookup.singleton.Translate("<sprite name=currency_gem> {0}/{1}"), curGems, themeData.TotalGems));
    }
    #region BtnFunc
    void EnterPageAnimation()
    {

    }
    public void PhotoOnSelection()
    {
        photo.GetComponent<photoVFXCtrl>().ZoomIn(dConstants.UI.StandardizedBtnAnimDuration);
    }
    public void PhotoExitSelection()
    {
        photo.GetComponent<photoVFXCtrl>().ZoomReset(dConstants.UI.StandardizedBtnAnimDuration);
    }
    public void PhotoOnClick()
    {
        //enter hidden obj or level
    }
    public void NoteOnSelection()
    {
        designNote.label.DOColor(dConstants.UI.DefaultColor_3rd, dConstants.UI.StandardizedBtnAnimDuration);
        designNote.text.DOColor(dConstants.UI.DefaultColor_1st, dConstants.UI.StandardizedBtnAnimDuration);
    }
    public void NoteExitSelection()
    {
        designNote.label.DOColor(dConstants.UI.DefaultColor_1st, dConstants.UI.StandardizedBtnAnimDuration);
        designNote.text.DOColor(dConstants.UI.DefaultColor_Black, dConstants.UI.StandardizedBtnAnimDuration);
    }
    public void NoteOnClick()
    {

    }
    #endregion
}
