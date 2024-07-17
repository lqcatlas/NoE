using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyscraperCellBg : MonoBehaviour
{
    [SerializeField] SpriteRenderer sprt;
    [SerializeField] List<Sprite> skyscraperSpritesByPhase;
    [SerializeField] Sprite toolSprite;

    private float ANIM_SPRT_SWITCH_DURATION = 1f;

    public void SetSpriteByPop(int pop)
    {
        sprt.sprite = GetSpriteByPop(pop);
    }
    public void UpdateSpriteByPop(int prevPop, int curPop)
    {
        Sprite prevSprt = GetSpriteByPop(prevPop);
        Sprite curSprt = GetSpriteByPop(curPop);
        if(prevSprt != curSprt) //trigger sprt switch VFX
        {
            sprt.DOFade(0f, ANIM_SPRT_SWITCH_DURATION / 2f).OnComplete(()=> sprt.sprite = curSprt);
            sprt.DOFade(1f, ANIM_SPRT_SWITCH_DURATION / 2f).SetDelay(ANIM_SPRT_SWITCH_DURATION / 2f);
        }
    }
    Sprite GetSpriteByPop(int pop)
    {
        if (pop == 0)
        {
            return skyscraperSpritesByPhase[0];
        }
        else if (pop >= 1 && pop <= 4)
        {
            return skyscraperSpritesByPhase[1];
        }
        else if (pop >= 5 && pop <= 9)
        {
            return skyscraperSpritesByPhase[2];
        }
        else if (pop >= 10 && pop <= 14)
        {
            return skyscraperSpritesByPhase[3];
        }
        else if (pop >= 15 && pop <= 29)
        {
            return skyscraperSpritesByPhase[4];
        }
        else if (pop >= 30 && pop <= 49)
        {
            return skyscraperSpritesByPhase[5];
        }
        else if (pop >= 50)
        {
            return skyscraperSpritesByPhase[6];
        }
        else
        {
            Debug.LogError($"invalid population value as {pop} in SkyscraperCellBg script");
            return skyscraperSpritesByPhase[0];
        }
    }
}
