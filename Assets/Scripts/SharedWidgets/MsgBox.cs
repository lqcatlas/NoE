using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MsgBox : MonoBehaviour
{
    static public MsgBox singleton;
    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        }
        else
        {
            Destroy(this);
        }
        page.SetActive(false);
    }
    [SerializeField] GameObject page;
    [SerializeField] TextMeshPro title_tmp;
    [SerializeField] TextMeshPro desc_tmp;
    [SerializeField] SpriteRenderer photo;
    [SerializeField] TextMeshPro note_tmp;
    public void ShowBox(string title, string desc, string prompt, Sprite photoSprt)
    {
        title_tmp.SetText(LocalizedAssetLookup.singleton.Translate(title));
        desc_tmp.SetText(LocalizedAssetLookup.singleton.Translate(desc));
        note_tmp.SetText(LocalizedAssetLookup.singleton.Translate(prompt));

        photo.gameObject.SetActive(photoSprt != null);
        photo.sprite = photoSprt;

        transform.localScale = Vector3.one;
        transform.DOScale(0f, dConstants.UI.StandardizedBtnAnimDuration).From();
        page.SetActive(true);
    }
    public void CloseBox()
    {
        transform.DOScale(0f, dConstants.UI.StandardizedBtnAnimDuration).OnComplete(()=> page.SetActive(false));
    }
}
