using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EclipseAnim : MonoBehaviour
{
    [SerializeField] float ECLIPSE_ANIM_DURATION = 1f;
    [SerializeField] float FLASHING_ANIM_DURATION = 0.3f;
    [SerializeField] GameObject moon;
    [SerializeField] GameObject moonMask;
    [SerializeField] GameObject fadeMask;
    [SerializeField] GameObject flashingMoon;
    [SerializeField] Vector3 startingPos;

    private void OnEnable()
    {
        //reset
        moonMask.transform.position = startingPos;
        fadeMask.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0f);
        moon.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        flashingMoon.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 0f);

        moonMask.transform.DOMoveX(0f, ECLIPSE_ANIM_DURATION).SetEase(Ease.OutCubic);
        moon.GetComponent<SpriteRenderer>().DOFade(1f, ECLIPSE_ANIM_DURATION).SetEase(Ease.OutCubic);
        fadeMask.GetComponent<SpriteRenderer>().DOFade(1f, ECLIPSE_ANIM_DURATION).SetEase(Ease.OutCubic);

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(ECLIPSE_ANIM_DURATION);
        seq.Append(moon.GetComponent<SpriteRenderer>().DOColor(Color.black, FLASHING_ANIM_DURATION).SetLoops(3, LoopType.Yoyo).SetEase(Ease.InOutCubic));
        seq.Join(fadeMask.GetComponent<SpriteRenderer>().DOColor(Color.gray, FLASHING_ANIM_DURATION).SetLoops(3, LoopType.Yoyo).SetEase(Ease.InOutCubic));
        seq.Join(flashingMoon.GetComponent<SpriteRenderer>().DOFade(1f, FLASHING_ANIM_DURATION).SetLoops(3, LoopType.Yoyo).SetEase(Ease.InOutCubic));
        //seq.AppendInterval(FLASHING_ANIM_DURATION * 2);
        seq.AppendCallback(() => gameObject.SetActive(false));
    }
}
