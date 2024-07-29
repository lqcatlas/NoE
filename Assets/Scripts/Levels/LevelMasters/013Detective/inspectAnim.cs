using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inspectAnim : MonoBehaviour
{
    [SerializeField] Transform axis;
    [SerializeField] Transform icon;
    static float INSPECT_CYCLE_DURATION = .6f;
    static int CYCLE_TIME = 12;
    private Sequence seq;
    private void OnEnable()
    {
        seq.Kill();
        seq = DOTween.Sequence();
        seq.Append(axis.DOLocalRotate(new Vector3(0f, 0f, 360f), INSPECT_CYCLE_DURATION).SetRelative().SetLoops(CYCLE_TIME).SetEase(Ease.Linear));
        seq.Insert(0f, icon.DOLocalRotate(new Vector3(0f, 0f, -360f), INSPECT_CYCLE_DURATION).SetRelative().SetLoops(CYCLE_TIME).SetEase(Ease.Linear));
    }
}
