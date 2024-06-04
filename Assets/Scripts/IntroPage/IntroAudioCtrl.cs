using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroAudioCtrl : MonoBehaviour
{
    static float FADE_IN_DURATION = 1f;
    static float FADE_OUR_DURATION = 2f;
    public List<AudioSource> introSource;

    public void PlaySource(int index)
    {
        introSource[index].gameObject.SetActive(true);
        introSource[index].Play();
        introSource[index].DOFade(0f, FADE_IN_DURATION).From();
    }
    public void FadeOutALL()
    {
        for(int i=0; i<introSource.Count; i++)
        {
            introSource[i].DOFade(0f, FADE_OUR_DURATION).OnComplete(() => introSource[i].gameObject.SetActive(false));
        }
    }
}
