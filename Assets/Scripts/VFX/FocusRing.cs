using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusRing : MonoBehaviour
{
    [SerializeField] float ANIM_DURATION;
    [SerializeField] float START_SCALE;
    [SerializeField] float END_SCALE;
    [SerializeField] float START_ALPHA;
    [SerializeField] float END_ALPHA;
    private void OnEnable()
    {
        transform.localScale = Vector3.one * START_SCALE;
        Color clr = GetComponent<SpriteRenderer>().color;
        GetComponent<SpriteRenderer>().color = new Color(clr.r, clr.g, clr.b, START_ALPHA);

        transform.DOScale(END_SCALE, ANIM_DURATION);
        GetComponent<SpriteRenderer>().DOFade(END_ALPHA, ANIM_DURATION).OnComplete(()=>gameObject.SetActive(false));
    }

}
