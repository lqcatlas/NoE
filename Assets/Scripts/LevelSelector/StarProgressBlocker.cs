using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class StarProgressBlocker : MonoBehaviour
{
    [SerializeField] LevelRecords records;
    [SerializeField] int requiredStars;
    [SerializeField] TextMeshPro text;
    [SerializeField] Transform bg;
    [SerializeField] GameObject blocker;
    [SerializeField] bool isUnlocked = false;

    private float BG_SCALE_SMALL = 1f;
    private float BG_SCALE_NORMAL = 2f;
    private string SMALL_TXT_TEMPLATE = "<sprite name=currency_star> {0}";
    private string NORMAL_TXT_TEMPLATE = "@Loc=ui_progress_block_desc@@";

    public void InitBlocker()
    {
        //should be called on selector init
        if(records.spentTokens >= requiredStars)
        {
            SetToUnlock();
        }
        else
        {
            SetToLock();
        }
    }
    public void UpdateBlocker_Anim()
    {
        if(records.spentTokens >= requiredStars && !isUnlocked)
        {
            //Anim to unlock
            text.SetText("");
            bg.DOScale(BG_SCALE_SMALL, dConstants.UI.StandardizedBtnAnimDuration).OnComplete(()=> SetToUnlock());
        }
    }
    void SetToUnlock()
    {
        //unlock
        isUnlocked = true;
        bg.localScale = Vector3.one * BG_SCALE_SMALL;
        text.SetText(string.Format(LocalizedAssetLookup.singleton.Translate(SMALL_TXT_TEMPLATE), requiredStars));
        blocker.SetActive(false);
    }
    void SetToLock()
    {
        //lock
        isUnlocked = false;
        bg.localScale = Vector3.one * BG_SCALE_NORMAL;
        text.SetText(string.Format(LocalizedAssetLookup.singleton.Translate(NORMAL_TXT_TEMPLATE), requiredStars, records.spentTokens));
        blocker.SetActive(true);
    }
}
