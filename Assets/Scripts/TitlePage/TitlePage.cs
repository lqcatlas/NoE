using DG.Tweening;
using Steamworks;
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
    static float TITLE_ANIM_DURATION = 2f;
    
    [Header("Gameplay Data")]
    [SerializeField] PlayerSettings playerSettings;
    [SerializeField] LevelRecords levelRecords;
    [SerializeField] IntroLines intro;
    [Header("Children Objs")]
    [SerializeField] GameObject page;
    [SerializeField] GameObject widgets;
    [SerializeField] SpriteRenderer titleSprite;
    [SerializeField] GameObject confirmBtn;
    [SerializeField] TextMeshPro line_tmp;
    [Header("Others")]
    [SerializeField] List<Sprite> titleByLanguage;


    static float LINE_EMERGE_DURATION = 2f;
    
    private void Start()
    {
        //line_tmp.color = new Color(dConstants.UI.DefaultColor_1st.r, dConstants.UI.DefaultColor_1st.g, dConstants.UI.DefaultColor_1st.b, 0f);
        if (!levelRecords.seenIntro)
        {
            IntroPage.singleton.StartIntro();
        }
        else
        {
            //Sequence seq = DOTween.Sequence();
            //seq.AppendInterval(1f);
            //seq.AppendCallback(() => IntroAnimToTitlePage());
            IntroAnimToTitlePage();
        }
    }
    
    public void GoToTitlePage()
    {
        page.SetActive(true);
        widgets.SetActive(true);
        BgCtrl.singleton.SetToPhase(dConstants.Gameplay.GamePhase.Title);
        titleSprite.gameObject.SetActive(true);
        confirmBtn.SetActive(true);
        //playtest only
        /*if (firstOpening)
        {
            Sequence seq = DOTween.Sequence();
            seq.AppendInterval(.5f);
            seq.AppendCallback(() => ShowPlaytestPopup());
        }
        */
    }
    public void IntroAnimToTitlePage()
    {
        levelRecords.seenIntro = true;
        page.SetActive(true);
        widgets.SetActive(false);
        titleSprite.gameObject.SetActive(true);
        titleSprite.DOFade(0f, TITLE_ANIM_DURATION / 2f).From();
        AudioCentralCtrl.singleton.BgMusicStart();
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(TITLE_ANIM_DURATION);
        seq.AppendCallback(() => GoToTitlePage());

        if (LocalizedAssetLookup.singleton.curLanguage == LanguageOption.CN)
        {
            SwitchToCN();
        }
        else
        {
            SwitchToEN();
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
        page.SetActive(false);
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
    public void OpenSetting()
    {
        if (!SettingPage.singleton.isOpening())
        {
            SettingPage.singleton.GotoSettingPage();
        }
        else
        {
            SettingPage.singleton.ClosePage();
        }
        
    }
    public void SwitchToCN()
    {
        LocalizedAssetLookup.singleton.SwitchLanguage(LanguageOption.CN);
        titleSprite.sprite = titleByLanguage[1];
        titleSprite.gameObject.GetComponent<AdvSpriteSlider>().ResetBaseSprite();
        //GoToTitlePage();
    }
    public void SwitchToEN()
    {
        LocalizedAssetLookup.singleton.SwitchLanguage(LanguageOption.EN);
        titleSprite.sprite = titleByLanguage[0];
        titleSprite.gameObject.GetComponent<AdvSpriteSlider>().ResetBaseSprite();
        //GoToTitlePage();
    }
    void ShowPlaytestPopup()
    {
        string title = "@Loc=ui_playtest_title@@";
        string desc = "@Loc=ui_playtest_desc@@";
        MsgBox.singleton.ShowBox(title, desc, "", null);
    }
    #region save
    private const string AUDIOVOLUME_SAVE_KEY = "setting.volume";
    private const string MUSICVOLUME_SAVE_KEY = "setting.musicVolume";
    private const string SOUNDVOLUME_SAVE_KEY = "setting.soundVolume";
    private const string INTROCOUNT_SAVE_KEY = "setting.intro";
    private const string LANGUAGE_SAVE_KEY = "setting.language";
    public void LoadFromSaveManager()
    {
        string str = SaveManager.controller.Inquire(string.Format(AUDIOVOLUME_SAVE_KEY));
        if (str != null)
        {
            float.TryParse(SaveManager.controller.Inquire(string.Format(AUDIOVOLUME_SAVE_KEY)), out playerSettings.audioVolume);
        }
        else
        {
            playerSettings.audioVolume = 1f;
        }
        str = SaveManager.controller.Inquire(string.Format(MUSICVOLUME_SAVE_KEY));
        if (str != null)
        {
            float.TryParse(SaveManager.controller.Inquire(string.Format(MUSICVOLUME_SAVE_KEY)), out playerSettings.musicVolume);
        }
        else
        {
            playerSettings.musicVolume = 0.7f;
        }
        str = SaveManager.controller.Inquire(string.Format(SOUNDVOLUME_SAVE_KEY));
        if (str != null)
        {
            float.TryParse(SaveManager.controller.Inquire(string.Format(SOUNDVOLUME_SAVE_KEY)), out playerSettings.soundVolume);
        }
        else
        {
            playerSettings.soundVolume = 0.7f;
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
        str = SaveManager.controller.Inquire(string.Format(LANGUAGE_SAVE_KEY));
        if (str != null)
        {
            int curIndex = 0;
            int.TryParse(SaveManager.controller.Inquire(string.Format(LANGUAGE_SAVE_KEY)), out curIndex);
            playerSettings.curLan = (LanguageOption)curIndex;
            LocalizedAssetLookup.singleton.SwitchLanguage(playerSettings.curLan);
            //Debug.LogWarning($"set lan to {playerSettings.curLan} based from loading");
        }
        else if (SteamManager.Initialized)
        {
            if (SteamApps.GetCurrentGameLanguage() == "schinese")
            {
                playerSettings.curLan = LanguageOption.CN;
                LocalizedAssetLookup.singleton.SwitchLanguage(playerSettings.curLan);
            }
        }
        else
        {
            playerSettings.curLan = LocalizedAssetLookup.singleton.defaultLanguage;
            LocalizedAssetLookup.singleton.SwitchLanguage(playerSettings.curLan);
        }
    }
    public void SaveToSaveManager()
    {
        //Debug.Log(string.Format("player settings save data to file, intro count:{0}", playerSettings.introCount));
        SaveManager.controller.Insert(string.Format(AUDIOVOLUME_SAVE_KEY), playerSettings.audioVolume.ToString());
        SaveManager.controller.Insert(string.Format(MUSICVOLUME_SAVE_KEY), playerSettings.musicVolume.ToString());
        SaveManager.controller.Insert(string.Format(SOUNDVOLUME_SAVE_KEY), playerSettings.soundVolume.ToString());
        SaveManager.controller.Insert(string.Format(INTROCOUNT_SAVE_KEY), playerSettings.introCount.ToString());
        SaveManager.controller.Insert(string.Format(LANGUAGE_SAVE_KEY), ((int)playerSettings.curLan).ToString());
    }
    #endregion
}
