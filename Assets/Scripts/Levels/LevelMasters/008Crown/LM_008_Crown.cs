using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    public override void InitCells()
    {
        //init bg for crowns
        crownHub.crownBgs = new List<KeyValuePair<CellMaster, GameObject>>();
        crownHub.cellBgHolder.transform.localScale = hub.boardMaster.cellHolder.localScale;
        List<Transform> oldCrowns = crownHub.cellBgHolder.GetComponentsInChildren<Transform>().ToList();
        oldCrowns.Remove(crownHub.cellBgHolder.transform);
        for (int i = 0; i < oldCrowns.Count; i++)
        {
            Destroy(oldCrowns[i].gameObject);
        }
        for (int i = 0; i < hub.boardMaster.cells.Count; i++)
        {
            DataCell temp_cellData = levelData.curBoard.GetCellDataByCoord(hub.boardMaster.cells[i].coord);
            if (temp_cellData != null)
            {
                hub.boardMaster.cells[i].DisplayNumber(temp_cellData.value);
                //create crown sprite
                GameObject obj = Instantiate(crownHub.crownTemplate, crownHub.cellBgHolder);
                obj.transform.position = hub.boardMaster.cells[i].transform.position;
                crownHub.crownBgs.Add(new KeyValuePair<CellMaster, GameObject>(hub.boardMaster.cells[i], obj));
                if (temp_cellData.status == (int)CrownStatus.on)
                {
                    obj.gameObject.SetActive(true);
                }
                else
                {
                    obj.gameObject.SetActive(false);
                }
            }
        }
    }
    public override void InitTool()
    {
        base.InitTool();
        hub.toolMaster.toolIcon.sprite = crownHub.toolSprite;
        UpdateToolStatusDisplay();
    }
    public override void AddtionalInit_Theme(bool isRewind = false)
    {
        if (!isRewind)
        {
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
        DataCell crownCell = levelData.curBoard.GetCellDataByCoord(coord);
        //level 3+, check if crowning is a success, level
        if (levelData.levelIndex >= 3)
        {
            for (int i = 0; i < levelData.curBoard.cells.Count; i++)
            {
                if (BoardCalculation.Manhattan_Dist(coord, levelData.curBoard.cells[i].coord) == 1)
                {
                    if (crownCell.value < levelData.curBoard.cells[i].value)
                    {
                        crownCell.value = 0;
                        crownCell.status = (int)CrownStatus.off;
                        crownLogs[crownLogs.Count - 1].success = false;
                    }
                }
            }
        }
        //level 6+, if succeed, taking crowns from neighbors
        if (levelData.levelIndex >= 6)
        {
            if (GetCurrentCrownSuccess())
            {
                for (int i = 0; i < levelData.curBoard.cells.Count; i++)
                {
                    if (BoardCalculation.Manhattan_Dist(coord, levelData.curBoard.cells[i].coord) == 1)
                    {
                        if (crownCell.value > levelData.curBoard.cells[i].value && levelData.curBoard.cells[i].status == (int)CrownStatus.on)
                        {
                            levelData.curBoard.cells[i].value -= 2;
                            levelData.curBoard.cells[i].value = Mathf.Max(levelData.curBoard.cells[i].value, numberCap.x);
                            levelData.curBoard.cells[i].status = (int)CrownStatus.off;
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
        for (int i = 0; i < crownHub.crownBgs.Count; i++)
        {
            DataCell temp_cellData = levelData.curBoard.GetCellDataByCoord(crownHub.crownBgs[i].Key.coord);
            if (temp_cellData.status == (int)CrownStatus.on)
            {
                crownHub.crownBgs[i].Value.SetActive(true);
            }
            else
            {
                crownHub.crownBgs[i].Value.SetActive(false);
            }
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
            return BoardCalculation.CountX_All(levelData.curBoard, 9);
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
    void UpdateToolStatusDisplay()
    {
        ToolStatusGroup targetDisplayTemplate = crownHub.toolStatusGroup;

        string toolName = targetDisplayTemplate.GetStatusName(levelData.curBoard.toolStatus);
        if (toolName != null)
        {
            hub.toolMaster.toolSubtitle.SetText(LocalizedAssetLookup.singleton.Translate(toolName));
        }
        Sprite toolInfograph = targetDisplayTemplate.GetStatusInfograph(levelData.curBoard.toolStatus);
        if (toolInfograph != null)
        {
            hub.toolMaster.infographGroup.SetActive(true);
            hub.toolMaster.infograph.sprite = toolInfograph;
        }
        else
        {
            hub.toolMaster.infographGroup.SetActive(false);
        }
    }
}
