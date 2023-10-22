using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgCtrl : MonoBehaviour
{
    [SerializeField] GameObject HugeTextures;
    [SerializeField] GameObject RandomThemeIcon;
    [SerializeField] GameObject ThemeIconNoise;
    
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
        switch (phase)
        {
            case dConstants.Gameplay.GamePhase.Title:
                HugeTextures.SetActive(true); ;
                RandomThemeIcon.SetActive(false);
                ThemeIconNoise.SetActive(true);
                break;
            case dConstants.Gameplay.GamePhase.Selector:
                HugeTextures.SetActive(true); ;
                RandomThemeIcon.SetActive(true);
                ThemeIconNoise.SetActive(true);
                break;
            case dConstants.Gameplay.GamePhase.Level:
                HugeTextures.SetActive(true); ;
                RandomThemeIcon.SetActive(false);
                ThemeIconNoise.SetActive(true);
                break;
            default:
                HugeTextures.SetActive(true); ;
                RandomThemeIcon.SetActive(false);
                ThemeIconNoise.SetActive(true);
                break;
        }
    }
}
