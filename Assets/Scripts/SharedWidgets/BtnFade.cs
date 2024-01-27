using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BtnFade : MonoBehaviour
{
    [SerializeField] float DefaultAlpha = 0.7f;
    public void HoverOn()
    {
        GetComponent<SpriteRenderer>().DOFade(1f, dConstants.UI.StandardizedBtnAnimDuration);
        //frame.DOFade(1f, dConstants.UI.StandardizedBtnAnimDuration);
        //fill.transform.DOScale(1f, dConstants.UI.StandardizedBtnAnimDuration);
        //frame.DOFade(0f, dConstants.UI.StandardizedBtnAnimDuration);
    }
    public void HoverOff()
    {
        GetComponent<SpriteRenderer>().DOFade(DefaultAlpha, dConstants.UI.StandardizedBtnAnimDuration);
        //frame.DOFade(0.6f, dConstants.UI.StandardizedBtnAnimDuration);
        //fill.transform.DOScale(0f, dConstants.UI.StandardizedBtnAnimDuration);
        //frame.DOFade(1f, dConstants.UI.StandardizedBtnAnimDuration);

    }
}
