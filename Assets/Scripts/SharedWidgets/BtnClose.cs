using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BtnClose : MonoBehaviour
{
    public float SCALE_ORIGINAL = 5f;
    public float SCALE_HOVERON = 6f;

    public void CloseBtnHoverEnter()
    {
        transform.DOScale(Vector3.one * SCALE_HOVERON, dConstants.UI.StandardizedBtnAnimDuration);
    }
    public void CloseBtnHoverExit()
    {
        transform.DOScale(Vector3.one * SCALE_ORIGINAL, dConstants.UI.StandardizedBtnAnimDuration);
    }
}
