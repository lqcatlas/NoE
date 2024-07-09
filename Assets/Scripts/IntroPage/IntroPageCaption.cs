using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class IntroPageCaption : MonoBehaviour
{
    [Header("Static Params")]
    //static float MaskEndPos = 60f;
    //static float MaskAnimDuration = 3f;
    static float FadeInDuration = 1.5f;
    static float FadeOutDuration = 1f;
    [Header("Children Objs")]
    //[SerializeField] RectTransform caption_mask;
    [SerializeField] TextMeshPro caption;

    public void CaptionUpdate(string txt)
    {
        
        caption.SetText(LocalizedAssetLookup.singleton.Translate(txt));
        caption.color = new Color(1f,1f,1f,0f);
        caption.DOFade(1f, FadeInDuration);

        /*
        caption.color = Color.white;
        caption_mask.localPosition = Vector3.zero;
        caption_mask.DOMoveX(MaskEndPos, MaskAnimDuration);
        */
    }
    public void CaptionFadeOut()
    {
        caption.color = Color.white;
        caption.DOFade(0f, FadeOutDuration);
    }
}
