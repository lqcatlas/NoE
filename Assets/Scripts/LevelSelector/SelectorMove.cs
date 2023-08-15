using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorMove : MonoBehaviour
{
    [SerializeField] float movingSpeedTier1;
    [SerializeField] float movingSpeedTier2;
    [SerializeField] Vector3 moveDirection;

    [SerializeField] Transform movingGroup;
    [SerializeField] SpriteRenderer bar;

    private bool tier1;
    private bool tier2;

    private void Start()
    {
        BarInit();
    }

    private void FixedUpdate()
    {
        if (tier2 && tier1)
        {
            movingGroup.GetComponent<RectTransform>().localPosition += moveDirection * Time.fixedDeltaTime * movingSpeedTier2;
        }
        else if (tier1)
        {
            movingGroup.GetComponent<RectTransform>().localPosition += moveDirection * Time.fixedDeltaTime * movingSpeedTier1;
        }
    }
    void BarInit()
    {
        tier1 = false;
        tier2 = false;
        bar.color = new Color(1f, 1f, 1f, 0f);
    }
    public void HoverOn()
    {
        tier1 = true;
        bar.DOFade(0.6f, dConstants.UI.StandardizedBtnAnimDuration);
    }
    public void HoverOff()
    {
        tier1 = false;
        bar.DOFade(0f, dConstants.UI.StandardizedBtnAnimDuration);
    }
    public void MouseDown()
    {
        tier2 = true;
    }
    public void MouseUp()
    {
        tier2 = false;
    }
}
