using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LM_005_Moon : LevelMasterBase
{
    enum MoonPhase : int { none = 0, crescent_1 = 1, quarter_1 = 2, full = 3, quarter_2 = 4, crescent_2 = 5 };

    [Header("Theme Additions")]
    public LMHub_005_Moon moonHub;
    public int fullPhaseCount = 0;
    public int endReason = -1;
    public bool eclipseTriggered = false;
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
        hub.toolMaster.toolSubtitle.SetText(LocalizedAssetLookup.singleton.Translate(moonHub.toolDisplayName[levelData.curBoard.toolStatus]));
        moonHub.SetPlateWidget(false);
        //moon phase tablet
        moonHub.InitToolToCycle(levelData.curBoard.toolStatus, levelData.levelIndex);
        //moonHub.AnimateCycle();
    }
    public override void AddtionalInit_Theme()
    {
        fullPhaseCount = 0;
        endReason = -1;
        eclipseTriggered = false;
    }
    public override void DelayedInit_Theme()
    {
        //if(levelData.levelIndex >= 3)
        //{
            moonHub.SetPlateWidget(true);
            moonHub.SetTabletToDegree(0);
            moonHub.AnimateTabletToDegree(levelData.curBoard.toolStatus);
        //}
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
        else if (levelData.levelIndex >= 2 && levelData.levelIndex <= 3)
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
        else if (levelData.levelIndex >= 4 && levelData.levelIndex <= 8)
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
                        eclipseTriggered = true;
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
        hub.toolMaster.toolSubtitle.SetText(LocalizedAssetLookup.singleton.Translate(moonHub.toolDisplayName[levelData.curBoard.toolStatus]));
        moonHub.AddPredictedToolToCycle(levelData.curBoard.toolStatus, levelData.levelIndex);
        moonHub.AnimateCycle();
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
        if (eclipseTriggered)
        {
            eclipseTriggered = false;
            //eclipseCoord = Vector2Int.zero;
            List<int> newBoardValues = new List<int>();
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
        }
    }
    public override bool CheckWinCondition()
    {
        /*
        让所有格子为7
        让所有格子成为相同数字
        让所有格子成为相同数字
        让所有格子为7
        让至少7个格子为1
        让数字总和成为32
        月食时，让所有格子回归初值。
        月食时，让所有格子回归初值。
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
            //List<int> targetNumber = new List<int>() { 0, 8, 1, 5 };
            return BoardCalculation.Same_All(levelData.curBoard);
        }
        else if (levelData.levelIndex == 4)
        {
            return BoardCalculation.CountX_All(levelData.curBoard, 7);
        }
        else if (levelData.levelIndex == 5)
        {
            return BoardCalculation.CountX_Ytimes(levelData.curBoard, 1, 7);
        }
        else if (levelData.levelIndex == 6)
        {
            return BoardCalculation.Sum_As_X(levelData.curBoard, 32);
        }
        else if (levelData.levelIndex == 7)
        {
            //todo 
            return false;
        }
        else if (levelData.levelIndex == 8)
        {
            //todo 
            return false;
        }
        else
        {
            Debug.LogError(string.Format("reach undefined level in CheckWinCondition of ({0})", levelData.theme));
        }
        return result;
    }
    public override bool CheckLoseCondition()
    {
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
    
}
