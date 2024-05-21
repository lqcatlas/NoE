using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TitlePage : MonoBehaviour, ISaveData
{
    static public TitlePage singleton;
    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        }
        else
        {
            Destroy(this);
        }
        firstOpening = true;
    }
    public bool firstOpening = true;

    [SerializeField] PlayerSettings playerSettings;
    [SerializeField] LevelRecords levelRecords;
    [SerializeField] IntroLines intro;

    [SerializeField] SpriteRenderer titleSprite;
    [SerializeField] GameObject confirmBtn;
    [SerializeField] TextMeshPro line_tmp;

    [SerializeField] List<Sprite> titleByLanguage;


    static float LINE_EMERGE_DURATION = 2f;
    
    private void Start()
    {
        //line_tmp.color = new Color(dConstants.UI.DefaultColor_1st.r, dConstants.UI.DefaultColor_1st.g, dConstants.UI.DefaultColor_1st.b, 0f);
        GoToTitlePage();
    }
    
    public void GoToTitlePage()
    {
        gameObject.SetActive(true);
        BgCtrl.singleton.SetToPhase(dConstants.Gameplay.GamePhase.Title);
        //playtest
        if (firstOpening)
        {
            ShowPlaytestPopup();
        }
    }
    public void EnterBtnClick()
    {
        if(playerSettings.introCount * 3 <= levelRecords.finishedLevels.Count && firstOpening)
        {
            int rng = Random.Range(0, intro.lines.Count);
            LineEmerge(LINE_EMERGE_DURATION, intro.lines[rng]);
            playerSettings.introCount += 1;
        }
        else
        {
            OpenSelector();
        }
        firstOpening = false;
    }
    public void OpenSelector()
    {
        LevelSelector.singleton.GoToSelector();
        BgCtrl.singleton.ResetToDefaultBg();
        gameObject.SetActive(false);
    }
    void LineEmerge(float duration, string txt)
    {
        titleSprite.gameObject.SetActive(false);
        confirmBtn.gameObject.SetActive(false);

        line_tmp.SetText(LocalizedAssetLookup.singleton.Translate(txt));
        line_tmp.DOFade(1f, duration).SetLoops(2, LoopType.Yoyo).OnComplete(() => OpenSelector());
        BgCtrl.singleton.SetToLightBg(duration);
    }
    public void ExitEntireGame()
    {
        Application.Quit();
    }
    public void SwitchToCN()
    {
        LocalizedAssetLookup.singleton.SwitchLanguage(LanguageOption.CN);
        titleSprite.sprite = titleByLanguage[0];
        titleSprite.gameObject.GetComponent<AdvSpriteSlider>().ResetBaseSprite();
        GoToTitlePage();
    }
    public void SwitchToEN()
    {
        LocalizedAssetLookup.singleton.SwitchLanguage(LanguageOption.EN);
        titleSprite.sprite = titleByLanguage[1];
        titleSprite.gameObject.GetComponent<AdvSpriteSlider>().ResetBaseSprite();
        GoToTitlePage();
    }
    void ShowPlaytestPopup()
    {
        string title = "@Loc=ui_playtest_title@@";
        string desc = "@Loc=ui_playtest_desc@@";
        MsgBox.singleton.ShowBox(title, desc, "", null);
    }
    #region save
    private const string AUDIOVOLUME_SAVE_KEY = "setting.volume";
    private const string INTROCOUNT_SAVE_KEY = "setting.intro";
    public void LoadFromSaveManager()
    {
        string str = SaveManager.controller.Inquire(string.Format(AUDIOVOLUME_SAVE_KEY));
        if (str != null)
        {
            float.TryParse(SaveManager.controller.Inquire(string.Format(AUDIOVOLUME_SAVE_KEY)), out playerSettings.audioVolume);
        }
        else
        {
            playerSettings.audioVolume = 0.5f;
        }
        str = SaveManager.controller.Inquire(string.Format(INTROCOUNT_SAVE_KEY));
        if (str != null)
        {
            int.TryParse(SaveManager.controller.Inquire(string.Format(INTROCOUNT_SAVE_KEY)), out playerSettings.introCount);
        }
        else
        {
            playerSettings.introCount = 0;
        }
    }
    public void SaveToSaveManager()
    {
        //Debug.Log(string.Format("player settings save data to file, intro count:{0}", playerSettings.introCount));
        SaveManager.controller.Insert(string.Format(AUDIOVOLUME_SAVE_KEY), playerSettings.audioVolume.ToString());
        SaveManager.controller.Insert(string.Format(INTROCOUNT_SAVE_KEY), playerSettings.introCount.ToString());
    }
    #endregion
}
