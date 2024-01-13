using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BtnRewind : MonoBehaviour
{
    public void RewindBtnHoverEnter()
    {
        transform.DORotate(new Vector3(0f, 0f, 15f), dConstants.UI.StandardizedBtnAnimDuration);
    }
    public void RewindBtnHoverExit()
    {
        transform.DORotate(Vector3.zero, dConstants.UI.StandardizedBtnAnimDuration);
    }
}
