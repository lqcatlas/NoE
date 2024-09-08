using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ThemePhotoTag : MonoBehaviour
{
    public SpriteRenderer sr;
    public List<Sprite> tagAssets;

    public void SetTag(int tagIndex = -1)
    {
        if(tagIndex == -1 || tagIndex >= tagAssets.Count)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
            sr.sprite = tagAssets[tagIndex];
            TagOffSelection();
        }
    }

    public void TagEnterSelection()
    {
        sr.DOColor(dConstants.UI.DefaultColor_1st, dConstants.UI.StandardizedBtnAnimDuration);
    }
    public void TagOffSelection()
    {
        sr.DOColor(dConstants.UI.DefaultColor_2nd, dConstants.UI.StandardizedBtnAnimDuration);
    }
    public void TagClick()
    {

    }
}
