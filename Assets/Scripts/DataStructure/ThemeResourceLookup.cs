using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class ThemeResource
{
    public string Theme;
    public int ThemeID;
    //public MonoScript MasterScript;
    public string ScriptName;
    public GameObject ThemeSpecialHub;
    public GameObject ThemeHiddenObjPage;
    public Sprite ThemeSprite;
}
[CreateAssetMenu(menuName = "Lookup/ThemeResource")]
public class ThemeResourceLookup : ScriptableObject
{
    public List<ThemeResource> ThemeXResource;

    /*public MonoScript GetThemeScript(int _themeID)
    {
        for(int i=0;i< ThemeXResource.Count; i++)
        {
            if (ThemeXResource[i].ThemeID == _themeID)
            {
                return ThemeXResource[i].MasterScript;
            }
        }
        Debug.LogError(string.Format("unbale to find theme ({0})", _themeID));
        return null;
    }
    public MonoScript GetThemeScript(string _themeName)
    {
        for (int i = 0; i < ThemeXResource.Count; i++)
        {
            if (ThemeXResource[i].Theme == _themeName)
            {
                return ThemeXResource[i].MasterScript;
            }
        }
        Debug.LogError(string.Format("unbale to find theme ({0})", _themeName));
        return null;
    }*/
    public string GetThemeScriptName(int _themeID)
    {
        for (int i = 0; i < ThemeXResource.Count; i++)
        {
            if (ThemeXResource[i].ThemeID == _themeID)
            {
                return ThemeXResource[i].ScriptName;
            }
        }
        Debug.LogError(string.Format("unbale to find theme ({0})", _themeID));
        return null;
    }
    public GameObject GetThemeSpecialHub(int _themeID)
    {
        for (int i = 0; i < ThemeXResource.Count; i++)
        {
            if (ThemeXResource[i].ThemeID == _themeID)
            {
                return ThemeXResource[i].ThemeSpecialHub;
            }
        }
        Debug.LogError(string.Format("unbale to find theme ({0})", _themeID));
        return null;
    }
    public GameObject GetThemeSpecialHub(string _themeName)
    {
        for (int i = 0; i < ThemeXResource.Count; i++)
        {
            if (ThemeXResource[i].Theme == _themeName)
            {
                return ThemeXResource[i].ThemeSpecialHub;
            }
        }
        Debug.LogError(string.Format("unbale to find theme ({0})", _themeName));
        return null;
    }
    public GameObject GetThemeHiddenObjectPage(int _themeID)
    {
        for (int i = 0; i < ThemeXResource.Count; i++)
        {
            if (ThemeXResource[i].ThemeID == _themeID)
            {
                return ThemeXResource[i].ThemeHiddenObjPage;
            }
        }
        Debug.LogError(string.Format("unbale to find theme hidden obj page ({0})", _themeID));
        return null;
    }
}
