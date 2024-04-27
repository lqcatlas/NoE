using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorMove : MonoBehaviour
{
    [SerializeField] float movingSpeedTier1;
    [SerializeField] float movingSpeedTier2;
    [SerializeField] Vector3 moveDirection;
    [SerializeField] Vector2 movingLimits;
    public bool reachEnding;

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
        if (reachEnding)
        {
            //do nothing
        }
        else if (tier2 && tier1)
        {
            movingGroup.GetComponent<RectTransform>().localPosition += moveDirection * Time.fixedDeltaTime * movingSpeedTier2;
        }
        else if (tier1)
        {
            movingGroup.GetComponent<RectTransform>().localPosition += moveDirection * Time.fixedDeltaTime * movingSpeedTier1;
        }
        /*if (movingGroup.GetComponent<RectTransform>().localPosition.x <= movingLimits.x)
        {
            movingGroup.GetComponent<RectTransform>().localPosition -= new Vector3(1f, 0f, 0f) * (movingGroup.GetComponent<RectTransform>().localPosition.x - movingLimits.x);
        }
        else if (movingGroup.GetComponent<RectTransform>().localPosition.x >= movingLimits.y)
        {
            movingGroup.GetComponent<RectTransform>().localPosition -= new Vector3(1f, 0f, 0f) * (movingGroup.GetComponent<RectTransform>().localPosition.x - movingLimits.y);
        }*/
    }
    void BarInit()
    {
        tier1 = false;
        tier2 = false;
        bar.color = new Color(1f, 1f, 1f, 0f);
        reachEnding = false;
    }
    public void HoverOn()
    {
        tier1 = true;
        tier2 = false;
        bar.DOFade(0.4f, dConstants.UI.StandardizedBtnAnimDuration);
    }
    public void HoverOff()
    {
        tier1 = false;
        tier2 = false;
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
