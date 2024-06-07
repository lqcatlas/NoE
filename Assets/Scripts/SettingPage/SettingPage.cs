using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingPage : MonoBehaviour
{
    static public SettingPage singleton;
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
        page.SetActive(false);
    }
    [SerializeField] GameObject page;
    [SerializeField] PlayerSettings playerSettings;
    [SerializeField] TextMeshPro musicVol;
    [SerializeField] TextMeshPro soundVol;
    public void GotoSettingPage()
    {
        musicVol.SetText(string.Format("{0}", Mathf.RoundToInt(playerSettings.musicVolume * 10)));
        soundVol.SetText(string.Format("{0}", Mathf.RoundToInt(playerSettings.soundVolume * 10)));

        page.transform.localScale = Vector3.one;
        page.transform.DOScale(0f, dConstants.UI.StandardizedBtnAnimDuration).From();
        page.SetActive(true);
    }
    public void MusicVolumeAdjust(int vol)
    {
        playerSettings.musicVolume = Mathf.Max(0f, Mathf.Min(1f, playerSettings.musicVolume + vol * 0.1f));
        AudioDraft.singleton.VolumeReset();
        AudioDraft.singleton.PlayGenericPlaySFX();
        musicVol.SetText(string.Format("{0}", Mathf.RoundToInt(playerSettings.musicVolume * 10)));

    }
    public void SoundVolumeAdjust(int vol)
    {
        playerSettings.soundVolume = Mathf.Max(0f, Mathf.Min(1f, playerSettings.soundVolume + vol * 0.1f));
        AudioDraft.singleton.VolumeReset();
        AudioDraft.singleton.PlayGenericPlaySFX();
        soundVol.SetText(string.Format("{0}", Mathf.RoundToInt(playerSettings.soundVolume * 10)));
    }
    public void ClosePage()
    {
        page.transform.DOScale(0f, dConstants.UI.StandardizedBtnAnimDuration).OnComplete(() => page.SetActive(false));
    }
}
