using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RuleTagAnim : MonoBehaviour
{
    [SerializeField] TextMeshPro tmp;
    static float CYCLE_DURATRION = 1.5f;
    Sequence seq;
    private void OnEnable()
    {
        seq = DOTween.Sequence();
        tmp.color = dConstants.UI.DefaultColor_1st;
        seq.Append(tmp.DOColor(dConstants.UI.DefaultColor_3rd, CYCLE_DURATRION).SetLoops(-1, LoopType.Yoyo));
    }
    private void OnDisable()
    {
        seq.Kill();
    }
}
