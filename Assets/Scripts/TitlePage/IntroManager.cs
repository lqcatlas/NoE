using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IntroManager : MonoBehaviour
{
    [SerializeField] TextMeshPro line_tmp;
    [SerializeField] IntroLines lines;
    private void Start()
    {
        line_tmp.color = new Color(dConstants.UI.DefaultColor_1st.r, dConstants.UI.DefaultColor_1st.g, dConstants.UI.DefaultColor_1st.b,0f);
    }
    public void IntroPhaseCheck()
    {

    }
    public void LineEmerge(float duration, string txt)
    {
        line_tmp.SetText(txt);
        line_tmp.DOFade(1f, duration).SetLoops(2, LoopType.Yoyo).OnComplete(()=> TitlePage.singleton.OpenSelector());
    }
}
