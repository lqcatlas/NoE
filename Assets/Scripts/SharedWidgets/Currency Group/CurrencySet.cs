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

    public readonly float PARTICLE_MOVE_DURATION = .8f;
    public readonly float PARTICLE_MOVE_INTERVAL = .1f;
    public readonly int MAX_PARTICLE_COUNT = 8;

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
    public void StarParticleAnimation(int adjustAmount, Vector3 originalPos)
    {
        int particleCnt = Mathf.Min(adjustAmount, MAX_PARTICLE_COUNT);
        Sequence seq = DOTween.Sequence();
        //Debug.LogWarning($"particleGen with {particleCnt} particles.");
        for (int i = 0; i < particleCnt; i++)
        {
            GameObject tmp = Instantiate(starIcon, VFXHolder.singleton.transform);
            tmp.transform.position = originalPos;
            //tmp.transform.localScale = Vector3.one;
            float timePos = i * PARTICLE_MOVE_INTERVAL;
            seq.Insert(timePos, tmp.transform.DOMove(starIcon.transform.position, PARTICLE_MOVE_DURATION).SetEase(Ease.OutSine).OnComplete(()=>Destroy(tmp)));
        }
    }
    public void GemParticleAnimation(int adjustAmount, Vector3 originalPos)
    {
        int particleCnt = Mathf.Min(adjustAmount, MAX_PARTICLE_COUNT);
        Sequence seq = DOTween.Sequence();
        for (int i = 0; i < particleCnt; i++)
        {
            GameObject tmp = Instantiate(gemIcon, VFXHolder.singleton.transform);
            tmp.transform.position = originalPos;
            //tmp.transform.localScale = Vector3.one;
            float timePos = i * PARTICLE_MOVE_INTERVAL;
            seq.Insert(timePos, tmp.transform.DOMove(gemIcon.transform.position, PARTICLE_MOVE_DURATION).SetEase(Ease.OutSine));
        }
    }
}
