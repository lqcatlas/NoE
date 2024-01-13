using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BtnRetry : MonoBehaviour
{
    public void RetryBtnHoverEnter()
    {
        transform.DORotate(new Vector3(0f, 0f, 90f), dConstants.UI.StandardizedBtnAnimDuration);
    }
    public void RetryBtnHoverExit()
    {
        transform.DORotate(Vector3.zero, dConstants.UI.StandardizedBtnAnimDuration);
    }
}
