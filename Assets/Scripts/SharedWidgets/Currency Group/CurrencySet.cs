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

    //public float ADJUST_ANIM_DURATION = .3f;

    public void StarCountAdjustAnimation(int adjustAmount)
    {
        Sequence seq = DOTween.Sequence();
        int step = Mathf.Min(5, Mathf.Abs(adjustAmount));
        for (int i = 0; i < step; i++)
        {
            int displayCount = playingRecords.tokens - adjustAmount + (adjustAmount > 0 ? Mathf.FloorToInt((float)adjustAmount / step * i) : -Mathf.CeilToInt((float)adjustAmount / step * i));
            //Debug.Log("tokens " + playingRecords.tokens);
            //Debug.Log("display number " + displayCount);
            seq.AppendCallback(() => curStarCount.SetText(displayCount.ToString()));
            seq.AppendInterval(dConstants.UI.StandardizedBtnAnimDuration / step);
        }
        seq.AppendCallback(() => curStarCount.SetText(playingRecords.tokens.ToString()));
    }

    public void GemCountAdjustAnimation(int adjustAmount)
    {
        Sequence seq = DOTween.Sequence();
        int step = Mathf.Min(5, Mathf.Abs(adjustAmount));
        for (int i = 0; i < step; i++)
        {
            int displayCount = playingRecords.gems - adjustAmount + (adjustAmount > 0 ? Mathf.FloorToInt((float)adjustAmount / step * i) : -Mathf.CeilToInt((float)adjustAmount / step * i));
            seq.AppendCallback(() => gemCount.SetText(displayCount.ToString()));
            seq.AppendInterval(dConstants.UI.StandardizedBtnAnimDuration / step);
        }
        seq.AppendCallback(() => gemCount.SetText(playingRecords.gems.ToString()));
    }
}
