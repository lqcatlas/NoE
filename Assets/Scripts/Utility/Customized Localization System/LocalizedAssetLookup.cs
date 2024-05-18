using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public enum LanguageOption:int{ EN = 0, CN = 1};
public class LocalizedAssetLookup : MonoBehaviour
{
    static public LocalizedAssetLookup singleton;
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
    [SerializeField] LocalizationSource source;
    public LanguageOption curLanguage;
    [SerializeField] LanguageOption defaultLanguage;
    public void SwitchLanguage(LanguageOption option)
    {
        if(option != curLanguage)
        {
            curLanguage = option;
        }
    }
    public string Translate(string txt)
    {
        if(txt == "")
        {
            return txt;
        }
        string TRIM_CHARS_START = "@Loc=";
        string TRIM_CHARS_END = "@@";
        //Debug.Log(string.Format("input txt is {0}", txt));
        if (txt.Contains(TRIM_CHARS_START))
        {
            //trim the localization key out
            string key = txt.Substring(txt.IndexOf(TRIM_CHARS_START), txt.IndexOf(TRIM_CHARS_END) - txt.IndexOf(TRIM_CHARS_START)).TrimStart(TRIM_CHARS_START).TrimEnd(TRIM_CHARS_END);
            string leadingTxt = txt.Substring(0, txt.IndexOf(TRIM_CHARS_START));
            string trailingTxt = txt.Substring(txt.IndexOf(TRIM_CHARS_END), txt.Length - txt.IndexOf(TRIM_CHARS_END)).TrimStart(TRIM_CHARS_END);
            //Debug.Log(string.Format("translator looking for {0}", key));
            //get cell in CSV source
            string result = source.GetLocalizedString(key, curLanguage);
            /*switch (curLanguage)
            {
                case LanguageOption.EN:
                    //Debug.Log("current lan EN");
                    result = (string)source.dataSheet.GetCell("Key", key, "EN");
                    break;
                case LanguageOption.CN:
                    result = (string)source.dataSheet.GetCell("Key", key, "CN");
                    break;
                default:
                    break;
            }*/
            //return result or handle failure (test)
            if (result == null)
            {
                if (curLanguage == LanguageOption.EN)
                {
                    result = txt;
                }
                else if (curLanguage == LanguageOption.CN)
                {
                    result = txt;
                }
                else
                {
                    result = "undefined languaage";
                }
            }
            return leadingTxt + result + trailingTxt;
        }
        else
        {
            return txt;
        }
    }

}
