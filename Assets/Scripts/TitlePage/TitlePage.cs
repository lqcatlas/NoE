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
    }
    [SerializeField] PlayerSettings playerSettings;
    [SerializeField] LevelRecords levelRecords;
    [SerializeField] IntroLines intro;

    [SerializeField] SpriteRenderer titleSprite;
    [SerializeField] GameObject confirmBtn;
    [SerializeField] TextMeshPro line_tmp;

    static float LINE_EMERGE_DURATION = 2f;
    
    private void Start()
    {
        //line_tmp.color = new Color(dConstants.UI.DefaultColor_1st.r, dConstants.UI.DefaultColor_1st.g, dConstants.UI.DefaultColor_1st.b, 0f);
    }
    
    public void GoToTitlePage()
    {
        gameObject.SetActive(true);
    }
    public void IntroLineCheck()
    {
        if(playerSettings.introCount <= levelRecords.finishedLevels.Count)
        {
            int rng = Random.Range(0, intro.lines.Count);
            LineEmerge(LINE_EMERGE_DURATION, intro.lines[rng]);
            playerSettings.introCount += 1;
        }
        else
        {
            OpenSelector();
        }
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
        str = SaveManager.controller.Inquire(string.Format(INTROCOUNT_SAVE_KEY));
        if (str != null)
        {
            int.TryParse(SaveManager.controller.Inquire(string.Format(INTROCOUNT_SAVE_KEY)), out playerSettings.introCount);
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
