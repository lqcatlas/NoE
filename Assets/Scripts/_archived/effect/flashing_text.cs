using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class flashing_text : MonoBehaviour
{
    private float maxAlpha;
    [SerializeField] float flashingTime = 0.5f;
    // Start is called before the first frame update
    void OnEnable()
    {
        if(flashingTime == 0)
        {
            flashingTime = 0.5f;
        }
        maxAlpha = GetComponent<TextMeshProUGUI>().color.a;
        GetComponent<TextMeshProUGUI>().DOFade(0f, flashingTime).From().SetLoops(-1,LoopType.Yoyo);
    }
    void OnDisable()
    {
        GetComponent<TextMeshProUGUI>().DOKill();
        Color EndColor = GetComponent<TextMeshProUGUI>().color;
        GetComponent<TextMeshProUGUI>().color = new Color(EndColor.r, EndColor.g, EndColor.b, maxAlpha);
    }
}
