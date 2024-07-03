using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCentralCtrl : MonoBehaviour
{
    static public AudioCentralCtrl singleton;
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
    
    [Header("Generic Lib")]
    [SerializeField] GenericSoundLibrary lib;
    [SerializeField] ThemeResourceLookup themeResources;
    //[SerializeField] AudioClip genericBgMusic;

    [Header("Player Settings")]
    [SerializeField] PlayerSettings playerSettings;
    [SerializeField] float MAX_MUSIC_VOLUME = 0.3f;
    [SerializeField] float MAX_SFX_VOLUME = 0.7f;

    [Header("Children Objs")]
    [SerializeField] List<AudioClip> keynotes;
    [SerializeField] AudioSource genericBgSource;
    [SerializeField] AudioSource themeBgSource;
    [SerializeField] AudioSource keynoteSource;
    [SerializeField] List<AudioSource> SFXSources;

    private bool genericBgInUse;
    private Sequence seq;
    // Start is called before the first frame update
    void Start()
    {
        //IntroStart();
        VolumeReset();
        //BgMusicStart();
    }

    AudioSource GetValidSFXSource()
    {
        for (int i = 0; i < SFXSources.Count; i++)
        {
            if (!SFXSources[i].isPlaying)
            {
                return SFXSources[i];
            }
        }
        Debug.LogError("no spare SFX player");
        return null;
    }
    public void PlayGenericPlaySFX()
    {
        PlaySFX(lib.GetPlaySFX());
    }
    public void PlayGenericLevelWinSFX()
    {
        PlaySFX(lib.GetLevelWinSFX());
    }
    public void PlayGenericLevelFailSFX()
    {
        PlaySFX(lib.GetLevelFailSFX());
    }
    public void PlaySFX(AudioClip _clip)
    {
        if (_clip)
        {
            AudioSource source = GetValidSFXSource();
            source.clip = _clip;
            source.Play();
            source.DOFade(0f, _clip.length * 0.2f).From();
        }
        else
        {
            Debug.LogError("playSFX play with null audio clip");
        }
    }
    public void PlayKeynote(int index)
    {
        //check if a keynote item is allocated to the given index
        if (keynotes.Count > index)
        {
            //check if a keynote clip is given
            if (keynotes[index])
            {
                AudioSource source = keynoteSource;
                if (source)
                {
                    source.Stop();
                    source.volume = 1f;
                    source.clip = keynotes[index];
                    source.Play();
                    source.DOFade(0f, 0.3f).From();
                }                
            }
        }
    }
    public void BgMusicStart()
    {
        //IntroEnd();
        if (!genericBgSource.gameObject.activeSelf)
        {
            genericBgInUse = true;
            genericBgSource.gameObject.SetActive(true);
            //genericBgSource.clip = genericBgMusic;
            genericBgSource.volume = MAX_MUSIC_VOLUME * playerSettings.audioVolume * playerSettings.musicVolume;
            genericBgSource.DOFade(0f, 5f).From();
        }
    }
    public void BgMusicSwitch(int themeIndex = 0)
    {
        AudioClip newClip = themeResources.GetThemeMusic(themeIndex);
        if(!genericBgInUse && newClip == null)
        {
            genericBgInUse = true;
            float targetVol = MAX_MUSIC_VOLUME * playerSettings.audioVolume * playerSettings.musicVolume;
            genericBgSource.volume = 0f;
            genericBgSource.gameObject.SetActive(true);
            seq.Kill();
            seq = DOTween.Sequence();
            seq.Append(genericBgSource.DOFade(targetVol, 2f).SetDelay(4f));
            seq.Join(themeBgSource.DOFade(0f, 5f));
            seq.AppendCallback(() => themeBgSource.gameObject.SetActive(false));
            
            
        }
        else if(genericBgInUse && newClip != null)
        {
            genericBgInUse = false;
            themeBgSource.clip = newClip; 
            float targetVol = MAX_MUSIC_VOLUME * playerSettings.audioVolume * playerSettings.musicVolume;
            themeBgSource.volume = 0f;
            themeBgSource.gameObject.SetActive(true);
            themeBgSource.Play();
            seq.Kill();
            seq = DOTween.Sequence();
            seq.Append(themeBgSource.DOFade(targetVol, 2f).SetDelay(4f));
            seq.Join(genericBgSource.DOFade(0f, 5f));
            seq.AppendCallback(() => genericBgSource.gameObject.SetActive(false));

        }
        else
        {
            //do nothing
        }
        
    }
    public void VolumeReset()
    {
        float musicVol = MAX_MUSIC_VOLUME * playerSettings.audioVolume * playerSettings.musicVolume;
        float sfxVol = MAX_SFX_VOLUME * playerSettings.audioVolume * playerSettings.soundVolume;
        genericBgSource.volume = musicVol;
        themeBgSource.volume = musicVol;
        keynoteSource.volume = sfxVol;
        for (int i = 0; i < SFXSources.Count; i++)
        {
            SFXSources[i].volume = sfxVol;
        }
    }
}
