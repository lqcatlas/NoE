using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencySet : MonoBehaviour
{
    public LevelRecords playingRecords;
    public TextMeshPro curStarCount;
    public TextMeshPro gemCount;
    public GameObject starIcon;
    public GameObject gemIcon;
    public Transform gemGroup;

    Sequence seqStar;
    Sequence seqGem;

    //public float ADJUST_ANIM_DURATION = .3f;

    public void StarCountAdjustAnimation(int adjustAmount, bool preview = false)
    {
        seqStar.Kill();
        seqStar = DOTween.Sequence();
        int step = Mathf.Min(5, Mathf.Abs(adjustAmount));
        for (int i = 0; i < step; i++)
        {
            int displayCount = playingRecords.tokens - (preview ? 0 : adjustAmount) + (adjustAmount > 0 ? Mathf.FloorToInt((float)adjustAmount / step * i) : Mathf.CeilToInt((float)adjustAmount / step * i));
            //Debug.Log("tokens " + playingRecords.tokens);
            //Debug.Log("display number " + displayCount);
            seqStar.AppendCallback(() => curStarCount.SetText(displayCount.ToString()));
            seqStar.AppendInterval(dConstants.UI.StandardizedBtnAnimDuration / step);
        }
        seqStar.AppendCallback(() => curStarCount.SetText((playingRecords.tokens + (preview ? adjustAmount : 0)).ToString()));
    }

    public void GemCountAdjustAnimation(int adjustAmount)
    {
        seqGem.Kill();
        seqGem = DOTween.Sequence();
        int step = Mathf.Min(5, Mathf.Abs(adjustAmount));
        for (int i = 0; i < step; i++)
        {
            int displayCount = playingRecords.gems - adjustAmount + (adjustAmount > 0 ? Mathf.FloorToInt((float)adjustAmount / step * i) : Mathf.CeilToInt((float)adjustAmount / step * i));
            seqGem.AppendCallback(() => gemCount.SetText(displayCount.ToString()));
            seqGem.AppendInterval(dConstants.UI.StandardizedBtnAnimDuration / step);
        }
        seqGem.AppendCallback(() => gemCount.SetText(playingRecords.gems.ToString()));
    }
}
