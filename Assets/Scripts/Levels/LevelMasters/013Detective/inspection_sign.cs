using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inspection_sign : MonoBehaviour
{
    public int ispStatus;
    public SpriteRenderer sr;
    public List<Sprite> ispSprites;

    private Sequence seq;

    public void SetSign(int status)
    {
        ispStatus = status;
        sr.sprite = ispSprites[status];
    }
    public void UpdateSign(int status)
    {        
        ispStatus = status;
        seq.Kill();
        seq = DOTween.Sequence();
        seq.Append(sr.DOFade(0f, dConstants.UI.StandardizedVFXAnimDuration / 2f).OnComplete(()=> sr.sprite = ispSprites[status]));
        seq.Append(sr.DOFade(1f, dConstants.UI.StandardizedVFXAnimDuration / 2f));
    }
}
