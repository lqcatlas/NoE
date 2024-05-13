using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ObjScaleInOut : MonoBehaviour
{
    //must be used on obj with unified scale on 3 dimensions
    //call scaleInOut to enable/disable with Anim
    [SerializeField] float originalScale;
    Sequence seq;
    public void ScaleIn()
    {
        DOTween.Kill(seq);
        transform.localScale = Vector3.zero;
        gameObject.SetActive(true);
        seq = DOTween.Sequence();
        seq.Append(transform.DOScale(originalScale, dConstants.UI.StandardizedBtnAnimDuration));
    }
    public void ScaleOut()
    {
        DOTween.Kill(seq);
        transform.localScale = Vector3.one * originalScale;
        gameObject.SetActive(true);
        seq = DOTween.Sequence();
        seq.Append(transform.DOScale(0f, dConstants.UI.StandardizedBtnAnimDuration));
        seq.AppendCallback(() => gameObject.SetActive(false));
    }
}
