using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [Header("VFX Prefabs")]
    [SerializeField] GameObject SelectorTransitionFX;

    int curStars = 0;
    int curGems = 0;
    public void UpdatePhotoGroup()
    {
        curStatus = DetermineCurStatus();
        UIUpdateBasedOnStatus();
    }
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
    public void UIUpdateBasedOnStatus()
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
            unlockReq.gameObject.SetActive(false);
            themeIcon.SetActive(true);
            photoLine.SetText(LocalizedAssetLookup.singleton.Translate(themeData.unlockedLine));

            designNote.gameObject.SetActive(curStatus == ThemePhotoStatus.finished || curStatus == ThemePhotoStatus.perfect);
            completeSign.gameObject.SetActive(curStatus == ThemePhotoStatus.perfect);
        }
    }
    void StaticTextUpdate()
    {
        //init all texts that used in a theme photo group whenever progress changed
        unlockReq.SetText(string.Format(LocalizedAssetLookup.singleton.Translate("@Loc=ui_themephoto_unlockreq@@"), themeData.unlockCost));
        themeName.SetText(LocalizedAssetLookup.singleton.Translate(themeData.themeTitle));
        starCollection.SetText(string.Format(LocalizedAssetLookup.singleton.Translate("@Loc=ui_themephoto_starcollection@@"), curStars, themeData.TotalStars));
        gemCollection.SetText(string.Format(LocalizedAssetLookup.singleton.Translate("@Loc=ui_themephoto_gemcollection@@"), curGems, themeData.TotalGems));
    }
    #region BtnFunc
    void EnterPageAnimation()
    {

    }
    public void PhotoEnterSelection()
    {
        if(curStatus == ThemePhotoStatus.locked)
        {
            if(themeData.unlockCost <= LevelSelector.singleton.playerLevelRecords.tokens)
            {
                //unlockable
                photo.GetComponent<photoVFXCtrl>().ZoomIn(dConstants.UI.StandardizedBtnAnimDuration);
            }
        }
        else
        {
            photo.GetComponent<photoVFXCtrl>().ZoomIn(dConstants.UI.StandardizedBtnAnimDuration);
        }
        
    }
    public void PhotoExitSelection()
    {
        photo.GetComponent<photoVFXCtrl>().ZoomReset(dConstants.UI.StandardizedBtnAnimDuration);
    }
    public void PhotoConfirmSelection()
    {
        //enter hidden obj or level
        if (curStatus == ThemePhotoStatus.locked)
        {
            if (themeData.unlockCost <= LevelSelector.singleton.playerLevelRecords.tokens)
            {
                //LevelSelector.singleton.UnlockTheme(themeData.themeUID, themeData.unlockCost);
                //enter hidden obj
                HiddenObjectLauncher.singleton.LaunchHiddenObjectPage(themeData);
                LevelSelector.singleton.CloseSelector();
            }
        }
        else if(curStatus == ThemePhotoStatus.unlocked || curStatus == ThemePhotoStatus.finished || curStatus == ThemePhotoStatus.perfect)
        {
            GoToLatestLevel();
        }
    }
    public void NoteEnterSelection()
    {
        designNote.label.DOColor(dConstants.UI.DefaultColor_3rd, dConstants.UI.StandardizedBtnAnimDuration);
        designNote.text.DOColor(dConstants.UI.DefaultColor_1st, dConstants.UI.StandardizedBtnAnimDuration);
    }
    public void NoteExitSelection()
    {
        designNote.label.DOColor(dConstants.UI.DefaultColor_1st, dConstants.UI.StandardizedBtnAnimDuration);
        designNote.text.DOColor(dConstants.UI.DefaultColor_Black, dConstants.UI.StandardizedBtnAnimDuration);
    }
    public void NoteConfirmSelection()
    {
        string _title = string.Format("{0}-{1}", LocalizedAssetLookup.singleton.Translate("@Loc=ui_designer_note_title@@"), LocalizedAssetLookup.singleton.Translate(string.Format("@Loc=themename_tm{0}@@", themeData.themeUID)));
        string _desc = themeData.manifesto;
        LevelSelector.singleton.DesignerNoteBox.ShowBox(_title, _desc);
    }
    #endregion

    void GoToLatestLevel()
    {
        int targetLevelUID = GetLatestLevelUID();
        GameObject obj = Instantiate(SelectorTransitionFX, transform);
        obj.transform.parent = VFXHolder.singleton.transform;
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(dConstants.VFX.SelectorToLevelAnimTransitionPhase1);
        seq.AppendCallback(() => LevelLauncher.singleton.LaunchLevelByUID(targetLevelUID));
        seq.AppendCallback(() => LevelSelector.singleton.CloseSelector());
    }
    int GetLatestLevelUID()
    {
        LevelRecords records = LevelSelector.singleton.playerLevelRecords;
        for (int i = 0; i < themeData.levels.Count; i++)
        {
            if (records.isLevelFinished(themeData.levels[i].levelUID))
            {
                continue;
            }
            else
            {
                return themeData.levels[i].levelUID;
            }
        }
        return themeData.levels[themeData.levels.Count - 1].levelUID;
    }
}
