using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LM_008_Crown : LevelMasterBase
{
    enum CrownStatus : int { off = 0, on = 1 };
    [Header("Theme Additions")]
    public LMHub_008_Crown crownHub;

    [Header("Crown Play Log")]
    public List<CrownLog> crownLogs;
    public class CrownLog
    {
        public bool success;
        public int takenCount;
    }

    //private int Count_SwictchToOn;

    public override void GetObjectReferences(GameObject _themeHub)
    {
        base.GetObjectReferences(null);
        crownHub = _themeHub.GetComponent<LMHub_008_Crown>();
    }

    public override void AddtionalInit_Theme(bool isRewind = false)
    {
        if (!isRewind)
        {
            //fullPhaseCount = 0;
            crownLogs = new List<CrownLog>();
        }
        else
        {
            crownLogs.RemoveAt(crownLogs.Count - 1);
        }
    }
    public override void HandlePlayerInput(Vector2Int coord)
    {
        Vector2Int numberCap = new Vector2Int(0, 9);
        //remove cell
        for (int i = 0; i < levelData.curBoard.cells.Count; i++)
        {
            if (levelData.curBoard.cells[i].coord == coord)
            {
                levelData.curBoard.cells[i].value += 3;
                levelData.curBoard.cells[i].value = Mathf.Min(levelData.curBoard.cells[i].value, numberCap.y);
                levelData.curBoard.cells[i].status = (int)CrownStatus.on;
            }
        }
        CrownLog currentLog = new CrownLog();
        currentLog.success = true;
        currentLog.takenCount = 0;
        crownLogs.Add(currentLog);
    }

    public override void HandleEnvironment(Vector2Int coord)
    {
        //successThisRound = true;
        Vector2Int numberCap = new Vector2Int(0, 9);
        //double pass on cells to check if crowning is a success
        for (int i = 0; i < levelData.curBoard.cells.Count; i++)
        {
            if (levelData.curBoard.cells[i].coord == coord)
            {
                for (int j = 0; j < levelData.curBoard.cells.Count; j++)
                {
                    if (BoardCalculation.Manhattan_Dist(levelData.curBoard.cells[i].coord, levelData.curBoard.cells[j].coord) == 1)
                    {
                        if (levelData.curBoard.cells[i].value < levelData.curBoard.cells[j].value)
                        {
                            levelData.curBoard.cells[i].value = 0;
                            levelData.curBoard.cells[i].status = (int)CrownStatus.off;
                            crownLogs[crownLogs.Count - 1].success = false;
                        }
                        else if (levelData.curBoard.cells[i].value > levelData.curBoard.cells[j].value)
                        {
                            levelData.curBoard.cells[j].value -= 2;
                            levelData.curBoard.cells[j].value = Mathf.Max(levelData.curBoard.cells[i].value, numberCap.x);
                            levelData.curBoard.cells[j].status = (int)CrownStatus.off;
                            crownLogs[crownLogs.Count - 1].takenCount += 1;
                        }
                    }
                }
            }
        }
    }
   
    public override void UpdateCells(Vector2Int coord)
    {
        base.UpdateCells(coord);
        //play crown VFX based on success or not
        if (GetCurrentCrownSuccess())
        {

        }
        else
        {

        }
    }
    public override void AddtionalUpdate_Theme(Vector2Int coord)
    {

    }
    public override bool CheckWinCondition()
    {
        bool result = false;
        if (levelData.levelIndex == 1)
        {
            return BoardCalculation.CountX_Ytimes(levelData.curBoard, 9, 2);
        }
        else if (levelData.levelIndex == 2)
        {
            return BoardCalculation.CountX_Ytimes(levelData.curBoard, 9, 2);
        }
        else if (levelData.levelIndex == 3)
        {
            return BoardCalculation.CountX_Ytimes(levelData.curBoard, 0, 5);
        }
        else if (levelData.levelIndex == 4)
        {
            return BoardCalculation.CountX_Ytimes(levelData.curBoard, 9, 6);
        }
        else if (levelData.levelIndex == 5)
        {
            return BoardCalculation.CountX_Ytimes(levelData.curBoard, 9, 6);
        }
        else if (levelData.levelIndex == 6)
        {
            return BoardCalculation.CountStatusX_Ytimes(levelData.curBoard, 1, 5);
        }
        else if (levelData.levelIndex == 7)
        {
            return GetTotalCrownTaken() >= 8;
        }
        else if (levelData.levelIndex == 8)
        {
            return BoardCalculation.CountXplus_Ytimes(levelData.curBoard, 7, 9);
        }
        else if (levelData.levelIndex == 9)
        {

        }
        else
        {
            Debug.LogError(string.Format("reach undefined level in CheckWinCondition of ({0})", levelData.theme));
        }
        return result;
    }

    bool GetCurrentCrownSuccess()
    {
        return crownLogs[crownLogs.Count - 1].success;
    }
    int GetCurrentCrownTaken()
    {
        return crownLogs[crownLogs.Count - 1].takenCount;
    }
    int GetTotalSuccess()
    {
        int total = 0;
        for(int i = 0; i < crownLogs.Count; i++)
        {
            if (crownLogs[i].success)
            {
                total += 1;
            }
        }
        return total;
    }
    int GetTotalCrownTaken()
    {
        int total = 0;
        for (int i = 0; i < crownLogs.Count; i++)
        {
            total += crownLogs[i].takenCount;
        }
        return total;
    }
}
