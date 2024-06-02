using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IntroPage : MonoBehaviour
{
    static public IntroPage singleton;
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
    }
    static float MoveAnimDuration = 3f;
    [Header("Children Objs")]
    [SerializeField] GameObject page;
    [SerializeField] IntroPageCell cell1;
    [SerializeField] IntroPageCell cell2;
    [SerializeField] IntroPageCell cell3;
    [SerializeField] IntroPageCaption captionGroup;

    public void StartIntro()
    {
        page.SetActive(true);
        cell1.CellFadeIn();
        cell1.transform.DOMoveX(-25f, MoveAnimDuration).From().SetEase(Ease.OutSine);
        captionGroup.CaptionUpdate("@Loc=ui_intro_line0@@");
    }
    public void Cell1Clicked()
    {
        cell1.CellFadeOut();
        cell1.transform.DOMoveX(-12f, MoveAnimDuration).SetEase(Ease.OutSine);
        captionGroup.CaptionFadeOut();

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(MoveAnimDuration / 2f);
        seq.AppendCallback(() => cell2.CellFadeIn());
        seq.AppendCallback(() => captionGroup.CaptionUpdate("@Loc=ui_intro_line1@@"));
    }
    public void Cell2Clicked()
    {
        cell2.CellFadeOut();
        cell2.transform.DOMoveX(-2f, MoveAnimDuration).SetEase(Ease.OutSine);
        captionGroup.CaptionFadeOut();

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(MoveAnimDuration / 2f);
        seq.AppendCallback(() => cell3.CellFadeIn());
        seq.AppendCallback(() => captionGroup.CaptionUpdate("@Loc=ui_intro_line2@@"));

    }
    public void Cell3Clicked()
    {
        cell3.CellFadeOut();
        //cell3.CellFadeIn();
        captionGroup.CaptionUpdate("@Loc=ui_intro_line3@@");

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(3f);
        seq.AppendCallback(() => captionGroup.CaptionFadeOut());
        seq.AppendInterval(1f);
        seq.AppendCallback(() => TitlePage.singleton.IntroAnimToTitlePage());
    }
}
