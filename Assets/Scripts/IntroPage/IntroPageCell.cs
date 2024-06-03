using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class IntroPageCell : MonoBehaviour
{
    [Header("Static Params")]
    static float CellFadeOutTime = 3.0f;
    static float CellFadeInTime = 1.0f;
    static float MaskFadeTime = 0.3f;
    static float MaskFadeAlpha = 0.5f;
    [Header("Children Objs")]
    [SerializeField] BoxCollider2D cellCollider;
    [SerializeField] SpriteRenderer numberSprt;
    [SerializeField] SpriteRenderer frameSprt;
    [SerializeField] SpriteRenderer maskSprt;
    public void MaskFadeIn()
    {
        maskSprt.DOFade(MaskFadeAlpha, MaskFadeTime);
    }
    public void MaskFadeOut()
    {
        maskSprt.DOFade(0f, MaskFadeTime);
    }
    public void CellFadeOut()
    {
        cellCollider.enabled = false;
        numberSprt.color = dConstants.UI.DefaultColor_1st;
        frameSprt.color = dConstants.UI.DefaultColor_3rd;
        numberSprt.DOFade(0f, CellFadeOutTime);
        frameSprt.DOFade(0f, CellFadeOutTime);
        //maskSprt.DOFade(0f, CellFadeTime);
    }
    public void CellFadeIn()
    {
        numberSprt.color = new Color(dConstants.UI.DefaultColor_1st.r, dConstants.UI.DefaultColor_1st.g, dConstants.UI.DefaultColor_1st.b, 0);
        frameSprt.color = new Color(dConstants.UI.DefaultColor_3rd.r, dConstants.UI.DefaultColor_3rd.g, dConstants.UI.DefaultColor_3rd.b, 0);
        numberSprt.DOFade(1f, CellFadeInTime);
        frameSprt.DOFade(1f, CellFadeInTime).OnComplete(()=> cellCollider.enabled = true);
    }
}
