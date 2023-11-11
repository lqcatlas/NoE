using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayingData/PlayerSettings")]
public class PlayerSettings : ScriptableObject, ISaveData
{
    public float audioVolume = 0.5f;
    private const string AUDIOVOLUME_SAVE_KEY = "setting.volume";
    
    public int introCount;
    private const string INTROCOUNT_SAVE_KEY = "setting.intro";

    public void LoadFromSaveManager()
    {
        string str = SaveManager.controller.Inquire(string.Format(AUDIOVOLUME_SAVE_KEY));
        if (str != null)
        {
            float.TryParse(SaveManager.controller.Inquire(string.Format(AUDIOVOLUME_SAVE_KEY)), out audioVolume);
        }
        str = SaveManager.controller.Inquire(string.Format(INTROCOUNT_SAVE_KEY));
        if (str != null)
        {
            int.TryParse(SaveManager.controller.Inquire(string.Format(INTROCOUNT_SAVE_KEY)), out introCount);
        }
    }
    public void SaveToSaveManager()
    {
        //Debug.Log(string.Format("selector save data to file, tokens:{0}", playerLevelRecords.tokens));
        SaveManager.controller.Insert(string.Format(AUDIOVOLUME_SAVE_KEY), audioVolume.ToString());
        SaveManager.controller.Insert(string.Format(INTROCOUNT_SAVE_KEY), introCount.ToString());
    }

}
