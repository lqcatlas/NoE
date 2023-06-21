using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class Curtain : MonoBehaviour
{
    [SerializeField] float OnTime = 0.5f;
    [SerializeField] float OffTime = 3f;
    [SerializeField] float OffDelay = 1f;
    public void CurtainOn()
    {
        GetComponent<Image>().DOKill();
        GetComponent<Image>().DOFade(1f, OnTime);
        GetComponent<Image>().raycastTarget = true;
    }
    public void CurtainOff()
    {
        //GetComponent<Image>().DOKill();
        GetComponent<Image>().DOFade(0f, OffTime).SetDelay(OffDelay).SetEase(Ease.InSine).OnComplete(() => { GetComponent<Image>().raycastTarget = false; });
    }
}
