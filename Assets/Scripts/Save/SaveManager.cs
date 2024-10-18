using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Steamworks;

[System.Serializable]
public class SaveData
{
    //public int GetCount { get { return dict.Count; } }

    public string save_str;

    public string ConvertToString(Dictionary<string, string> _dict)
    {
        string save = "";
        List<string> _list = new List<string>();
        foreach(KeyValuePair<string, string> item in _dict)
        {
            _list.Add(item.Key + "□" + item.Value);
        }
        save = string.Join("■", _list);
        //Debug.Log("save_str: " + save);
        return save;
    }
    public Dictionary<string, string> ConvertToDictionary(string _str)
    {
        Dictionary<string, string> _dict = new Dictionary<string, string>();
        List<string> _list = new List<string>();
        _list = _str.Split('■').ToList();
        //Debug.Log("list item count: " + _list.Count);
        foreach (string item in _list)
        {
            //Debug.Log("item 2 split: " + item);
            if(item.Length > 0)
            {
                string[] KeyValue = item.Split('□');
                _dict.Add(KeyValue[0], KeyValue[1]);
            }
            
        }
        //Debug.Log("dict item count: " + _dict.Count);
        return _dict;
    }

}

public class SaveManager : MonoBehaviour
{
    public static SaveManager controller;
    void Awake()
    {
        if (controller == null)
        {
            controller = this;
        }
        else if (controller != null)
        {
            Destroy(gameObject);
        }
        //DontDestroyOnLoad(controller);
        //ResetCurSave();
        ClearAllSaves = false;
        SavesCleared = false;
        SaveDataModules = FindAllISaveDataModules();
        LoadFromFile();
    }

    [Header("Debug")]
    public bool ClearAllSaves;
    private bool SavesCleared;

    [Header("Savings")]
    public SaveData curSave;

    
    //public List<ScriptableObject> ScriptablesWithSave;
    private List<ISaveData> SaveDataModules;
    private Dictionary<string, string> dict2save = new Dictionary<string, string>();
    public List<string> dictVisualized = new List<string>();
    private void Start()
    {

        /*SaveDataModules = new List<ISaveData>();
        for (int i=0;i< ScriptablesWithSave.Count; i++)
        {
            SaveDataModules.Add(ScriptablesWithSave[i].GetComponent<ISaveData>());
        }*/
        //Debug.Log(string.Format("Find in total of {0} modules has ISaveData interface", SaveDataModules.Count));
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            ClearAllSaves = true;
        }
        if (ClearAllSaves && !SavesCleared)
        {
            //ClearAllSaves = false;
            SavesCleared = true;
            ClearSaveFile();
        }
    }
    private void OnApplicationQuit()
    {
        if (!ClearAllSaves)
        {
            PrepareSaveFromAllModules();
            SaveToFile();
        }
        else
        {
            SaveToFile();
        }
        
    }
    public void ResetCurSave()
    {
        curSave.save_str = "";
        dict2save.Clear();
        VisualizeSaves();
    }
    public string Inquire(string uid)
    {
        if (dict2save.ContainsKey(uid))
        {
            return dict2save[uid];
        }
        else
        {
            //Debug.Log("an invalid save string inquired");
            return null;
        }
    }
    public void Insert(string uid, string data)
    {
        if (dict2save.ContainsKey(uid))
        {
            dict2save[uid] = data;
        }
        else
        {
            dict2save.Add(uid, data);
            //VisualizeSaves();
        }
    }
    void VisualizeSaves()
    {
        dictVisualized.Clear();
        foreach (KeyValuePair<string, string> item in dict2save)
        {
            dictVisualized.Add(item.Key + ": " + item.Value);
        }
        //Debug.Log(string.Format("{0} save items visualized", dictVisualized.Count));
    }
    public void LoadFromFile()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            string finalPath = GetSaveFilePath();
            Debug.LogWarning($"reading files from {finalPath}");
            if (!File.Exists(finalPath))
            {
                ClearSaveFile();
            }
            FileStream file = File.Open(Path.Combine(Application.persistentDataPath, finalPath), FileMode.Open);
            //Debug.Log(Application.persistentDataPath);
            curSave.save_str = (string)bf.Deserialize(file);
            dict2save = curSave.ConvertToDictionary(curSave.save_str);
            file.Close();
        }
        catch (IOException ex)
        {
            Debug.LogError(ex.Message);
            Debug.LogError("game load fails, using fresh start");
        }
        VisualizeSaves();
        //StartCoroutine("SaveLoadedWrapper");
        for(int i = 0; i < SaveDataModules.Count; i++)
        {
            SaveDataModules[i].LoadFromSaveManager();
        }
    }
    public void PrepareSaveFromAllModules()
    {
        for (int i = 0; i < SaveDataModules.Count; i++)
        {
            SaveDataModules[i].SaveToSaveManager();
        }
    }
    public string GetSaveFolder()
    {
        string steamID = SteamUser.GetSteamID().ToString();
        string finalFolder = Path.Combine(Application.persistentDataPath, "cloudsave", steamID);
        return finalFolder;
    }
    public string GetSaveFilePath()
    {
        string save_filename = "gamesave.sav";
#if UNITY_EDITOR
        save_filename = "editor_gamesave.sav";
#endif
        string finalPath = Path.Combine(GetSaveFolder(), save_filename);
        return finalPath;
    }
    public void SaveToFile()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            string finalFolder = GetSaveFolder();
            string finalPath = GetSaveFilePath();
            Debug.LogWarning($"saving files to {finalPath}");
            Directory.CreateDirectory(finalFolder);
            FileStream file = File.Create(finalPath);
            curSave.save_str = curSave.ConvertToString(dict2save);
            bf.Serialize(file, curSave.save_str);
            file.Close();
        }
        catch (IOException ex)
        {
            Debug.LogError(ex.Message);
            Debug.LogError("game save fails, no progression saved");
        }
        //Debug.Log(string.Format("game saved to file with {0} items in it", dict2save.Count));
        VisualizeSaves();
    }
    public void ClearSaveFile()
    {
        ResetCurSave();
        SaveToFile();
    }
    List<ISaveData> FindAllISaveDataModules()
    {
        IEnumerable<ISaveData> modules = FindObjectsOfType<MonoBehaviour>().OfType<ISaveData>();
        return new List<ISaveData>(modules);
    }
    /*IEnumerator SaveLoadedWrapper()
    {
        yield return new WaitForSeconds(0.1f);
        loadingData.flavorText = "Save Loaded";
        loadingData.progression = 0.9f;
        GameLoading.Raise();
        yield return new WaitForSeconds(0.5f);
        loadingData.flavorText = "Entering Selection";
        loadingData.progression = 1f;
        GameLoading.Raise();

        SaveLoaded.Raise();
        SaveLoadedFinished.Raise();
    }
    */
}
