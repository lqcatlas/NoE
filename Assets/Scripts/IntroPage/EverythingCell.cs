using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class EverythingCell : MonoBehaviour
{
    static float switchChance = .4f;
    static float CellFadeInTime = 1.0f;
    static float FadeDelayRange = 0.6f;
    [SerializeField] EverythingCellSpriteLib lib;
    [SerializeField] SpriteRenderer numberSprt;
    [SerializeField] SpriteRenderer frameSprt;
    public void UpdateSprite()
    {
        float rng = Random.Range(0f, 1f);
        if (rng > switchChance)
        {
            numberSprt.sprite = lib.GetRNGSprite();
        }
    }
    private void OnEnable()
    {
        CellFadeIn();
    }
    public void CellFadeIn()
    {
        numberSprt.color = new Color(dConstants.UI.DefaultColor_1st.r, dConstants.UI.DefaultColor_1st.g, dConstants.UI.DefaultColor_1st.b, 0);
        frameSprt.color = new Color(dConstants.UI.DefaultColor_3rd.r, dConstants.UI.DefaultColor_3rd.g, dConstants.UI.DefaultColor_3rd.b, 0);
        float rng = Random.Range(0f, FadeDelayRange);
        numberSprt.DOFade(1f, CellFadeInTime).SetDelay(rng);
        frameSprt.DOFade(1f, CellFadeInTime).SetDelay(rng);
    }
}
