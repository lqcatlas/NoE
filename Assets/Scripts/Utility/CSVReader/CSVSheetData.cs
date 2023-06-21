using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class CSVSheetData
{
    public string sheetName;
    public List<Dictionary<string, object>> sheet;

    public CSVSheetData()
    {
        sheetName = "unamed sheet";
        sheet = new List<Dictionary<string, object>>();
    }

    public Dictionary<string, object> GetLineByUID(int uid)
    {
        for (int i = 0; i < sheet.Count; i++)
        {
            if ((string)sheet[i]["UID"] == uid.ToString())
            {
                return sheet[i];
            }
        }
        Debug.LogError(string.Format("GetLine() searching for a invalid UID({0}) in sheet({1})", uid, sheetName));
        return null;
    }
    public object GetCell(string searchCol, object searchKey, string returnCol)
    {
        if(sheet.Count == 0)
        {
            Debug.LogError(string.Format("empty sheet {0}", sheetName));
            return null;
        }
        //check if Search/Target Column existed
        if(!sheet[0].ContainsKey(searchCol))
        {
            Debug.LogError(string.Format("GetCell() searching for a invalid column({0}) in sheet({1})", searchCol, sheetName));
            return null;
        }
        if (!sheet[0].ContainsKey(returnCol))
        {
            Debug.LogError(string.Format("GetCell() searching for a invalid column({0}) in sheet({1})", returnCol, sheetName));
            return null;
        }
        //find Search Line Index
        for (int i = 0; i < sheet.Count; i++)
        {
            if (sheet[i][searchCol] == searchKey)
            {
                return sheet[i][returnCol];
            }
        }
        Debug.LogError(string.Format("GetCell() searching for a invalid line({0}) in sheet({1})", (string)searchKey, sheetName));
        return null;
    }
}
