using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExplodeAnim_Flask : MonoBehaviour
{
    [SerializeField] GameObject inner;
    [SerializeField] GameObject outer;
    [SerializeField] GameObject number;
    [SerializeField] TextMeshPro reason;
    [Header("Anim Params")]
    //[SerializeField] float INNER_ROTATION = 30f;
    [SerializeField] float INNER_DURATION = 1.2f;
    [SerializeField] float NUMBER_DURATION = 1.4f;
    [SerializeField] float OUTER_DURATION = 1.8f;
    [SerializeField] float HOLD_DURATION = 3.6f;
    [SerializeField] float SHRINK_DURATION = 3.8f;
    public void SetReason(string txt)
    {
        reason.SetText(LocalizedAssetLookup.singleton.Translate(txt));
    }
    private void OnEnable()
    {
        inner.transform.DORotate(Vector3.zero, OUTER_DURATION).SetEase(Ease.InCubic);
        //Debug.Log("play War animation TODO");
        Sequence seq = DOTween.Sequence();
        seq.Append(inner.transform.DOScale(0f, INNER_DURATION).From().SetEase(Ease.InCubic));
        seq.Join(number.transform.DOScale(0f, NUMBER_DURATION).From().SetEase(Ease.InCubic));
        seq.Append(outer.transform.DOScale(0f, OUTER_DURATION - INNER_DURATION).From().SetEase(Ease.InCubic));
        seq.Append(outer.transform.DOScale(1.05f, HOLD_DURATION - OUTER_DURATION));
        seq.Append(transform.DOScale(Vector3.zero, SHRINK_DURATION - HOLD_DURATION));
        seq.AppendCallback(() => Destroy(gameObject));
        //sword2.GetComponent<SpriteRenderer>().DOFade(0f, ONE_HIT_DURATION).SetEase(Ease.OutCubic).SetLoops(2, LoopType.Restart)
    }
}
