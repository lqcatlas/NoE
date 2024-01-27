using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class RuleLine : MonoBehaviour
{
    [SerializeField] List<Sprite> iconOptions;

    public GameObject tagGroup;
    public SpriteRenderer icon;
    public TextMeshPro notificationTag;
    public TextMeshPro text;
    public SpriteRenderer underline;
    public Transform mask;

    private Sequence seq;
    public void SetRuleLine(RuleItem rule)
    {
        tagGroup.gameObject.SetActive(false);
        notificationTag.SetText(LocalizedAssetLookup.singleton.Translate("@Loc=ui_new_tag@@"));
        text.SetText(LocalizedAssetLookup.singleton.Translate(rule.ruleDesc));
        mask.gameObject.SetActive(false);
        underline.gameObject.SetActive(false);
    }
    public void SetRuleLine(string txt)
    {
        tagGroup.gameObject.SetActive(false);
        text.SetText(LocalizedAssetLookup.singleton.Translate(txt));
        //text.SetText(LocalizedAssetLookup.singleton.TranslateKey(txt));
        //icon.sprite = iconOptions[0];
        //icon.gameObject.SetActive(false);
        underline.gameObject.SetActive(false);
        mask.gameObject.SetActive(false);
    }
    public void InitLineAnimation()
    {
        mask.gameObject.SetActive(true);
        mask.localPosition = new Vector3(2.5f, 0f, 0f);
        mask.localScale = new Vector3(60f, 5.4f, 1f);
        tagGroup.SetActive(false);
        //icon.gameObject.SetActive(false);
    }
    public void AnimateLine(float delay = 0f)
    {
        float shakeDuration = 0.7f;
        float shakeDelayAddition = 0.3f;
        tagGroup.transform.localScale = Vector3.zero;
        seq.Pause();
        seq.Kill();
        //DOTween.Kill(mask);
        //DOTween.Kill(tagGroup.transform);
        seq = DOTween.Sequence();
        seq.AppendInterval(delay);
        //icon.gameObject.SetActive(true);
        //icon.transform.DOScale(0f, shakeDelayAddition).From().SetDelay(delay);
        seq.Append(mask.DOLocalMoveX(30f, shakeDuration).OnComplete(() => mask.localScale = Vector3.zero));
        seq.Insert(delay + shakeDuration / 2f, tagGroup.transform.DOScale(1f, shakeDelayAddition).OnStart(()=>tagGroup.SetActive(true)));
        //text.transform.DOShakePosition(shakeDuration, 0.2f, 200, 90, false, true, ShakeRandomnessMode.Full).SetDelay(delay + shakeDelayAddition);
        //text.transform.DOShakeRotation(shakeDuration, 5f, 200, 90, true, ShakeRandomnessMode.Full).SetDelay(delay + shakeDelayAddition);
        //text.transform.DOShakeScale(shakeDuration, 0.05f, 200, 90, true, ShakeRandomnessMode.Full).SetDelay(delay + shakeDelayAddition);
        

    }
}
