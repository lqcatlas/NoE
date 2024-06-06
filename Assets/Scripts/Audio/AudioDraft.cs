using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioDraft : MonoBehaviour
{
    static public AudioDraft singleton;
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

    [Header("Player Settings")]
    [SerializeField] PlayerSettings playerSettings;
    [SerializeField] float MAX_MUSIC_VOLUME = 0.4f;
    [SerializeField] float MAX_SFX_VOLUME = 0.6f;

    [Header("Children Objs")]
    [SerializeField] List<AudioClip> keynotes;
    [SerializeField] AudioSource introSource;
    [SerializeField] AudioSource puzzleSource;
    [SerializeField] AudioSource keynoteSource;
    [SerializeField] List<AudioSource> SFXSources;
    // Start is called before the first frame update
    void Start()
    {
        //IntroStart();
        VolumeReset();
        //PuzzleMusicStart();
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
    public void IntroStart()
    {
        introSource.gameObject.SetActive(true);
        introSource.DOFade(0.2f, 3f);
    }
    public void IntroContinue()
    {
        introSource.DOKill();
        introSource.DOFade(0.5f, 3f);
    }
    public void IntroEnd()
    {
        introSource.DOFade(0f, 3f);
    }
    public void PuzzleMusicStart()
    {
        //IntroEnd();
        if (!puzzleSource.gameObject.activeSelf)
        {
            puzzleSource.gameObject.SetActive(true);
            puzzleSource.volume = MAX_MUSIC_VOLUME * playerSettings.audioVolume * playerSettings.musicVolume;
            puzzleSource.DOFade(0f, 5f).From();
        }
    }
    public void VolumeReset()
    {
        puzzleSource.volume = MAX_MUSIC_VOLUME * playerSettings.audioVolume * playerSettings.musicVolume;
        for (int i = 0; i < SFXSources.Count; i++)
        {
            SFXSources[i].volume = MAX_SFX_VOLUME * playerSettings.audioVolume * playerSettings.soundVolume;
        }
    }
}
