using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ThemePhotoTag : MonoBehaviour
{
    public SpriteRenderer sr;
    public List<Sprite> tagAssets;

    static Vector3 boxOffset = new Vector3(0f, -4.3f, 0f);

    private bool selected;
    private int tagIndex = -1;
    public void SetTag(int index = -1)
    {
        if(index == -1 || index >= tagAssets.Count)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
            tagIndex = index;
            sr.sprite = tagAssets[tagIndex];
            TagOffSelection();
        }
    }

    public void TagEnterSelection()
    {
        sr.DOColor(dConstants.UI.DefaultColor_1st, dConstants.UI.StandardizedBtnAnimDuration);
        selected = true;
    }
    public void TagOffSelection()
    {
        sr.DOColor(dConstants.UI.DefaultColor_2nd, dConstants.UI.StandardizedBtnAnimDuration);
        if (selected)
        {
            FloatingBox.singleton.CloseBox();
        }
        selected = false;
        
    }
    public void TagClick()
    {
        if (selected)
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 pos = Camera.main.ScreenToWorldPoint(mousePos);
            pos.z = 0f;

            string tagTxt = $"@Loc=ui_theme_tag_{tagIndex+1}_desc@@";
            
            FloatingBox.singleton.ShowBox(tagTxt, pos, pos + boxOffset);
            //Debug.LogWarning($"call tag click() with position on {pos}");
        }
    }
}
