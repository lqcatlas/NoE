using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class LocalizedItem
{
    public string key;
    public string EN;
    public string CN;

    public LocalizedItem(string _key, string _EN, string _CN)
    {
        key = _key;
        EN = _EN;
        CN = _CN;
    }
}

[CreateAssetMenu(menuName = "CSVData/LocalizationSheet")]
public class LocalizationSource : ScriptableObject
{
    public List<LocalizedItem> locList;
    public string GetLocalizedString(string key, LanguageOption lanType)
    {
        for(int i = 0; i < locList.Count; i++)
        {
            if (locList[i].key == key)
            {
                switch (lanType)
                {
                    case LanguageOption.EN:
                        return locList[i].EN;
                    case LanguageOption.CN:
                        return locList[i].CN;
                    default:
                        return locList[i].EN;
                }
            }
        }
        return null;
    }
}
