using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
//using Unity.VisualScripting;
using UnityEngine;
/*
[System.Serializable]
public class NarrativeLineGroup
{
    public TextMeshPro line;
    public GameObject mask;
}
*/
public class NarrativeMaster : MonoBehaviour
{
    static float TypeInterval = 0.2f;
    public RectTransform group;
    public TextMeshPro title;
    public List<TextMeshPro> lines;

    TextMeshPro targetLine;
    string QueuedText = "";
    string FinalText = "";
    int curCharacters = 0;
    bool typing;

    //List<string> QueuedWords;
    Sequence seq;
    public void ClearAllLines()
    {
        seq.Kill();
        typing = false;
        targetLine = null;
        for (int i = 0; i < lines.Count; i++)
        {
            lines[i].SetText("");
        }
    }
    public void TypeALine(string _text)
    {
        if (typing)
        {
            seq.Kill();
            targetLine.SetText(FinalText);
        }
        UpdateTypingSpeed();
        _text = LocalizedAssetLookup.singleton.Translate(_text);
        targetLine = null;
        for(int i=0;i< lines.Count; i++)
        {
            if (lines[i].text.Length == 0)
            {
                targetLine = lines[i];
                break;
            }
        }
        if (targetLine)
        {
            FinalText = _text;
            QueuedText = GetQueuedTexts(_text);
            curCharacters = 0;
            typing = true;
            //TextMeshPro _targetLine = targetLine;
            TryTypeCharacter(targetLine);
        }
        else
        {
            Debug.LogError("narrative lines are full, no line can be used to type new texts.");
        }
    }
    void TryTypeCharacter(TextMeshPro _targetLine)
    {
        if(curCharacters < QueuedText.Length)
        {
            _targetLine.SetText(QueuedText.Substring(0, ++curCharacters));
            seq.Kill();
            seq = DOTween.Sequence();
            seq.AppendInterval(TypeInterval)
                .AppendCallback(() => TryTypeCharacter(_targetLine));
        }
        else
        {
            _targetLine.SetText(FinalText);
            typing = false;
        }
    }
    string GetQueuedTexts(string richtextSentence)
    {
        //List<string> splitWords = new List<string>();
        return Regex.Replace(richtextSentence, "<.*?>", string.Empty);
    }
    void UpdateTypingSpeed()
    {
        if(LocalizedAssetLookup.singleton.curLanguage == LanguageOption.EN)
        {
            TypeInterval = dConstants.VFX.ENTypingInterval;
        }
        else if(LocalizedAssetLookup.singleton.curLanguage == LanguageOption.CN)
        {
            TypeInterval = dConstants.VFX.CNTypingInterval;
        }
    }
}
