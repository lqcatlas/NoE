using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
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
    public TextMeshPro title;
    public List<TextMeshPro> lines;
    public void ClearAllLines()
    {
        for(int i = 0; i < lines.Count; i++)
        {
            lines[i].SetText("");
        }
    }
    public void TypeALine(string _text)
    {
        _text = LocalizedAssetLookup.singleton.Translate(_text);
        TextMeshPro targetLine = null;
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
            QueuedText = _text;
            curCharacters = 0;
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
            Sequence seq = DOTween.Sequence();
            seq.AppendInterval(TypeInterval)
                .AppendCallback(() => TryTypeCharacter(_targetLine));
        }
    }
    string QueuedText = "";
    int curCharacters = 0;

}
