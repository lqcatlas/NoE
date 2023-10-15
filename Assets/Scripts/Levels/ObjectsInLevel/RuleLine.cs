using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RuleLine : MonoBehaviour
{
    [SerializeField] List<Sprite> iconOptions;

    public SpriteRenderer icon;
    public TextMeshPro text;
    public SpriteRenderer underline;

    public void SetRuleLine(RuleItem rule)
    {
        text.SetText(LocalizedAssetLookup.singleton.Translate(rule.ruleDesc));
        //text.SetText(LocalizedAssetLookup.singleton.TranslateKey(rule.ruleDesc));
        icon.sprite = iconOptions[(int)rule.tag];
        icon.gameObject.SetActive(false);
        if (rule.tag == RuleItem.RuleItemTag.addition || rule.tag == RuleItem.RuleItemTag.transition)
        {
            underline.gameObject.SetActive(false);
        }
        else
        {
            underline.gameObject.SetActive(false);
        }
    }
    public void SetRuleLine(string txt)
    {
        text.SetText(LocalizedAssetLookup.singleton.Translate(txt));
        //text.SetText(LocalizedAssetLookup.singleton.TranslateKey(txt));
        icon.sprite = iconOptions[0];
        icon.gameObject.SetActive(false);
        underline.gameObject.SetActive(false);
    }
    public void AnimateLine(float delay = 0f)
    {
        icon.gameObject.SetActive(true);
        icon.transform.DOScale(0f, 1f).From().SetDelay(delay);
        float shakeDuration = 0.7f;
        float shakeDelayAddition = 0.3f;
        text.transform.DOShakePosition(shakeDuration, 0.2f, 200, 90, false, true, ShakeRandomnessMode.Full).SetDelay(delay + shakeDelayAddition);
        text.transform.DOShakeRotation(shakeDuration, 5f, 200, 90, true, ShakeRandomnessMode.Full).SetDelay(delay + shakeDelayAddition);
        text.transform.DOShakeScale(shakeDuration, 0.05f, 200, 90, true, ShakeRandomnessMode.Full).SetDelay(delay + shakeDelayAddition);
    }
}
