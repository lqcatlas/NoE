using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LM_001_Clock : LevelMasterBase
{
    [Header("Theme Additions")]
    public LMHub_001_Clock clockHub;
    
    public override void GetObjectReferences(GameObject _themeHub)
    {
        base.GetObjectReferences(null);
        clockHub = _themeHub.GetComponent<LMHub_001_Clock>();
    }
    public override void InitCells()
    {
        clockHub.runningClocks = new List<KeyValuePair<CellMaster, GameObject>>();
        clockHub.cellBgHolder.transform.localScale = hub.boardMaster.cellHolder.localScale;
        //clear old running clock bgs
        List<Transform> oldBgs = clockHub.cellBgHolder.GetComponentsInChildren<Transform>(true).ToList();
        oldBgs.Remove(clockHub.cellBgHolder.transform);
        for (int i = 0; i < oldBgs.Count; i++)
        {
            Destroy(oldBgs[i].gameObject);
        }
        //generate new bgs
        for (int i = 0; i < hub.boardMaster.cells.Count; i++)
        {
            DataCell temp_cellData = levelData.curBoard.GetCellDataByCoord(hub.boardMaster.cells[i].coord);
            if (temp_cellData != null)
            {
                hub.boardMaster.cells[i].DisplayNumber(temp_cellData.value);
                //additional
                GameObject clockBg = Instantiate(clockHub.runningClockTemplate, clockHub.cellBgHolder);
                clockBg.transform.position = hub.boardMaster.cells[i].transform.position;
                //clockBg.transform.localScale = hub.boardMaster.cells[i].transform.localScale;
                clockHub.runningClocks.Add(new KeyValuePair<CellMaster, GameObject>(hub.boardMaster.cells[i], clockBg));
                clockBg.gameObject.SetActive(temp_cellData.status == 1);
            }
        }
    }
    
    public override void AddtionalInit_Theme(bool isRewind = false)
    {
        clockHub.clockTool.transform.position = hub.toolMaster.toolIcon.transform.position;
        hub.toolMaster.toolIcon.gameObject.SetActive(false);
        UpdateToolStatusDisplay();
    }
    public override void HandlePlayerInput(Vector2Int coord)
    {
        AudioCentralCtrl.singleton.PlaySFX(clockHub.tickClips.GetClip());
        //cell is played get +1
        if (levelData.levelIndex >= 1 && levelData.levelIndex <= 2)
        {
            for (int i = 0; i < levelData.curBoard.cells.Count; i++)
            {
                if (levelData.curBoard.cells[i].coord == coord)
                {
                    levelData.curBoard.cells[i].value += 1;
                    levelData.curBoard.cells[i].value = BoardCalculation.ModX_Range(levelData.curBoard.cells[i].value, new Vector2Int(1, 12));
                }
            }
        }
        //cell is played get +1
        //cell was played all get +1 as well
        else if (levelData.levelIndex >= 3)
        {
            for (int i = 0; i < levelData.curBoard.cells.Count; i++)
            {
                if (levelData.curBoard.cells[i].status == 1)
                {
                    levelData.curBoard.cells[i].value += 1;
                    levelData.curBoard.cells[i].value = BoardCalculation.ModX_Range(levelData.curBoard.cells[i].value, new Vector2Int(1, 12));
                }
            }
            for (int i = 0; i < levelData.curBoard.cells.Count; i++)
            {
                if (levelData.curBoard.cells[i].coord == coord)
                {
                    levelData.curBoard.cells[i].value += 1;
                    levelData.curBoard.cells[i].value = BoardCalculation.ModX_Range(levelData.curBoard.cells[i].value, new Vector2Int(1, 12));
                    levelData.curBoard.cells[i].status = 1;
                }
            }  
        }
        else
        {
            Debug.LogError(string.Format("master script of {0} reaches undefined level", levelData.theme));
        }
    }
    //private bool narrative_lv7_1 = false;
    //private bool narrative_lv8_1 = false;
    public override void UpdateCells(Vector2Int coord)
    {
        //only instatntly update the play cell by one
        for (int i = 0; i < hub.boardMaster.cells.Count; i++)
        {
            DataCell temp_cellDataPrev = levelData.previousBoard.GetCellDataByCoord(hub.boardMaster.cells[i].coord);
            if (temp_cellDataPrev.coord == coord)
            {
                int showNumber = BoardCalculation.ModX_Range(temp_cellDataPrev.value + 1, new Vector2Int(1, 12));
                hub.boardMaster.cells[i].NumberShift(showNumber);
            }
        }
    }
    public override void AddtionalUpdate_Theme(Vector2Int coord)
    {
        
        //cell bg update addition
        for (int i = 0; i < clockHub.runningClocks.Count; i++)
        {
            DataCell temp_cellData = levelData.curBoard.GetCellDataByCoord(clockHub.runningClocks[i].Key.coord);
            clockHub.runningClocks[i].Value.SetActive(temp_cellData.status == 1);
        }
        clockHub.runningClockBg.DOFade(0.2f, 0.05f).SetEase(Ease.OutCubic);
        clockHub.runningClockBg.DOFade(0f, 1.15f).SetDelay(0.05f).SetEase(Ease.InCubic);
        
        /*
         * //narrative update addition
        if (!narrative_lv7_1 && levelData.levelIndex == 7 && levelData.curBoard.toolCount <= 8)
        {
            narrative_lv7_1 = true;
            TryTypeNextPlayLine(0);
        }
        if (!narrative_lv8_1 && levelData.levelIndex == 8 && BoardCalculation.CountX_All(levelData.curBoard, 5))
        {
            narrative_lv8_1 = true;
            TryTypeNextPlayLine(0);
        }
        */
    }
    public override void DelayedPlay_Theme()
    {
        //delayed update on all cells for additional +1
        for (int i = 0; i < hub.boardMaster.cells.Count; i++)
        {
            DataCell temp_cellData = levelData.curBoard.GetCellDataByCoord(hub.boardMaster.cells[i].coord);
            if (temp_cellData.value != hub.boardMaster.cells[i].curNumber)
            {
                hub.boardMaster.cells[i].NumberShift(temp_cellData.value);
            }
        }

    }
    public override bool CheckWinCondition()
    {
        bool result = false;
        if (levelData.levelIndex == 1)
        {
            return BoardCalculation.CountX_All(levelData.curBoard, 8);
        }
        else if (levelData.levelIndex == 2)
        {
            return BoardCalculation.CountX_All(levelData.curBoard, 10);
        }
        else if (levelData.levelIndex == 3)
        {
            return BoardCalculation.CountX_All(levelData.curBoard, 12);
        }
        else if (levelData.levelIndex == 4)
        {
            return BoardCalculation.Unique_All(levelData.curBoard);
        }
        else if (levelData.levelIndex == 5)
        {
            return BoardCalculation.CountX_All(levelData.curBoard, 2);
        }
        else if (levelData.levelIndex == 6)
        {
            return BoardCalculation.CountX_All(levelData.curBoard, 3);
        }
        else if (levelData.levelIndex == 7)
        {
            return BoardCalculation.CountX_All(levelData.curBoard, 4);
        }
        else if (levelData.levelIndex == 8)
        {
            return BoardCalculation.Unique_All(levelData.curBoard);
        }
        else if (levelData.levelIndex == 9)
        {
            return BoardCalculation.CountX_All(levelData.curBoard, 5);
        }
        else if (levelData.levelIndex == 10)
        {
            return BoardCalculation.CountX_All(levelData.curBoard, 6);
        }
        else
        {
            Debug.LogError(string.Format("reach undefined level in CheckWinCondition of ({0})", levelData.theme));
        }
        return result;
    }
    public override bool CheckLoseCondition()
    {
        if (base.CheckLoseCondition())
        {
            return true;
        }
        bool result = false;
        return result;
    }

    void UpdateToolStatusDisplay()
    {
        ToolStatusGroup targetDisplayTemplate = clockHub.toolStatusGroup;
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
