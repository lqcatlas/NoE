using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class ConfirmBtn : MonoBehaviour
{
    [SerializeField] SpriteRenderer fill;
    [SerializeField] SpriteRenderer frame;

    public void HoverOn()
    {
        fill.DOFade(0.4f, dConstants.UI.StandardizedBtnAnimDuration);
    }

    public void HoverOff()
    {
        fill.DOFade(0f, dConstants.UI.StandardizedBtnAnimDuration);
    }
}
