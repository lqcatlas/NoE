using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Load_Localization : MonoBehaviour
{
    [SerializeField] string filename;
    [SerializeField] LocalizationSource source;
#if UNITY_EDITOR
    private void Start()
    {
        CSVSheetData dataSheet = new CSVSheetData();
        dataSheet.sheet = CSVReader.Read("csv/"+filename);
        dataSheet.sheetName = filename;
        source.locList.Clear();
        for (int i=0;i< dataSheet.sheet.Count; i++)
        {
            LocalizedItem item = new LocalizedItem(
                (string)dataSheet.sheet[i]["Key"],
                (string)dataSheet.sheet[i]["EN"],
                (string)dataSheet.sheet[i]["CN"]);
            source.locList.Add(item);
        }
        EditorUtility.SetDirty(source);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log(string.Format("localization data loaded. total of {0} lines", source.locList.Count));
    }
#endif
}
