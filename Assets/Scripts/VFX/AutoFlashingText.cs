using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AutoFlashingText : MonoBehaviour
{
    private float maxAlpha;
    [SerializeField] float flashingTime = 0.5f;
    // Start is called before the first frame update
    void OnEnable()
    {
        if (flashingTime == 0)
        {
            flashingTime = 0.5f;
        }
        maxAlpha = GetComponent<TextMeshPro>().color.a;
        GetComponent<TextMeshPro>().DOFade(0f, flashingTime).From().SetLoops(-1, LoopType.Yoyo);
    }
    void OnDisable()
    {
        GetComponent<TextMeshPro>().DOKill();
        Color EndColor = GetComponent<TextMeshPro>().color;
        GetComponent<TextMeshPro>().color = new Color(EndColor.r, EndColor.g, EndColor.b, maxAlpha);
    }
}
