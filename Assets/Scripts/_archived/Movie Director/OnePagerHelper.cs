using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class OnePagerHelper : MonoBehaviour
{
    public float stayTime = 2f;
    public float transitionTime = 0.6f;
    public float totalTime = 2f * 3f + 0.6f * 11f;

    [SerializeField] Image title1;
    [SerializeField] Image title2;

    [SerializeField] TextMeshProUGUI line1;
    [SerializeField] TextMeshProUGUI line2;
    [SerializeField] TextMeshProUGUI line3;

    private void Start()
    {
        float delayTotal = 0f;
        delayTotal += transitionTime;
        DOTween.ToAlpha(() => title1.color, x => title1.color = x, 0f, transitionTime).SetDelay(delayTotal);
        DOTween.ToAlpha(() => title2.color, x => title2.color = x, 0f, transitionTime).SetDelay(delayTotal);
        delayTotal += transitionTime;

        DOTween.ToAlpha(() => line1.color, x => line1.color = x, 1f, transitionTime).SetDelay(delayTotal);
        delayTotal += transitionTime + stayTime;
        DOTween.ToAlpha(() => line1.color, x => line1.color = x, 0f, transitionTime).SetDelay(delayTotal);
        delayTotal += transitionTime + transitionTime;
        DOTween.ToAlpha(() => line2.color, x => line2.color = x, 1f, transitionTime).SetDelay(delayTotal);
        delayTotal += transitionTime + stayTime;
        DOTween.ToAlpha(() => line2.color, x => line2.color = x, 0f, transitionTime).SetDelay(delayTotal);
        delayTotal += transitionTime + transitionTime;
        DOTween.ToAlpha(() => line3.color, x => line3.color = x, 1f, transitionTime).SetDelay(delayTotal);
        delayTotal += transitionTime + stayTime;
        //DOTween.ToAlpha(() => line3.color, x => line3.color = x, 0f, transitionTime).SetDelay(delayTotal);
        //delayTotal += transitionTime + transitionTime;

    }
}
