using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class FloatingBox : MonoBehaviour
{
    static public FloatingBox singleton;
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
    [SerializeField] TextMeshPro desc_tmp;
    public void ShowBox(string desc, Vector3 startPos, Vector3 endPos)
    {
        desc_tmp.SetText(LocalizedAssetLookup.singleton.Translate(desc));
        transform.position = startPos;  
        transform.localScale = Vector3.one;
        transform.DOScale(0f, dConstants.UI.StandardizedBtnAnimDuration).From();
        transform.DOMove(endPos, dConstants.UI.StandardizedBtnAnimDuration);
        page.SetActive(true);
    }
    public void CloseBox()
    {
        transform.DOScale(0f, dConstants.UI.StandardizedBtnAnimDuration).OnComplete(() => page.SetActive(false));
    }
}
