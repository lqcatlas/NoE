using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatisticCtrl : MonoBehaviour
{
    [SerializeField] LevelRecords playerRecords;

    private void Update()
    {
        /*if (Input.GetKeyUp(KeyCode.S))
        {
            ShowPlayingRecordBox();
        }*/
    }
    public string GenPlayingRecordText()
    {
        string record_txt = "";
        for (int i = 0; i < playerRecords.unlockedThemes.Count; i++)
        {
            int themeIndex = playerRecords.unlockedThemes[i];
            record_txt += string.Format("theme{0}: ", themeIndex.ToString("000"));
            int finishedLvCountsWithin = 0;
            for(int j = 0; j < playerRecords.finishedLevels.Count; j++)
            {
                if (Mathf.FloorToInt(playerRecords.finishedLevels[j] / 100) == themeIndex)
                {
                    finishedLvCountsWithin += 1;
                }
            }
            record_txt += string.Format("{0} levels finished.<br>", finishedLvCountsWithin);
        }
        return record_txt;
    }
    public void ShowPlayingRecordBox()
    {
        string msg = GenPlayingRecordText();
        MsgBox.singleton.ShowBox("Record", msg, "", null);
    }
}
