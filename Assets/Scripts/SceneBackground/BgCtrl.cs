using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgCtrl : MonoBehaviour
{
    [SerializeField] GameObject HugeTextures;
    [SerializeField] GameObject RandomThemeIcon;
    [SerializeField] GameObject ThemeIconNoise;
    [SerializeField] GameObject ScratchedBlackboard;
    [SerializeField] SpriteRenderer ScratchedTopMask;
    [SerializeField] SpriteRenderer LightBackground;

    static public BgCtrl singleton;
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
        SetToPhase(dConstants.Gameplay.GamePhase.Title);
    }
    public void SetToPhase(dConstants.Gameplay.GamePhase phase)
    {
        HugeTextures.SetActive(false);
        RandomThemeIcon.SetActive(false);
        ThemeIconNoise.SetActive(false);
        ScratchedBlackboard.SetActive(false);
        switch (phase)
        {
            case dConstants.Gameplay.GamePhase.Title:
                //HugeTextures.SetActive(true);
                ThemeIconNoise.SetActive(true);
                ScratchedBlackboard.SetActive(true);
                break;
            case dConstants.Gameplay.GamePhase.Selector:
                HugeTextures.SetActive(true); ;
                RandomThemeIcon.SetActive(true);
                ThemeIconNoise.SetActive(true);
                ScratchedBlackboard.SetActive(true);
                break;
            case dConstants.Gameplay.GamePhase.Level:
                HugeTextures.SetActive(true); ;
                ThemeIconNoise.SetActive(true);
                break;
            default:
                HugeTextures.SetActive(true); ;
                ThemeIconNoise.SetActive(true);
                break;
        }
    }
    public void SetToLightBg(float fadeTime = 0.05f)
    {
        LightBackground.DOFade(1f, fadeTime);
    }
    public void ResetToDefaultBg(float fadeTime = 0.05f)
    {
        LightBackground.DOFade(0f, fadeTime);
    }
    public void TopMaskFadeIn(float fadeTime = 0.05f)
    {
        ScratchedTopMask.color = new Color(.4f, .4f, .4f, 0f);
        ScratchedTopMask.DOFade(1f, fadeTime);
    }
    public void TopMaskFadeOut(float fadeTime = 0.05f)
    {
        ScratchedTopMask.color = new Color(.4f, .4f, .4f, 1f);
        ScratchedTopMask.DOFade(0f, fadeTime);
    }
}
