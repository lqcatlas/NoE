using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using TreeEditor;
using UnityEngine;

public class HiddenObjectPage : MonoBehaviour
{
    public enum HiddenObjectStatus { close = 0, searching = 1, found = 2};
    [Header("Gameplay Data")]
    //public LevelSelector selector;
    [SerializeField] SheetItem_ThemeSetup themeData;
    public HiddenObjectStatus curStatus = HiddenObjectStatus.close;

    [Header("Animation Params")]
    public float TRANSIT_ANIM_DURATION;
    public float FOUND_ANIM_DURATION;
    public float HINT_DELAY;
    public Transform iconDestination;
    [Header("Children Objs")]
    public GameObject objectGroup;
    public GameObject riddleGroup;
    public GameObject closeGroup;
    public GameObject photo;
    //public GameObject photo_black;

    public TextMeshPro riddleTitle;
    public TextMeshPro riddleDesc;
    public GameObject themeIcon;
    public TextMeshPro themeName;
    public GameObject hintRing;

    [SerializeField] bool displayOn;
    [SerializeField] float hintTimer;
    public void Awake()
    {
        displayOn = false;
    }
    public void StartHiddenObject(SheetItem_ThemeSetup source = null)
    {
        curStatus = HiddenObjectStatus.searching;
        //displayOn = true;
        if(source != null)
        {
            themeData = source;
        }
        riddleTitle.SetText(LocalizedAssetLookup.singleton.Translate("@Loc=ui_hiddenobject_riddletitle@@"));
        riddleDesc.SetText(LocalizedAssetLookup.singleton.Translate(themeData.hint));
        themeName.SetText(LocalizedAssetLookup.singleton.Translate(themeData.themeTitle));
        hintTimer = 0;
        themeIcon.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        hintRing.SetActive(false);
    }
    public void SetAsBackground(SheetItem_ThemeSetup source = null)
    {
        curStatus = HiddenObjectStatus.found;
        riddleGroup.SetActive(false);
        objectGroup.SetActive(false);
        closeGroup.SetActive(false);
        photo.GetComponent<photoVFXCtrl>().ZoomIn(0);
        photo.GetComponent<photoVFXCtrl>().Replace(0);
        photo.GetComponent<photoVFXCtrl>().ReduceOffsetMovement();
    }
    public GameObject GetCurrentBackgroundPhoto()
    {
        return photo;
    }
    private void FixedUpdate()
    {
        if (curStatus == HiddenObjectStatus.searching)
        {
            hintTimer += Time.fixedDeltaTime;
        }
        if(hintTimer >= HINT_DELAY)
        {
            hintRing.SetActive(true);
            hintTimer -= HINT_DELAY;
        }
    }
    #region BtnFunc
    public void OnIconClick()
    {
        if(curStatus == HiddenObjectStatus.searching)
        {
            //status change
            curStatus = HiddenObjectStatus.found;
            hintRing.gameObject.SetActive(false);
            closeGroup.SetActive(false);
            //unlock given theme
            LevelSelector.singleton.UnlockTheme(themeData.themeUID, themeData.unlockCost);
            //play found animation. Then transit to level
            photo.GetComponent<photoVFXCtrl>().ZoomIn(FOUND_ANIM_DURATION);
            photo.GetComponent<photoVFXCtrl>().Replace(FOUND_ANIM_DURATION / 2f);
            //theme icon appear and then fly to target location
            themeName.DOFade(1f, FOUND_ANIM_DURATION / 2f);
            themeIcon.GetComponent<SpriteRenderer>().DOFade(1f, FOUND_ANIM_DURATION / 2f);
            themeIcon.transform.DOMove(iconDestination.position, TRANSIT_ANIM_DURATION).SetDelay(FOUND_ANIM_DURATION / 2f).SetEase(Ease.InSine);
            themeIcon.transform.DOScale(1.2f, dConstants.UI.StandardizedBtnAnimDuration).SetDelay(TRANSIT_ANIM_DURATION + FOUND_ANIM_DURATION / 2f);
            themeIcon.GetComponent<SpriteRenderer>().DOFade(0f, dConstants.UI.StandardizedBtnAnimDuration).SetDelay(TRANSIT_ANIM_DURATION + FOUND_ANIM_DURATION / 2f).OnComplete(() => StartLevelDelayed());
        }
    }
    void StartLevelDelayed()
    {
        riddleGroup.SetActive(false);
        objectGroup.SetActive(false);
        photo.GetComponent<photoVFXCtrl>().ReduceOffsetMovement();
        LevelLauncher.singleton.LaunchLevelByUID(themeData.levels[0].levelUID);
    }
    public void OnCloseClick()
    {
        Destroy(gameObject);
        LevelSelector.singleton.GoToSelector();
    }
    #endregion
}
