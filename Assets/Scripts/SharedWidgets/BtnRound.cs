using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static SelectorNode;

public class BtnRound : MonoBehaviour
{
    [Header("Children Objs")]
    [SerializeField] SpriteRenderer fill;
    [SerializeField] SpriteRenderer frame;

    private void OnEnable()
    {
        fill.gameObject.SetActive(true);
        fill.color = new Color(1f, 1f, 1f, 0f);
        fill.transform.localScale = Vector3.zero;
        frame.gameObject.SetActive(true);
    }
    public void HoverOn()
    {
        fill.DOFade(0.8f, dConstants.UI.StandardizedBtnAnimDuration);
        fill.transform.DOScale(1f, dConstants.UI.StandardizedBtnAnimDuration);
        frame.DOFade(0f, dConstants.UI.StandardizedBtnAnimDuration);
    }
    public void HoverOff()
    {
        fill.DOFade(0f, dConstants.UI.StandardizedBtnAnimDuration);
        fill.transform.DOScale(0f, dConstants.UI.StandardizedBtnAnimDuration);
        frame.DOFade(1f, dConstants.UI.StandardizedBtnAnimDuration);

    }
}
