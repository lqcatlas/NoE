using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LM_005_Moon : LevelMasterBase
{
    enum MoonPhase : int { none = 0, crescent_1 = 1, quarter_1 = 2, full = 3, quarter_2 = 4, crescent_2 = 5 };

    [Header("Theme Additions")]
    public LMHub_005_Moon moonHub;
    public int fullPhaseCount = 0;
    public int endReason = -1;
    public bool eclipseTrigger = false;
    public bool eclipsed = false;
    public int eclipseCount = 0;
    public Vector2Int eclipseCoord;

    public override void GetObjectReferences(GameObject _themeHub)
    {
        base.GetObjectReferences(null);
        moonHub = _themeHub.GetComponent<LMHub_005_Moon>();
        //init theme-specific params
    }

    public override void InitTool()
    {
        base.InitTool();
        hub.toolMaster.frame.gameObject.SetActive(false);
        hub.toolMaster.toolIcon.gameObject.SetActive(false);
        UpdateToolStatusDisplay();
        moonHub.SetPlateWidget(false);
        //moon phase tablet
        moonHub.InitToolToCycle(levelData.curBoard.toolStatus, levelData.levelIndex);
        //moonHub.AnimateCycle();
    }
    public override void AddtionalInit_Theme()
    {
        fullPhaseCount = 0;
        endReason = -1;
        eclipseTrigger = false;
        eclipsed = false;
        eclipseCount = 0;

        if (levelData.levelIndex == 7 || levelData.levelIndex == 9)
        {
            hub.goalMaster.lines[1].SetText(string.Format("{0}{1}", LocalizedAssetLookup.singleton.Translate("@Loc=ui_goal_current_sum@@"), levelData.curBoard.CurrentSum()));
            hub.goalMaster.lines[1].gameObject.SetActive(true);
            LayoutRebuilder.ForceRebuildLayoutImmediate(hub.goalMaster.goalLayout);
        }
    }
    public override void DelayedInit_Theme()
    {
        if(levelData.levelIndex >= 3)
        {
            moonHub.SetPlateWidget(true);
            moonHub.SetTabletToDegree(0);
            moonHub.AnimateTabletToDegree(levelData.curBoard.toolStatus);
        }
    }
    public override void HandlePlayerInput(Vector2Int coord)
    {
        Vector2Int numberRange_lv1 = new Vector2Int(0, 9);
        Vector2Int numberRange = new Vector2Int(0, 10000);
        //cell number rule
        //lv 1
        //crescent +1, quarter +3
        if (levelData.levelIndex == 1)
        {
            for (int i = 0; i < levelData.curBoard.cells.Count; i++)
            {
                if (levelData.curBoard.cells[i].coord == coord)
                {
                    if(levelData.curBoard.toolStatus == (int)MoonPhase.crescent_1 || levelData.curBoard.toolStatus == (int)MoonPhase.crescent_2)
                    {
                        levelData.curBoard.cells[i].value += 1;
                        levelData.curBoard.cells[i].value = BoardCalculation.ModX_Range(levelData.curBoard.cells[i].value, numberRange_lv1);
                        //Play sfx ?
                    }
                    else if (levelData.curBoard.toolStatus == (int)MoonPhase.quarter_1 || levelData.curBoard.toolStatus == (int)MoonPhase.quarter_2)
                    {
                        levelData.curBoard.cells[i].value += 3;
                        levelData.curBoard.cells[i].value = BoardCalculation.ModX_Range(levelData.curBoard.cells[i].value, numberRange_lv1);
                    }
                }

            }
        }     
        //lv 2+
        //crescent +1, quarter +3, full +5
        else if (levelData.levelIndex >= 2 && levelData.levelIndex <= 4)
        {
            for (int i = 0; i < levelData.curBoard.cells.Count; i++)
            {
                if (levelData.curBoard.cells[i].coord == coord)
                {
                    if (levelData.curBoard.toolStatus == (int)MoonPhase.crescent_1 || levelData.curBoard.toolStatus == (int)MoonPhase.crescent_2)
                    {
                        levelData.curBoard.cells[i].value += 1;
                        levelData.curBoard.cells[i].value = BoardCalculation.ConstrainX_Range(levelData.curBoard.cells[i].value, numberRange);
                        //Play sfx ?
                    }
                    else if (levelData.curBoard.toolStatus == (int)MoonPhase.quarter_1 || levelData.curBoard.toolStatus == (int)MoonPhase.quarter_2)
                    {
                        levelData.curBoard.cells[i].value += 3;
                        levelData.curBoard.cells[i].value = BoardCalculation.ConstrainX_Range(levelData.curBoard.cells[i].value, numberRange);
                    }
                    else if (levelData.curBoard.toolStatus == (int)MoonPhase.full)
                    {
                        levelData.curBoard.cells[i].value += 5;
                        levelData.curBoard.cells[i].value = BoardCalculation.ConstrainX_Range(levelData.curBoard.cells[i].value, numberRange);
                    }
                }
            }
        }
        else if (levelData.levelIndex >= 5)
        {
            for (int i = 0; i < levelData.curBoard.cells.Count; i++)
            {
                if (levelData.curBoard.cells[i].coord == coord)
                {
                    if (levelData.curBoard.toolStatus == (int)MoonPhase.crescent_1 || levelData.curBoard.toolStatus == (int)MoonPhase.crescent_2)
                    {
                        levelData.curBoard.cells[i].value += 1;
                        levelData.curBoard.cells[i].value = BoardCalculation.ConstrainX_Range(levelData.curBoard.cells[i].value, numberRange);
                    }
                    else if (levelData.curBoard.toolStatus == (int)MoonPhase.quarter_1 || levelData.curBoard.toolStatus == (int)MoonPhase.quarter_2)
                    {
                        levelData.curBoard.cells[i].value += 3;
                        levelData.curBoard.cells[i].value = BoardCalculation.ConstrainX_Range(levelData.curBoard.cells[i].value, numberRange);
                    }
                    else if (levelData.curBoard.toolStatus == (int)MoonPhase.full && levelData.curBoard.cells[i].value == 9)
                    {
                        eclipseTrigger = true;
                        eclipseCount += 1;
                        eclipseCoord = coord;
                        moonHub.eclipseVFX.SetActive(true);
                    }
                    else if (levelData.curBoard.toolStatus == (int)MoonPhase.full)
                    {
                        levelData.curBoard.cells[i].value += 5;
                        levelData.curBoard.cells[i].value = BoardCalculation.ConstrainX_Range(levelData.curBoard.cells[i].value, numberRange);
                    }
                }
            }
        }
        else
        {
            Debug.LogError(string.Format("master script of {0} reaches undefined level", levelData.theme));
        }
    }
    public override void HandleEnvironment(Vector2Int coord)
    {
        //lv 1
        //crescent/quarter alternate
        if (levelData.levelIndex == 1)
        {
            if(levelData.curBoard.toolStatus == (int)MoonPhase.crescent_1)
            {
                levelData.curBoard.toolStatus = (int)MoonPhase.quarter_1;
            }
            else if(levelData.curBoard.toolStatus == (int)MoonPhase.quarter_1)
            {
                levelData.curBoard.toolStatus = (int)MoonPhase.crescent_1;
                fullPhaseCount += 1;
            }
        }
        //lv 2-8
        //phase rotate
        else if (levelData.levelIndex >= 2)
        {
            levelData.curBoard.toolStatus = levelData.curBoard.toolStatus + 1;
            if(levelData.curBoard.toolStatus == 6)
            {
                levelData.curBoard.toolStatus = (int)MoonPhase.crescent_1;
                fullPhaseCount += 1;
            }
        }
        else
        {
            Debug.LogError(string.Format("master script of {0} reaches undefined level", levelData.theme));
        }
        moonHub.AnimateTabletToDegree(levelData.curBoard.toolStatus);
    }
    public override void UpdateTool(Vector2Int coord)
    {
        base.UpdateTool(coord);
        //moon phase tablet rotate
        //hub.toolMaster.toolIcon.sprite = moonHub.statusSprites[levelData.curBoard.toolStatus];\
        UpdateToolStatusDisplay();
        moonHub.AddPredictedToolToCycle(levelData.curBoard.toolStatus, levelData.levelIndex);
        moonHub.AnimateCycle();
    }
    public override void AddtionalUpdate_Theme(Vector2Int coord)
    {
        if (levelData.levelIndex == 7 || levelData.levelIndex == 9)
        {
            hub.goalMaster.lines[1].SetText(string.Format("{0}{1}", LocalizedAssetLookup.singleton.Translate("@Loc=ui_goal_current_sum@@"), levelData.curBoard.CurrentSum()));
            hub.goalMaster.lines[1].gameObject.SetActive(true);
            LayoutRebuilder.ForceRebuildLayoutImmediate(hub.goalMaster.goalLayout);
        }
    }
    public override void DelayedPlay_Theme()
    {
        Vector2Int numberRange = new Vector2Int(0, 10000);
        /*
        //old eclipse
        if (eclipseTriggered)
        {
            eclipseTriggered = false;
            eclipseCoord = Vector2Int.zero;
            for (int i = 0; i < levelData.curBoard.cells.Count; i++)
            {
                levelData.curBoard.cells[i].value -= 5;
                levelData.curBoard.cells[i].value = BoardCalculation.ConstrainX_Range(levelData.curBoard.cells[i].value, numberRange);
                //levelData.curBoard.cells[i].value = BoardCalculation.ModX_Range(levelData.curBoard.cells[i].value, numberRange);
            }
            UpdateCells(eclipseCoord); 
        }
        */
        //new eclipse number change
        if (eclipseTrigger)
        {
            //eclipseTrigger = false;
            //eclipseCoord = Vector2Int.zero;
            List<int> newBoardValues = new List<int>();
            /*
            //calculate all new cell values
            for (int i = 0; i < levelData.curBoard.cells.Count; i++)
            {
                Vector2Int curCoord = levelData.curBoard.cells[i].coord;
                Vector2Int targetCoord = new Vector2Int(levelData.curBoard.boardSize.x - curCoord.x - 1, levelData.curBoard.boardSize.y - curCoord.y - 1);
                bool foundMirroredCell = false;
                for (int j = 0; j < levelData.curBoard.cells.Count; j++)
                {
                    if (levelData.curBoard.cells[j].coord == targetCoord)
                    {
                        foundMirroredCell = true;
                        newBoardValues.Add(BoardCalculation.ConstrainX_Range(Mathf.FloorToInt(levelData.curBoard.cells[j].value / 2f), numberRange));
                    }
                }
                if (!foundMirroredCell)
                {
                    newBoardValues.Add(BoardCalculation.ConstrainX_Range(Mathf.FloorToInt(levelData.curBoard.cells[i].value / 2f), numberRange));
                }
            }
            //set all cell values into new values
            for (int i = 0; i < levelData.curBoard.cells.Count; i++)
            {
                levelData.curBoard.cells[i].value = newBoardValues[i];
            }
            */
            //calculate all new cell values
            for (int i = 0; i < levelData.curBoard.cells.Count; i++)
            {
                Vector2Int curCoord = levelData.curBoard.cells[i].coord;
                Vector2Int targetCoord = new Vector2Int(levelData.curBoard.boardSize.x - curCoord.x - 1, levelData.curBoard.boardSize.y - curCoord.y - 1);
                bool foundMirroredCell = false;
                int targetValue = 0;
                for (int j = 0; j < levelData.curBoard.cells.Count; j++)
                {
                    if (levelData.curBoard.cells[j].coord == targetCoord)
                    {
                        foundMirroredCell = true;
                        targetValue = levelData.curBoard.cells[j].value;
                    }
                }
                if (!foundMirroredCell)
                {
                    targetValue = levelData.curBoard.cells[i].value;
                }
                if (targetValue == 1)
                {
                    newBoardValues.Add(0);
                }
                else if(targetValue == 3 || targetValue == 5)
                {
                    newBoardValues.Add(1);
                }
                else if (targetValue == 7 || targetValue == 9)
                {
                    newBoardValues.Add(3);
                }
                else
                {
                    newBoardValues.Add(targetValue);
                }
            }
            //set all cell values into new values
            for (int i = 0; i < levelData.curBoard.cells.Count; i++)
            {
                levelData.curBoard.cells[i].value = newBoardValues[i];
            }
            //update with a longer number shift vfx
            for (int i = 0; i < hub.boardMaster.cells.Count; i++)
            {
                DataCell temp_cellData = levelData.curBoard.GetCellDataByCoord(hub.boardMaster.cells[i].coord);
                if (temp_cellData != null)
                {
                    if (temp_cellData.value != hub.boardMaster.cells[i].curNumber)
                    {
                        hub.boardMaster.cells[i].NumberShift(temp_cellData.value, 3f);
                    }
                    //hub.boardMaster.cells[i].numberTxt.SetText(temp_cellData.value.ToString());
                }
            }
            //update sum calculation
            if (levelData.levelIndex == 7 || levelData.levelIndex == 9)
            {
                hub.goalMaster.lines[1].SetText(string.Format("{0}{1}", LocalizedAssetLookup.singleton.Translate("@Loc=ui_goal_current_sum@@"), levelData.curBoard.CurrentSum()));
                hub.goalMaster.lines[1].gameObject.SetActive(true);
                LayoutRebuilder.ForceRebuildLayoutImmediate(hub.goalMaster.goalLayout);
            }
        }
    }
    public override bool CheckWinCondition()
    {
        /*
        让所有格子为7
        让所有格子成为相同数字
        让所有格子成为相同数字
        让数字总和下降至20以下
        让左侧一列格子为0
        将所有格子变成不同值
        让所有格子回归初值
        月食时，让所有格子回归初值
         */
        bool result = false;
        if (levelData.levelIndex == 1)
        {
            //List<int> targetLineNumber = new List<int>() { 0, 8, 1, 0 };
            return BoardCalculation.CountX_All(levelData.curBoard, 7);
        }
        else if (levelData.levelIndex == 2)
        {
            //List<int> targetLineNumber = new List<int>() { 0, 8, 1, 3 };
            return BoardCalculation.Same_All(levelData.curBoard);
        }
        else if (levelData.levelIndex == 3)
        {
            //List<int> targetLineNumber = new List<int>() { 0, 8, 1, 3 };
            return BoardCalculation.Same_All(levelData.curBoard);
        }
        else if (levelData.levelIndex == 4)
        {
            //List<int> targetNumber = new List<int>() { 0, 8, 1, 5 };
            return BoardCalculation.Same_All(levelData.curBoard);
        }
        else if (levelData.levelIndex == 5)
        {
            return eclipseCount == 2;
        }
        else if (levelData.levelIndex == 6)
        {
            return eclipseCount == 3;
        }
        else if (levelData.levelIndex == 7)
        {
            return BoardCalculation.Sum_Lesser_X(levelData.curBoard, 10);
        }
        else if (levelData.levelIndex == 8)
        {
            return BoardCalculation.Even_All(levelData.curBoard);
        }
        else if (levelData.levelIndex == 9)
        {
            return BoardCalculation.Sum_Lesser_X(levelData.curBoard, 10);
        }
        else if (levelData.levelIndex == 10)
        {
            return (
                levelData.curBoard.GetCellDataByCoord(new Vector2Int(0, 0)).value == 0 && 
                levelData.curBoard.GetCellDataByCoord(new Vector2Int(0, 1)).value == 0 && 
                levelData.curBoard.GetCellDataByCoord(new Vector2Int(0, 2)).value == 0
                );
        }
        else if (levelData.levelIndex == 11)
        {
            return (
                levelData.curBoard.GetCellDataByCoord(new Vector2Int(0, 0)).value == 0 &&
                levelData.curBoard.GetCellDataByCoord(new Vector2Int(0, 1)).value == 0 &&
                levelData.curBoard.GetCellDataByCoord(new Vector2Int(0, 2)).value == 0 &&
                levelData.curBoard.GetCellDataByCoord(new Vector2Int(2, 0)).value == 2 &&
                levelData.curBoard.GetCellDataByCoord(new Vector2Int(2, 1)).value == 2 &&
                levelData.curBoard.GetCellDataByCoord(new Vector2Int(2, 2)).value == 2
                );
        }
        else if (levelData.levelIndex == 12)
        {
            return (
                levelData.curBoard.GetCellDataByCoord(new Vector2Int(0, 2)).value == 0 &&
                levelData.curBoard.GetCellDataByCoord(new Vector2Int(1, 2)).value == 2 &&
                levelData.curBoard.GetCellDataByCoord(new Vector2Int(2, 2)).value == 6 &&
                levelData.curBoard.GetCellDataByCoord(new Vector2Int(0, 1)).value == 4 &&
                levelData.curBoard.GetCellDataByCoord(new Vector2Int(1, 1)).value == 2 &&
                levelData.curBoard.GetCellDataByCoord(new Vector2Int(2, 1)).value == 4 &&
                levelData.curBoard.GetCellDataByCoord(new Vector2Int(0, 0)).value == 6 &&
                levelData.curBoard.GetCellDataByCoord(new Vector2Int(1, 0)).value == 2 &&
                levelData.curBoard.GetCellDataByCoord(new Vector2Int(2, 0)).value == 0 &&
                eclipseTrigger
                );
        }
        else if (levelData.levelIndex == 13)
        {
            return (
                levelData.curBoard.GetCellDataByCoord(new Vector2Int(0, 2)).value == 1 &&
                levelData.curBoard.GetCellDataByCoord(new Vector2Int(1, 2)).value == 2 &&
                levelData.curBoard.GetCellDataByCoord(new Vector2Int(2, 2)).value == 3 &&
                levelData.curBoard.GetCellDataByCoord(new Vector2Int(0, 1)).value == 4 &&
                levelData.curBoard.GetCellDataByCoord(new Vector2Int(1, 1)).value == 5 &&
                levelData.curBoard.GetCellDataByCoord(new Vector2Int(2, 1)).value == 6 &&
                levelData.curBoard.GetCellDataByCoord(new Vector2Int(0, 0)).value == 7 &&
                levelData.curBoard.GetCellDataByCoord(new Vector2Int(1, 0)).value == 8 &&
                levelData.curBoard.GetCellDataByCoord(new Vector2Int(2, 0)).value == 9
                );
        }
        else if (levelData.levelIndex == 14)
        {
            return (
                levelData.curBoard.GetCellDataByCoord(new Vector2Int(0, 2)).value == 1 &&
                levelData.curBoard.GetCellDataByCoord(new Vector2Int(1, 2)).value == 2 &&
                levelData.curBoard.GetCellDataByCoord(new Vector2Int(2, 2)).value == 3 &&
                levelData.curBoard.GetCellDataByCoord(new Vector2Int(0, 1)).value == 1 &&
                levelData.curBoard.GetCellDataByCoord(new Vector2Int(1, 1)).value == 2 &&
                levelData.curBoard.GetCellDataByCoord(new Vector2Int(2, 1)).value == 3 &&
                levelData.curBoard.GetCellDataByCoord(new Vector2Int(0, 0)).value == 1 &&
                levelData.curBoard.GetCellDataByCoord(new Vector2Int(1, 0)).value == 2 &&
                levelData.curBoard.GetCellDataByCoord(new Vector2Int(2, 0)).value == 3 &&
                eclipseTrigger
                );
        }
        else
        {
            Debug.LogError(string.Format("reach undefined level in CheckWinCondition of ({0})", levelData.theme));
        }
        return result;
    }
    public override bool CheckLoseCondition()
    {
        eclipseTrigger = false;
        if (levelData.curBoard.toolCount == 0)
        {
            endReason = 0;
            return true;
        }
        else if(BoardCalculation.CountXplus_Ytimes(levelData.curBoard, 10, 1) && levelData.levelIndex >= 3)
        {
            endReason = 1;
            return true;
        }
        return false;
    }
    //to do add a special lose banner of sun
    void UpdateToolStatusDisplay()
    {
        hub.toolMaster.toolIcon.sprite = moonHub.statusSprites[levelData.curBoard.toolStatus];

        ToolStatusGroup targetDisplayTemplate = moonHub.toolStatusGroup;
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
