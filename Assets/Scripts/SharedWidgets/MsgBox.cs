using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MsgBox : MonoBehaviour
{
    
    [SerializeField] TextMeshPro title_tmp;
    [SerializeField] TextMeshPro desc_tmp;
    [SerializeField] SpriteRenderer photo;
    [SerializeField] TextMeshPro prompt_tmp;
    private void Awake()
    {
        gameObject.SetActive(false);
    }
    public void ShowBox(string title, string desc, string prompt, Sprite photoSprt)
    {
        title_tmp.SetText(LocalizedAssetLookup.singleton.Translate(title));
        desc_tmp.SetText(LocalizedAssetLookup.singleton.Translate(desc));
        prompt_tmp.SetText(LocalizedAssetLookup.singleton.Translate(prompt));

        photo.sprite = photoSprt;

        transform.localScale = Vector3.one;
        transform.DOScale(0f, dConstants.UI.StandardizedBtnAnimDuration).From();
        gameObject.SetActive(true);
    }
    public void CloseBox()
    {
        transform.DOScale(0f, dConstants.UI.StandardizedBtnAnimDuration).OnComplete(()=>gameObject.SetActive(false));
    }
}
